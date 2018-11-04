public class PlayerChoises : MonoBehaviour
{
    //Class for saving refrences for player Track, Car and Minimap Icon choises.
    //Is plased in Car/level select scene inside hierarchy as empty object named PlayerChoises.
    public static PlayerChoises m_instance = null;

    public int m_levelNumber = 0;                           //Nubler value of chosen Race track.
    [SerializeField]static public int m_playerAmount =1;    //How many players have confirmed their choises.
    static public int m_playerJoinedAmount =0;              //How many players have joined into game.

    private int m_restart = 0;

    public int m_chosenCar1 = 0;                            //Player 1 Car choise.
    public int m_chosenCar2 = 0;                            //Player 2 Car choise.
    public int m_chosenCar3 = 0;                            //Player 3 Car choise.
    public int m_chosenCar4 = 0;                            //Player 4 Car choise.
    public int m_chosenCar5 = 0;                            //Player 5 Car choise.

    public int m_chosenIcon1 = 0;                           //Player 1 Minimap icon choise.
    public int m_chosenIcon2 = 0;                           //Player 2 Minimap icon choise.
    public int m_chosenIcon3 = 0;                           //Player 3 Minimap icon choise.
    public int m_chosenIcon4 = 0;                           //Player 4 Minimap icon choise.
    public int m_chosenIcon5 = 0;                           //Player 5 Minimap icon choise.

    public string m_player1input;                           //Player 1 Controller scheme saved as string value.
    public string m_player2input;                           //Player 2 Controller scheme saved as string value.
    public string m_player3input;                           //Player 3 Controller scheme saved as string value.
    public string m_player4input;                           //Player 4 Controller scheme saved as string value.

    static public bool m_player1Confirmed = false;          //Player 1 slot Choises confirmed and locked.
    static public bool m_player2Confirmed = false;          //Player 2 slot Choises confirmed and locked.
    static public bool m_player3Confirmed = false;          //Player 3 slot Choises confirmed and locked.
    static public bool m_player4Confirmed = false;          //Player 4 slot Choises confirmed and locked.
                     
    static public bool m_player1Joined = false;             //Player joined Player 1 slot. 
    static public bool m_player2Joined = false;             //Player joined Player 2 slot.
    static public bool m_player3Joined = false;             //Player joined Player 3 slot.
    static public bool m_player4Joined = false;             //Player joined Player 4 slot.

    static public bool m_canvas = false;                             //Car select start rase info canvas.
    static public bool m_ConfirmCanvas = false;                             //Car select start rase info canvas.
    static public bool m_inRace = false;                          //Boolean lock for starting race after choises are confirmed.




    // Awake is called once when this awakws.
    void Awake ()
    {
        if (m_instance == null)                           //Preventing from destroying player choises between scenes.
        {
            m_instance = this;
        }
        else if (m_instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        m_canvas = GameObject.Find("PressStartPanel");      //Finding correct canvas in car selection scene.
                
    }
            
    // Update is called once per frame
    void Update() 
    {
            
        if (m_inRace == false)                              //If race has started or different amount of player slots have joined and confirmed race can't start.
        {
            if ((m_playerAmount == m_playerJoinedAmount) && (m_playerAmount > 0))
            {
                m_canvas = true;                   //When coditions are met players are prompted to start the race.
                m_ConfirmCanvas = false;
                if (Input.GetButtonDown("StartRace"))
                {
                    LoadLevel(m_levelNumber);               //Calling method to load chosen scene.
                    m_restart = m_levelNumber;
                }
            }
            else
            {
                m_canvas = false;                  //Disabeling player promt.
                m_ConfirmCanvas = true;
            }
        }
    }

    //Method to save Level selection input.
    public void LevelChoice(int Levelnumber)
    {
        m_levelNumber = Levelnumber;
    }

    //Saves choises for playerslot 1 in track/car selection scene
    public void ChangeChoisesP1(string CN,int Car,int Icon,bool confirmed)
    {
        m_player1input = CN;  
        m_chosenCar1 = Car+1;
        m_chosenIcon1 = Icon+1;
        m_player1Confirmed = confirmed;

    }

    //Gives ChangeChoisesP1 values for player 1 in race scene.
    public void GiveChoisesP1()
    {
        GameObject.Find("Player1").GetComponent <DLPlayerSettings>().GetValues(m_chosenCar1,m_chosenIcon1,m_player1input);
        print("P1 choises: "+ m_chosenCar1+","+m_chosenIcon1+","+m_player1input);

    }

    //Saved choises for playerslot 2 in track/car selection scene
    public void ChangeChoisesP2(string CN,int Car,int Icon,bool confirmed)
    {
        m_player2input = CN;
        m_chosenCar2 = Car+1;
        m_chosenIcon2 = Icon+1;
        m_player1Confirmed = confirmed;
    }
    //Gives ChangeChoisesP2 values for player 2 in race scene.
    public void GiveChoisesP2()
    {
        GameObject.Find("Player2").GetComponent <DLPlayerSettings>().GetValues(m_chosenCar2,m_chosenIcon2,m_player2input);
        print("P2 choises: "+ m_chosenCar2+","+m_chosenIcon2+","+m_player2input);

    }

    //Saved choises for playerslot 3 in track/car selection scene
    public void ChangeChoisesP3(string CN,int Car,int Icon,bool confirmed)
    {
        m_player3input = CN;
        m_chosenCar3 = Car+1;
        m_chosenIcon3 = Icon+1;
        m_player1Confirmed = confirmed;
    }

    //Gives ChangeChoisesP3 values for player 3 in race scene.
    public void GiveChoisesP3()
    {
        GameObject.Find("Player3").GetComponent <DLPlayerSettings>().GetValues(m_chosenCar3,m_chosenIcon3,m_player3input);

    }

    //Saved choises for playerslot 4 in track/car selection scene
    public void ChangeChoisesP4(string CN,int Car,int Icon,bool confirmed)
    {
        m_player4input = CN;
        m_chosenCar4 = Car+1;
        m_chosenIcon4 = Icon+1;
        m_player1Confirmed = confirmed;
    }

    //Gives ChangeChoisesP4 values for player 4 in race scene.
    public void GiveChoisesP4()
    {
        GameObject.Find("Player4").GetComponent <DLPlayerSettings>().GetValues(m_chosenCar4,m_chosenIcon4,m_player4input);

    }

    //Method to select correct race track from scenes in build.
    public void LoadLevel(int level)//Load correct level using the level number recorded fron level selection script.
    {
        if (level == 1)//If level number was 1 load forbidden canyon
        {
            print("ForbiddenCanyon");
            GameObject.Find("Canvas").GetComponent<DLFadeManager>().GoToCanyon();
            m_inRace = true;   
        }

        else if (level == 2)//If level number was 2 load New malenon city 
        {
            print("NewMalenonCity");
            GameObject.Find("Canvas").GetComponent<DLFadeManager>().GoToNMC();
            m_inRace = true;   
        }

        else
        {
            print("Error with level int");
        }
    }

    public void Restart()
    {
        LoadLevel(m_restart);
    }

}


