public class CarSelection : MonoBehaviour
{
    //This script handels players vehicle selections.
    public int m_playerNum;                                 //Player number to assingn players position from 1 to 4.
    [SerializeField] private GameObject m_spotlight;        //Spotlight gameobject that lights up when selection has been confirmed.
    [SerializeField] private GameObject[] m_carList;        //Array of possible vehicle choises.
    [SerializeField] private GameObject[] m_iconList;       //Array of possible minimap icons players can choose.
    public int m_carIndex;                                  //Active vehicle shown from array.
    public int m_iconIndex;                                 //Active icon shown from array.
    private float m_up;                                     //Float used to gain input from analog sticks up down movement in steps.
    private float m_left;                                   //Float used to gain input from analog sticks left right movement in steps.
    private bool m_toggleCar = false;                       //Bool to prevent vehicle choices from scrolling too fast.
    private bool m_toggleIcon = false;                      //Bool to prevent icon choices from scrolling too fast.
    private bool m_confirm = false;                         //Has player confirmed choices.
    private bool m_letConfirm = false;                      //Is player allowed to confirm choices.
    [SerializeField] private string m_CN;                   //String that represents controller from input manager. Player 1 to 4 inputs are assingned for gamepads and player 5 and 6 are different keyboard inputs.

    private void Start()
    {
        //Player can't confirm without making selections first.
        m_letConfirm = false;
        //Spotlight is off.
        m_spotlight.SetActive(false);

        //Set all Vehicles and icons inactive.
        foreach (GameObject go in m_carList)
        {
            go.SetActive(false);
        }
        foreach (GameObject go in m_iconList)
        {
            go.SetActive(false);
        }
    
        //Set first choices active.
        if (m_carList[m_carIndex])
        {
            m_carList[m_carIndex].SetActive(true);
        }
        if (m_iconList[m_iconIndex])
        {
            m_iconList[m_iconIndex].SetActive(true);
        }
    }

    void Update()
    {
        //Controller number info is set from PlayerSlot script.
        m_CN = PlayerSlot.m_playerslots[m_playerNum-1];

        //If player is using gamepad as a control method.
        if (m_CN == "Player1"||m_CN == "Player2"||m_CN == "Player3"||m_CN == "Player4")
        {
            //Change up and left values with raw input from analog joystick. 
            m_up = Input.GetAxisRaw(m_CN+"Vertical");
            m_left = Input.GetAxisRaw(m_CN+"Horizontal");
             
            //If up value is higher than 0.2f and player hasn't confirmed choises change to next icon image.
            if (m_up > 0.2f && m_toggleIcon == false && m_confirm == false)
            {
                StartCoroutine(Up());
                m_toggleIcon = true;
            }

            //If up value is lower than 0.2f and player hasn't confirmed choises change to previous icon image.
            else if (m_up < -0.2f && m_toggleIcon == false && m_confirm == false)
            {
                StartCoroutine(Down());
                m_toggleIcon = true;
            }

            //If left value is higher than 0.2f and player hasn't confirmed choises change to next vehicle on list.
            if (m_left > 0.2f && m_toggleCar == false && m_confirm == false)
            {
                StartCoroutine(Left());
                m_toggleCar = true;
            }

            //If left value is lower than 0.2f and player hasn't confirmed choises change to previous vehicle on list.
            else if (m_left < -0.2f && m_toggleCar == false && m_confirm == false)
            {
                StartCoroutine(Right());
                m_toggleCar = true;
            }

            //To prevent player instantly confirming car choise.
            if (m_letConfirm == false && Input.GetButtonDown(m_CN+"Drift")) 
            {  
                StartCoroutine(WaitToConfirm());
            }

            //Player confirming car choise.
            else if (m_letConfirm == true && Input.GetButtonDown(m_CN+"Drift") && m_confirm == false) 
            {                
                Confirm();
                GameObject.Find("Playerslot").GetComponent <PlayerSlot>().RemoveRotSpeed(1);
                m_confirm = true;
            }


            //Player un-confirming car choise.
            if (Input.GetButtonDown(m_CN+"Boost")&& m_confirm == true) 
            {                
                Release();
                GameObject.Find("Playerslot").GetComponent <PlayerSlot>().AddRotSpeed(1);

            }

            //Player removed from active players.
            else if (Input.GetButtonDown(m_CN+"Boost")&& m_confirm == false)
            {
                    
                GameObject.Find("Playerslot").GetComponent <PlayerSlot>().RemoveplayerFromSlot(m_playerNum - 1);                  
            }
                    
        }

        //Else if player is using keyboard as a control method.
        else if (m_CN == "Player5"||m_CN == "Player6")
        {
            m_up = Input.GetAxisRaw(m_CN+"Vertical");
            m_left = Input.GetAxisRaw(m_CN+"Horizontal");

            //If up value is higher than 0.2f and player hasn't confirmed choises change to next icon image.
            if (m_up > 0.2f && m_toggleIcon == false && m_confirm == false)
            {
                StartCoroutine(Up());
                m_toggleIcon = true;
            }

            //If up value is lower than 0.2f and player hasn't confirmed choises change to previous icon image.
            else if (m_up < -0.2f && m_toggleIcon == false && m_confirm == false)
            {
                StartCoroutine(Down());
                m_toggleIcon = true;
            }

            //If left value is higher than 0.2f and player hasn't confirmed choises change to next vehicle on list.
            if (m_left > 0.2f && m_toggleCar == false && m_confirm == false)
            {
                StartCoroutine(Left());
                m_toggleCar = true;
            }

            //If left value is lower than 0.2f and player hasn't confirmed choises change to previous vehicle on list.
            else if (m_left < -0.2f && m_toggleCar == false && m_confirm == false)
            {
                StartCoroutine(Right());
                m_toggleCar = true;
            }

            //To prevent player instantly confirming car choise.
            if (m_letConfirm == false && Input.GetButtonDown(m_CN+"Drift"))
            {                
                StartCoroutine(WaitToConfirm());
            }

            //Player confirming car choise.
            else if (m_letConfirm == true && Input.GetButtonDown(m_CN+"Drift") && m_confirm == false)
            {                
                Confirm();
                GameObject.Find("Playerslot").GetComponent <PlayerSlot>().RemoveRotSpeed(1);
                m_confirm = true;
            }

            //Player un-confirming car choise.
            if (Input.GetButtonDown(m_CN+"Deselect")&& m_confirm == true)
            {                
                Release();
                GameObject.Find("Playerslot").GetComponent <PlayerSlot>().AddRotSpeed(1);
                m_letConfirm = false;
                m_confirm = false;
            }

            //Player removed from active players.
            if (Input.GetButtonDown(m_CN+"Deselect")&& m_confirm == false)
            {     
                GameObject.Find("Playerslot").GetComponent <PlayerSlot>().RemoveplayerFromSlot(m_playerNum - 1);
            }
        }
    }

    public void ToggleLeft()
    {
        m_carList[m_carIndex].SetActive(false);
        m_carIndex--;
        if (m_carIndex < 0)
            m_carIndex = m_carList.Length - 1;
        m_carList[m_carIndex].SetActive(true);
    }

    public void ToggleRight()
    {
        m_carList[m_carIndex].SetActive(false);
        m_carIndex++;
        if (m_carIndex == m_carList.Length)
            m_carIndex = 0;
    
        m_carList[m_carIndex].SetActive(true);
    }
    public void ToggleUp()
    {
        m_iconList[m_iconIndex].SetActive(false);
        m_iconIndex--;
        if (m_iconIndex < 0)
            m_iconIndex = m_iconList.Length - 1;
        m_iconList[m_iconIndex].SetActive(true);
    }

    public void ToggleDown()
    {
        m_iconList[m_iconIndex].SetActive(false);
        m_iconIndex++;
        if (m_iconIndex == m_iconList.Length)
            m_iconIndex = 0;

        m_iconList[m_iconIndex].SetActive(true);
    }
    
    
    private IEnumerator Up()
    {
        ToggleUp();
        yield return new WaitForSeconds(0.5f);
        m_toggleIcon = false;
    }
    private IEnumerator Down()
    {
        ToggleDown();
        yield return new WaitForSeconds(0.5f);
        m_toggleIcon = false;
    }
    private IEnumerator Right()
    {
        ToggleRight();
        yield return new WaitForSeconds(0.5f);
        m_toggleCar = false;
    }
    private IEnumerator Left()
    {
        ToggleLeft();
        yield return new WaitForSeconds(0.5f);
        m_toggleCar = false;
    }

    //Confirmation of players choises is saved to PlayerChoises that is not destroyed between scenes.
    public void Confirm()
    {
        print("confirmed");
        m_spotlight.SetActive(true);
             
        if (m_playerNum == 1)
        {
            GameObject.Find("PlayerChoises").GetComponent <PlayerChoises>().ChangeChoisesP1(m_CN,m_carIndex,m_iconIndex,true);
            PlayerChoises.m_playerAmount++;

        }
        else if (m_playerNum == 2)
        {
            GameObject.Find("PlayerChoises").GetComponent <PlayerChoises>().ChangeChoisesP2(m_CN,m_carIndex,m_iconIndex,true);
            PlayerChoises.m_playerAmount++;
        }
        else if (m_playerNum == 3)
        {
            GameObject.Find("PlayerChoises").GetComponent <PlayerChoises>().ChangeChoisesP3(m_CN,m_carIndex,m_iconIndex,true);
            PlayerChoises.m_playerAmount++;
        }
        else if (m_playerNum == 4)
        {
            GameObject.Find("PlayerChoises").GetComponent <PlayerChoises>().ChangeChoisesP4(m_CN,m_carIndex,m_iconIndex,true);
            PlayerChoises.m_playerAmount++;
        }

    }

    //Remove of players choises that are saved to PlayerChoises.
    public void Release()
    {
        m_spotlight.SetActive(false);
        m_letConfirm = false;
        m_confirm = false;
        if (m_playerNum == 1)
        {
            GameObject.Find("PlayerChoises").GetComponent <PlayerChoises>().ChangeChoisesP1("empty",-1,-1,false);
            PlayerChoises.m_playerAmount--;
        }
        else if (m_playerNum == 2)
        {
            GameObject.Find("PlayerChoises").GetComponent <PlayerChoises>().ChangeChoisesP2("empty",-1,-1,false);
            PlayerChoises.m_playerAmount--;
        }
        else if (m_playerNum == 3)
        {
            GameObject.Find("PlayerChoises").GetComponent <PlayerChoises>().ChangeChoisesP3("empty",-1,-1,false);
            PlayerChoises.m_playerAmount--;
        }
        else if (m_playerNum == 4)
        {
            GameObject.Find("PlayerChoises").GetComponent <PlayerChoises>().ChangeChoisesP4("empty",-1,-1,false);
            PlayerChoises.m_playerAmount--;
        }
    }

    //Wait so that player doesn't accidentally lock choises when joining in.
    IEnumerator WaitToConfirm()
    {
        yield return new WaitForSeconds(0.1f);
        m_letConfirm = true;
    }
}

