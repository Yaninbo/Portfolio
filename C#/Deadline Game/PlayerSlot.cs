using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Deadline
{
    public class PlayerSlot : MonoBehaviour
    {
        //PlayerSlot handels adding and removing players from game and also assining correct controls to every player. 

        public string m_playerSlot;
        private string m_empty ="empty"; //System.Array.IndexOf didn't accept string "empty" but m_empty worked so this was created.

        private int m_PlayerNum;        //m_PlayerNum or Player Number is used to identify which player joined first, scound, third, and fourth 
        private int m_ControllerNum;    //m_Controllernum or Controller number is used to see which controller was used to joun game and assing that to correct Player Number.

        static public string[] m_playerslots = new string[]{ "empty", "empty", "empty", "empty" };      //playerslots array is used to assing control schemes as strings to player slots


        [SerializeField] private GameObject m_carList1; //GameObject that has all available car models as child objects to show cars in car selection for player slot 1. 
        [SerializeField] private GameObject m_carList2; //GameObject that has all available car models as child objects to show cars in car selection for player slot 2. 
        [SerializeField] private GameObject m_carList3; //GameObject that has all available car models as child objects to show cars in car selection for player slot 3. 
        [SerializeField] private GameObject m_carList4; //GameObject that has all available car models as child objects to show cars in car selection for player slot 4. 

        [SerializeField] private GameObject m_iconList1; //GameObject that has all available minimap icons as child objects to show icons in car selection for player slot 1.
        [SerializeField] private GameObject m_iconList2; //GameObject that has all available minimap icons as child objects to show icons in car selection for player slot 2.
        [SerializeField] private GameObject m_iconList3; //GameObject that has all available minimap icons as child objects to show icons in car selection for player slot 3.
        [SerializeField] private GameObject m_iconList4; //GameObject that has all available minimap icons as child objects to show icons in car selection for player slot 4.

        [SerializeField] private GameObject m_pressToJoin1; //GameObject that has all available minimap icons as child objects to show icons in car selection for player slot 1.
        [SerializeField] private GameObject m_pressToJoin2; //GameObject that has all available minimap icons as child objects to show icons in car selection for player slot 2.
        [SerializeField] private GameObject m_pressToJoin3; //GameObject that has all available minimap icons as child objects to show icons in car selection for player slot 3.
        [SerializeField] private GameObject m_pressToJoin4; //GameObject that has all available minimap icons as child objects to show icons in car selection for player slot 4.

        public GameObject m_pressToConfirm1; //GameObject that has all available minimap icons as child objects to show icons in car selection for player slot 1.
        public GameObject m_pressToConfirm2; //GameObject that has all available minimap icons as child objects to show icons in car selection for player slot 2.
        public GameObject m_pressToConfirm3; //GameObject that has all available minimap icons as child objects to show icons in car selection for player slot 3.
        public GameObject m_pressToConfirm4; //GameObject that has all available minimap icons as child objects to show icons in car selection for player slot 4.

        public GameObject m_startGameCanvas;    //Canvas that shows players that race can be started.
        public GameObject m_confirmCanvas;    //Canvas that shows players that car needs to be confirmed.

        static public int m_rotationspeed = 1;  //Speed of which cars and car platforms rotate in car selection to add some visual flare.
                                                //Actual rotation is handeld in rotation script attached to trueCarList in hierarchy.

    
        // Use this for initialization
        void Start()
        {
            //Base rotation speed.
            m_rotationspeed = 1;

            //Turns all carLists off if they weren't already.
            ShowCar();                          
        }
        
        // Update is called once per frame
        void Update()
        {
            //If someone presses Submit button on any acceptable controller.
            if (Input.GetButtonDown("Submit"))
            {
                //Controller number and slot name are quickly reseted.
                m_ControllerNum = 0;
                m_playerSlot = ("empty");

                //If Submit button was pressed from joystick 1 which input is named "Player1Drift" then Controller number is 1.
                if (Input.GetButtonDown("Player1Drift"))
                {
                    m_ControllerNum = 1;
                }
                //If Submit button was pressed from joystick 2 which input is named "Player2Drift" then Controller number is 2.
                else if (Input.GetButtonDown("Player2Drift"))
                {
                    m_ControllerNum = 2;
                }
                //If Submit button was pressed from joystick 3 which input is named "Player3Drift" then Controller number is 3.
                else if (Input.GetButtonDown("Player3Drift"))
                {
                    m_ControllerNum = 3;
                }
                //If Submit button was pressed from joystick 4 which input is named "Player4Drift" then Controller number is 4.
                else if (Input.GetButtonDown("Player4Drift"))
                {
                    m_ControllerNum = 4;

                }
                //If Submit button was pressed from joystick 5 which input is named "Player5Drift" then Controller number is 5.
                else if (Input.GetButtonDown("Player5Drift"))
                {
                    m_ControllerNum = 5;

                }
                //If Submit button was pressed from joystick 6 which input is named "Player6Drift" then Controller number is 6.
                else if (Input.GetButtonDown("Player6Drift"))
                {
                    m_ControllerNum = 6;
                }
                 
                //According to which input was given correct string is attached to m_playerslot.
                switch (m_ControllerNum)
                {
                    case 1:
                        m_playerSlot = "Player1";
                        break;
                    case 2:
                        m_playerSlot = "Player2";
                        break;
                    case 3:
                        m_playerSlot = "Player3";
                        break;
                    case 4:
                        m_playerSlot = "Player4";
                        break;
                    case 5:
                        m_playerSlot = "Player5";
                        break;
                    case 6:
                        m_playerSlot = "Player6";
                        break;
                    default:
                        break;
                }

                //Player is added to car selection slot with correct input string.
                AddplayerToSlot(m_playerSlot);

                //Players Car and Icon lists turn on and are interactable.
                ShowCar();                    
            }

            //If all playerslots are empty and any acceptable controller presses cancel game returns to level selection from car selection.
            if ((m_playerslots[0] == "empty")&&(m_playerslots[1] == "empty")&&(m_playerslots[2] == "empty")&&(m_playerslots[3] == "empty") && Input.GetButtonDown ("Cancel"))
            {
                GameObject.Find("SceneManager").GetComponent <SceneMovementManager >().NextSelection(5);

            }

            //If PlayerChoises tels that canvas should be active then set canvas active
            if (PlayerChoises.m_canvas == true)
            {
                m_startGameCanvas.SetActive(true);
                m_confirmCanvas.SetActive (false);
            }

            //Else don't set canvas active.
            else if (PlayerChoises.m_canvas == false)
            {
                m_startGameCanvas.SetActive(false);
                m_confirmCanvas.SetActive (true);
            }
        }

        //AddplayerToSlot method takes the string value that was called in update and assings it to slot in playerslots array if conditions are met.
        void AddplayerToSlot(string playerCase)
        {
            //if PlayerSlots array already contains the same string that method is trying to assing as playercase or there isn't any empty slots available in array player isn't assinged to slot.
            if (!System.Array.Exists<string>(m_playerslots, element => element == playerCase) && System.Array.Exists<string>(m_playerslots,element => element == m_empty))
            {
                //CN is short for Controllernumber and is assingned integer index value of first arrayslot that is empty.
                int CN = System.Array.IndexOf<string>(m_playerslots, m_empty);

                //That index of array gets its "empty" string replased wit playerCase string.
                m_playerslots[CN] = playerCase;

                //Rotation speed is increased for visual effect.
                AddRotSpeed(1);

                //Integer that keeps track of how many players have joined game increases 1 for each player joined.
                PlayerChoises.m_playerJoinedAmount++; 
            }
        }

        //RemoveplayerFromSlot method takes players slot index value and replaces playerCase string with "empty".4
        public void RemoveplayerFromSlot(int playerNumber)
        {
            //If playerslot array position of playerNumber is not "empty".
            if ( m_playerslots[playerNumber] != "empty")
            {
                //player slots array index position of playernumber ir replaced with "empty".
                m_playerslots[playerNumber] = "empty";

//                m_playerConfirmed[playerNumber] = "empty";

                //Rotation speed is decreased for visual effect.
                RemoveRotSpeed(1);

                //Integer that keeps track of how many players have joined game decreases 1 for each player removed.
                PlayerChoises.m_playerJoinedAmount--; 

                //Players Car and Icon lists turn off and are not interactable.
                ShowCar();
            }
        }

        //Method that removes all previous choises in case players visit level selection after car selection.
        public void FlushSlot(int playerNumber)
        {
            m_playerslots[playerNumber] = "empty";
//            m_playerConfirmed[playerNumber] = "empty";
            print("Player"+ playerNumber  +" controller: "+ m_playerslots[playerNumber]);
            m_rotationspeed = 1;
            ShowCar();
        }

        //Method that checks if playerslot array positions have "empty" string
        //If slot has string "empty" it turns off car and icon lists for that slot and turns playerjoined boolean to false in player choises.
        //If slot has anything but "empty" it turns on Car and Icon lists for that slot and turns playerjoined boolean to true in player choises.
        void ShowCar()
        {
            if (m_playerslots[0] == "empty")
            {
                m_carList1.SetActive(false);
                m_iconList1.SetActive(false);
                m_pressToJoin1.SetActive(true);
                m_pressToConfirm1.SetActive(false);
                PlayerChoises.m_player1Joined = false;
            }

            else if(m_playerslots[0] != "empty")
            {
                m_carList1.SetActive(true);
                m_iconList1.SetActive(true);
                m_pressToJoin1.SetActive(false);
                m_pressToConfirm1.SetActive(true);
                PlayerChoises.m_player1Joined = true;
            } 

            if (m_playerslots[1] == "empty")
            {
                m_carList2.SetActive(false);
                m_iconList2.SetActive(false);
                m_pressToJoin2.SetActive(true);
                m_pressToConfirm2.SetActive(false);
                PlayerChoises.m_player2Joined = false;
            }
            else if(m_playerslots[1] != "empty")
            {
                m_carList2.SetActive(true);
                m_iconList2.SetActive(true);
                m_pressToJoin2.SetActive(false);
                m_pressToConfirm2.SetActive(true);
                PlayerChoises.m_player2Joined = true;
            }  
            if (m_playerslots[2] == "empty")
            {
                m_carList3.SetActive(false);
                m_iconList3.SetActive(false);
                m_pressToJoin3.SetActive(true);
                m_pressToConfirm3.SetActive(false);
                PlayerChoises.m_player3Joined = false;
            }
            else if(m_playerslots[2] != "empty")
            {
                m_carList3.SetActive(true);
                m_iconList3.SetActive(true);
                m_pressToJoin3.SetActive(false);
                m_pressToConfirm3.SetActive(true);
                PlayerChoises.m_player3Joined = true;
            }  
            if (m_playerslots[3] == "empty")
            {
                m_carList4.SetActive(false);
                m_iconList4.SetActive(false);
                m_pressToJoin4.SetActive(true);
                m_pressToConfirm4.SetActive(false);
                PlayerChoises.m_player4Joined = false;
            }
            else if(m_playerslots[3] != "empty")
            {
                m_carList4.SetActive(true);
                m_iconList4.SetActive(true);
                m_pressToJoin4.SetActive(false);
                m_pressToConfirm4.SetActive(true);
                PlayerChoises.m_player4Joined = true;
            }  
        }

        //Method that adds rotation speed to car turning in rotation script.
        public void AddRotSpeed(int speed)
        {
            m_rotationspeed +=speed;
        }

        //Method that decreases rotation speed to car turning in rotation script.
        public void RemoveRotSpeed(int speed)
        {
            m_rotationspeed-=speed;
        }
    }
}
