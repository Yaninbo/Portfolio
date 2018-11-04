public class DiscDestroy : MonoBehaviour
{
    //This script handels destruction of thrownd disc, players movement after throw, addition of score and loading of next level.

    [HideInInspector] public static int m_scorepoints;      //Current levels score in integer.
    [HideInInspector] public static int m_totalscorepoints; //Cources total score in integer.
    [HideInInspector] public static int m_SceneNum;         //Integer that keeps track of what is current level on scorecard.
    private int m_caseNumReseved;                           //Gotten from DiscBase scipt.

    public GameObject m_disc;                               //GameObject variable named m_disc.
    private GameObject m_ActiveBasket;                      //Reference to current levels goal basket.
    private GameObject m_player;                            //Reference to player.

    public string m_LevelName;                              //Next scenes name.

    private float m_startTime;                              //Start time is used to keep track of if current throw is taking too long.    
    private float m_killTime = 13f;                         //Kill time is used to adjust how long disc is allowed to stay before giving player new throw. 

    private bool m_Win = false;                             //Boolean representing if current level has been finished.
    private bool m_thisSleeps;                              //Variable to save the boolean value about disc's stationary state.
    private bool m_hasTicked  = false;                      //Variable to check if next throw has been given.

    private Rigidbody m_rigidDisc;                          //Rigidbody type variable named rigidDisc.
    private Transform m_playerTrans;                        //Reference to player transform so that player can be teleported under correct circumstances.
    private Scorecard m_scoreCard;                          //Reference to scorecard so that it can be updated after throws.

	void Awake()
	{
        //Find goal of the current level.
		m_ActiveBasket = GameObject.Find("EndlevelBasket");
        //Find player in current level.
		m_player = GameObject.Find("Players");
        //Get start time.
		m_startTime = Time.time;
	}

    void Update()
	{
        //Get next levels name in case throw lands in to victory trigger area.
		m_LevelName = m_ActiveBasket.GetComponentInChildren<EndLevel>().LevelName;	
        //Get player transform component.
        m_playerTrans = m_player.GetComponent<Transform>();
        //Start counting how long disc has been moveing.
		float timeAlive = Time.time - m_startTime;
        //Get the disc's rigidbody component
        m_rigidDisc = DiscBase.GetComponent<Rigidbody>(); 
        //Check if disc's rigidbody is staionary.
        m_thisSleeps = m_rigidDisc.IsSleeping();

        //If disc has stopped and hasn't been active too long.
        if (m_thisSleeps == true && m_hasTicked == false)
        {  
            //Set Throw Active Notification off in notifications script.
            //Get current landing trigger area info from DiscBase script.
            m_caseNumReseved = GetComponentInChildren <DiscBase>().m_caseNum;      

            //Give player next throw
            NextThrow();

            //Prevent NextThrow() happening twise in a row.
            m_hasTicked = true;
				
        }

        //Else if DiscRestriction allowes new throw change m_hasTicked back to false.
        else if (DiscRestriction.m_canthrow == true)
        {
            m_hasTicked = false;
        }

        //Else if player landed on goal trigger zone, start loading on next level. 
        else if(m_Win == 1)
        {
            StartCoroutine ("Wait");
        }

        //Else if disc has been alive too long, Destroy disc and give player new throw with out penalty.
		else if(timeAlive > m_killTime)
		{
			Destroy(gameObject);               
			DiscRestriction.m_canthrow = true;
		}
		
    }

    //Method that handles where next throw is starting and how many points are added to scorecard.
	void NextThrow ()
	{
        switch (m_caseNumReseved)
        {
            case 1: 
                //OUT
                //Point added to scorecard and player is not moved, but player can throw again.
                AddPoints(1);
                Destroy(gameObject);
                DiscRestriction.m_canthrow = true;
                break;

            case 2: 
                //Basket
                //Player got to the end of level. Point added to score card. Player is not moved and cannot throw again. m_Win boolean is turned to true.
                AddPoints(1);
                m_Win = true;
                DiscRestriction.m_canthrow = false;
                break;

            case 3:
                //NearBasket
                //Player got to the end of level. Two points added to score card. Player is not moved and cannot throw again. m_Win boolean is turned to true.
                AddPoints(2);
                m_Win = true;
                DiscRestriction.m_canthrow = false;
                break;

            case 4:
                //Normal throw
                //Point added to scorecard and player is moved to where last thrown disc landed. Player can throw again.
                AddPoints(1);
                m_playerTrans.position = rigidBullet.position - new Vector3(0f, 0f, 1f);
                Destroy(gameObject);
                DiscRestriction.m_canthrow = true;
                break;

            default:
                //Error situation
                //If script gets here the disc is destroyed and new throw is allowed without penalty.
                Destroy(gameObject);               
                DiscRestriction.m_canthrow = true;
                break;
        }
    }

    //Method to load next scene after goal has been reached.
	IEnumerator Wait()
	{            
		yield return new WaitForSeconds (3);
		SceneManager.LoadScene (m_LevelName);
        m_SceneNum++;
        m_scorepoints = 0;
	}

    //Addition of points to scorecard after throws.
    public void AddPoints(int point)
    {
        m_scorepoints +=point;
        m_totalscorepoints +=point;
    }
}