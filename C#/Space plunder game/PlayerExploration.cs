public class PlayerExploration : MonoBehaviour
{
    //Script to handle players exploration phaze in Space plunder.

    private Vector3 m_nextPosition;                             //Reference to the next position player is trying to move.
    private Vector3 m_movementDirection;                        //Direction of movement.
    private GameObject m_currentEnemy;                          //Reference to collided enemy.
    public float m_rayRange = 1f;                               //Raycast components range.
    [SerializeField] private GameObject m_starfighter;          //Player model.
    [SerializeField] private GameObject m_UI;                   //Reference to Main Canvas.
    [SerializeField] private TextMeshProUGUI m_TreasureText;    //Reference to on screen treasure piece counter.
    [SerializeField] private int m_playerMovementAmmount = 1;   //Integer number of how many steps player can move in one turn.
    [SerializeField] private int m_TreasurePartsFound = 0;      //Number of treasure pieces found.
    public bool m_playerNextTurn;                               //Boolean that allows player to move.
    private bool m_playerSpawned;                               //Boolean that prevent player from being moved in to starting tile after game has started.                     
    static public bool m_battleStarted;                         //Boolean that keeps track if player is in combat.
    private bool m_LLRinUse = false;                            //Boolean to check if Long radar is currently in use.
    private bool m_tresureFound;                                //Boolean to check if all treasure pieces have been found.

    //Tutorial booleans
    private bool m_firstSJHP = false;                           //Boolean to check if Space junk tutorial has been shown.
    private bool m_firstSJAC = false;                           //Boolean to check if Ammo crate tutorial has been shown.
    private bool m_firstSJMS = false;                           //Boolean to check if Meteor storm tutorial has been shown.
    private bool m_firstLRR = false;                            //Boolean to check if Long radar tutorial has been shown.
    private bool m_firstSRR = false;                            //Boolean to check if Short radar tutorial has been shown.
    private bool m_firstMap = false;                            //Boolean to check if Treasure map tutorial has been shown.
    private bool m_firstPlanet = false;                         //Boolean to check if Planet tutorial has been shown.
    public bool m_enemyTutorial1 = false;                       //Boolean to check if Enemy 1 tutorial has been shown.
    public bool m_enemyTutorial2 = false;                       //Boolean to check if Enemy 2 tutorial has been shown.
    public bool m_enemyTutorial3 = false;                       //Boolean to check if Enemy 3 tutorial has been shown.
    public bool m_enemyTutorial4 = false;                       //Boolean to check if Enemy 4 tutorial has been shown.
    public bool m_enemyTutorial5 = false;                       //Boolean to check if Enemy 5 tutorial has been shown.

    [SerializeField] private GameObject m_longRangeRadarArea;   //Gameobject that forms long range radar area.
    [SerializeField] private GameObject m_playerReference;      //Reference to GameObject that holds players information.

    public Transform m_notificationIcon;                        //Transform of notification icon.
    public GameObject[] m_notificationIcons;                    //Array of different notifications that can appear during game.

    //Gameobjects that form short range radar area.
    [SerializeField] private GameObject m_SRR1;
    [SerializeField] private GameObject m_SRR2;


    // Use this for initialization
    void Start()
    {
        m_SRR1.SetActive(false);
        m_SRR2.SetActive(false);
        m_longRangeRadarArea.SetActive(false);
        m_notificationIcons[0].SetActive(false);    //Treasure
        m_notificationIcons[1].SetActive(false);    //Health
        m_notificationIcons[2].SetActive(false);    //SRR
        m_notificationIcons[3].SetActive(false);    //LRR
        m_notificationIcons[4].SetActive(false);    //Map
        m_notificationIcons[5].SetActive(false);    //Ammo
        m_notificationIcons[6].SetActive(false);    //Meteor
        m_TreasureText.text = ("0/9");
        m_playerSpawned = false;
        m_battleStarted = false;
        m_playerNextTurn = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //if player has not been moved to start position run GameStart() method.
        if (m_playerSpawned == false)
        {
            GameStart();
        }
        
        //If player is on the lower half of game area move notification area above player.
        if (gameObject.transform.position.y < 5f)
        {
            NotificationIconPosition();
        }
        //If player is on the higer half of game area move notification area below player.
        else if (gameObject.transform.position.y >= 5f)
        {
            NotificationIconPosition();
        }

        //If long range radar is active.
        if (m_LLRinUse == true)
        {
            //If touch screen is touched shoot ray to screenpoint which was touched.
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                RaycastHit Hit;
                Ray touchRay = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);

                //If raycast hits Gameobject, move long range radar Gameobject to that place.
                if (Physics.Raycast(touchRay, out Hit))
                {
                    m_longRangeRadarArea.SetActive(true);
                    m_longRangeRadarArea.transform.position = Hit.collider.gameObject.transform.position;
                    StartCoroutine(LongRadarEnds());
                    m_LLRinUse = false;
                    return;
                }
            }
        }
    }

    //Raycast that checks what is in chosen direction.
    void ShootRay(Vector3 direction)
    {
        Vector3 DirectionRay = transform.TransformDirection(direction);
        Debug.DrawRay(transform.position, DirectionRay * m_rayRange, Color.green);
        RaycastHit Hit;
        if (Physics.Raycast(transform.position, DirectionRay, out Hit, m_rayRange))
        {

            if (Hit.collider.CompareTag("Respawn"))
            {
                Move(Hit.transform.position);

            }
            //Raycast hit empty tile. Remove fog from tile and move player on to that tile.
            if (Hit.collider.CompareTag("empty"))
            {
                Hit.collider.gameObject.GetComponentInParent<EmptyBlock>().Fow();
                Move(Hit.transform.position);
                m_playerNextTurn = false;

            }
            //Raycast hit enemy tile. Remove fog from tile and start combat.
            if (Hit.collider.CompareTag("enemy"))
            {
                m_nextPosition = Hit.transform.position;
                m_currentEnemy = Hit.collider.gameObject;
                Hit.collider.gameObject.GetComponent<EnemyBase>().Fow();
                m_playerNextTurn = false;
            }
            //Raycast hit treasure tile. Remove fog from tile and add one to found treasure pieces. Then move player on to that tile.
            if (Hit.collider.CompareTag("treasure"))
            {
                Hit.collider.gameObject.GetComponentInParent<Item>().Fow();
                Move(Hit.transform.position);
                m_playerNextTurn = false;
            }
            //Raycast hit item tile. Remove fog from tile and activate correct item. Then move player on to that tile.
            if (Hit.collider.CompareTag("item"))
            {
                Hit.collider.gameObject.GetComponentInParent<Item>().Fow();
                Move(Hit.transform.position);
                m_playerNextTurn = false;
            }
            //Raycast hit hazard tile. Remove fog from tile and activate hazard item. Then move player on to that tile.
            if (Hit.collider.CompareTag("hazard"))
            {
                Hit.collider.gameObject.GetComponentInParent<Item>().Fow();
                Move(Hit.transform.position);
                m_playerNextTurn = false;
            }
            //Raycast hit item tile. Activate resource then move player on to that tile.
            if (Hit.collider.CompareTag("resource"))
            {
                FirstPlanet();
                Move(Hit.transform.position);
            }
            //End turn.
            if (Hit.collider.CompareTag("wall"))
            {
                StartCoroutine(TurnEnds());
            }
            //End turn
            else
            {
                StartCoroutine(TurnEnds());
            }
        }
    }

    //Method that moves player in exploration phaze.
    void Move(Vector3 nextBlock)
    {
        //If player is out of combat.
        if (m_battleStarted == false)
        {
            //Move player model to next position.
            transform.position = nextBlock;
            //Add one to used turn counter.
            PlayerReference.m_Playerturns += 1;
            //End turn.
            StartCoroutine(TurnEnds());
        }
        //Else if player is in combat deny movement request.
        else if (m_battleStarted == true)
        {
            print("can't move you are locked in battle");
        }
    }

    //Method to move up using touch commands.
    public void Up()
    {
        //If its players next turn and player isn't in battle currently.
        if (m_playerNextTurn == true && m_battleStarted == false)
        {
            //Turn player model.
            m_starfighter.transform.rotation = Quaternion.Euler(180, 0, 270);
            //Shoot ray to see whats ahead.
            ShootRay(Vector3.up);
            //Deny additional movement until turn ends.
            m_playerNextTurn = false;
        }
        return;
    }

    //Method to move down using touch commands.
    public void Down()
    {
        //If its players next turn and player isn't in battle currently.
        if (m_playerNextTurn == true && m_battleStarted == false)
        {
            //Turn player model.
            m_starfighter.transform.rotation = Quaternion.Euler(180, 0, 90);
            //Shoot ray to see whats ahead.
            ShootRay(Vector3.down);
            //Deny additional movement until turn ends.
            m_playerNextTurn = false;
        }
        return;
    }

    //Method to move left using touch commands.
    public void Left()
    {
        //If its players next turn and player isn't in battle currently.
        if (m_playerNextTurn == true && m_battleStarted == false)
        {
            //Turn player model.
            m_starfighter.transform.rotation = Quaternion.Euler(180, 0, 180);
            //Shoot ray to see whats ahead.
            ShootRay(Vector3.left);
            //Deny additional movement until turn ends.
            m_playerNextTurn = false;
        }
        return;
    }

    //Method to move right using touch commands.
    public void Right()
    {
        //If its players next turn and player isn't in battle currently.
        if (m_playerNextTurn == true && m_battleStarted == false)
        {
            //Turn player model.
            m_starfighter.transform.rotation = Quaternion.Euler(180, 0, 0);
            //Shoot ray to see whats ahead.
            ShootRay(Vector3.right);
            //Deny additional movement until turn ends.
            m_playerNextTurn = false;
        }
        return;
    }

    //Method to move notification area depending on where player is on board so that is visible at all times.
    void NotificationIconPosition()
    {
        if (gameObject.transform.position.y < 5f)
        {
            m_notificationIcon.transform.localPosition = new Vector3(0f, 100f, -0.5f);
        }
        else if (gameObject.transform.position.y >= 5f)
        {
            m_notificationIcon.transform.localPosition = new Vector3(0f, -100f, -0.5f);
        }
    }

    //GameStart squence
    public void GameStart()
    {
        //Find start position tile.
        GameObject startposition = GameObject.FindGameObjectWithTag("Respawn");

        //If player position is not same as start position, move player to start position.
        if (gameObject.transform.position != startposition.transform.position)
        {
            print("GameStarting");
            Move(startposition.transform.position);
            return;
        }
        //Else player has spawned correctly.
        else if (gameObject.transform.position == startposition.transform.position)
        {
            print("PlayerSpawned");
            m_playerSpawned = true;
            return;
        }
    }


    public void EndBattle()
    {
        print("Exploration is on");
        m_currentEnemy.GetComponentInParent<EnemyBase>().EnemyDefeated();
    }

    public void MoveAfterBattle()
    {
        print("Next position: " + m_nextPosition);
        Move(m_nextPosition);
        m_battleStarted = false;
    }

    //Method that handels different items.
    public void Item(int ItemNum)
    {
        switch (ItemNum)
        {
            case 1:
                //clue
                PartOfTreasure();
                print("Treasure Piece");
                break;
            case 2:
                SpaceJunk(20, 10, 0, 1);
                //space junk repair kit
                print("Space junk repair kit");
                break;
            case 3:
                ShortrangeRadar();
                //short range radar
                print("Short range radar");
                break;
            case 4:
                LongrangeRadar();
                //long range radar
                print("Long range radar");
                break;
            case 5:
                ClosestTreasure();
                print("Treasure Map");
                break;
            case 6:
                SpaceJunk(0, 0, 1, 5);
                //ammocrate
                print("Space junk ammo crate");
                break;
            case 7:
                SpaceJunk(-10, -5, 0, 6);
                //meteorstorm
                print("Meteor storm");
                break;
            case 8:
                //item already taken
                print("item already taken");
                break;
            default:
                break;
        }
        return;
    }

    //Enemy tutorials.
    public void Enemy(int EnemyNum)
    {
        switch (EnemyNum)
        {
            case 1:
                if (m_enemyTutorial1 == false)
                {
                    m_UI.GetComponent<UI>().Tutorial(11);
                    m_enemyTutorial1 = true;
                }
                break;
            case 2:
                if (m_enemyTutorial2 == false)
                {
                    m_UI.GetComponent<UI>().Tutorial(12);
                    m_enemyTutorial2 = true;
                }
                break;
            case 3:
                if (m_enemyTutorial3 == false)
                {
                    m_UI.GetComponent<UI>().Tutorial(13);
                    m_enemyTutorial3 = true;
                }
                break;
            case 4:
                if (m_enemyTutorial4 == false)
                {
                    m_UI.GetComponent<UI>().Tutorial(14);
                    m_enemyTutorial4 = true;
                }
                break;
            case 5:
                if (m_enemyTutorial5 == false)
                {
                    m_UI.GetComponent<UI>().Tutorial(15);
                    m_enemyTutorial5 = true;
                }
                break;
            default:
                break;
        }
        return;
    }

    //Planets tutorial
    void FirstPlanet()
    {
        if (m_firstPlanet == false)
        {
            m_UI.GetComponent<UI>().Tutorial(10);
            m_firstPlanet = true;
        }
        else
        {
            return;
        }
    }

    //Adds tresure pieces and turns on victory screen when all pieces have been found.
    void PartOfTreasure()
    {
        StartCoroutine(QuickIconFlash(m_notificationIcons[0]));

        m_TreasurePartsFound += 1;
        m_TreasureText.text = (m_TreasurePartsFound + "/9");
        if (m_TreasurePartsFound == 1)
        {
            m_UI.GetComponent<UI>().Tutorial(1);
        }
        else if (m_TreasurePartsFound == 9)
        {
            m_UI.GetComponent<UI>().Victory();
        }
        else
        {
            return;
        }
    }

    //Method that finds closest treasure piece on gameboard and makes it blink for a moment.
    public GameObject ClosestTreasure()
    {
        StartCoroutine(QuickIconFlash(m_notificationIcons[4]));
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("clue");
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }
        if (m_firstSRR == false)
        {
            m_UI.GetComponent<UI>().Tutorial(5);
            m_firstMap = true;
        }
        closest.GetComponent<Item>().Blink();
        print(closest);
        return closest;
    }

    //Makes all nearby enemies and hazards blink for a moment.
    void ShortrangeRadar()
    {
        StartCoroutine(QuickIconFlash(m_notificationIcons[2]));
        m_SRR1.SetActive(true);
        m_SRR2.SetActive(true);
        StartCoroutine(ShortRadarEnds());
        if (m_firstSRR == false)
        {
            m_UI.GetComponent<UI>().Tutorial(3);
            m_firstSRR = true;
            return;
        }
        else
        {
            return;
        }
    }

    //Reveals chosen part of game area for short period of time.
    void LongrangeRadar()
    {
        m_LLRinUse = true;
        m_notificationIcons[3].SetActive(true);
        if (m_firstLRR == false)
        {
            m_UI.GetComponent<UI>().Tutorial(4);
            m_firstLRR = true;
            return;
        }
        else
        {
            return;
        }

    }

    //Method that adds or subtracts healt points, shield points and/or ammunition from player.
    void SpaceJunk(int health, int shields, int ammo, int icon)
    {

        StartCoroutine(QuickIconFlash(m_notificationIcons[icon]));
        m_playerReference.GetComponent<PlayerReference>().GetResources(health, shields, ammo);
        if (m_firstSJHP == false && icon == 1)
        {
            m_UI.GetComponent<UI>().Tutorial(2);
            m_firstSJHP = true;
        }
        else if (m_firstSJAC == false && icon == 5)
        {
            m_UI.GetComponent<UI>().Tutorial(6);
            m_firstSJAC = true;
        }
        else if (m_firstSJMS == false && icon == 6)
        {
            m_UI.GetComponent<UI>().Tutorial(7);
            m_firstSJMS = true;
        }
    }

    //End turn.
    IEnumerator TurnEnds()
    {
        yield return new WaitForSeconds(0.2f);
        m_playerNextTurn = true;

    }

    //Turns off long range radar.
    IEnumerator LongRadarEnds()
    {
        yield return new WaitForSeconds(2f);
        m_longRangeRadarArea.SetActive(false);
        m_playerNextTurn = true;
        m_notificationIcons[3].SetActive(false);

    }

    //Turns off short range radar.
    IEnumerator ShortRadarEnds()
    {
        yield return new WaitForSeconds(2f);
        m_SRR1.SetActive(false);
        m_SRR2.SetActive(false);
        m_playerNextTurn = true;

    }

    //Showing notification for a moment.
    IEnumerator QuickIconFlash(GameObject Icon)
    {
        Icon.SetActive(true);
        yield return new WaitForSeconds(1f);
        Icon.SetActive(false);
    }
}