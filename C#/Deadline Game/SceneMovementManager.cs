public class SceneMovementManager : MonoBehaviour
{
    //This script handels transition between level selection and car selection.
    public CinemachineVirtualCamera m_levelCam;     //Cinemachine virtual camera point that is used while player is in level selection.
    public CinemachineVirtualCamera m_carCam;       //Cinemachine virtual camera point that is used while players are selecting vehicles.
    public CinemachineVirtualCamera m_startCam;     //Cinemachine virtual camera point that is used while camera is moving to first selection.
    public CinemachineVirtualCamera m_midCam;       //Cinemachine virtual camera point that is used while camera is moving between selections.

    public GameObject m_computer;                   //LevelSelection gameobject.
    public GameObject m_joinCanvas;                 //Canvas element which indicates that players are alowed to join game.
    public GameObject m_startCanvas;                //Canvas element which indicates that all players are ready and game can be started.
    public GameObject m_playerSlots;                //Car selection platforms gameobject.

    [SerializeField]private int m_sceneNum = 0;     //Integer representing what part of selection we are currently in.

    // Use this for initialization
    void Start()
    {
        //Empty player selections if there were any.
        FlushCars();
        //Set level selection gameobject active.
        m_computer.SetActive(true);
        //Set car selection gameobject inactive.
        m_playerSlots.SetActive(false);
        //Go in to update switch case option 1.
        NextSelection(1);

    }

    void Update()
    {

        switch (m_sceneNum)
        {
            //Camera starts from start position and sets level selection gameobject active.
            //Then starts Gotolevel() Coroutine.
            case 1:
                m_levelCam.Priority = 1;
                m_carCam.Priority = 1;
                m_startCam.Priority = 10;
                m_midCam.Priority = 1;
                m_computer.SetActive(true);
                m_startCanvas.SetActive(false);
                m_playerSlots.SetActive(false);
                StartCoroutine (GotoLevel());
                break;

            //Camera moves to level selection camera position and sets level selection gameobject active in case it wasn't.
            case 2:
                m_levelCam.Priority = 10;
                m_carCam.Priority = 1;
                m_startCam.Priority = 1;
                m_midCam.Priority = 1;
                m_computer.SetActive(true);
                m_startCanvas.SetActive(false);
                m_playerSlots.SetActive(false);
                break;

            //Camera moves towards start camera position and sets level selection gameobject and player slots gameobject active in case they weren't.
            //Then starts GotoCar() Coroutine.
            case 3:
                m_levelCam.Priority = 1;
                m_carCam.Priority = 1;
                m_startCam.Priority = 10;
                m_midCam.Priority = 1;
                m_computer.SetActive(true);
                m_startCanvas.SetActive(false);
                m_playerSlots.SetActive(true);
                StartCoroutine (GotoCar());
                break;

            //Camera moves to vehicle selection camera position and sets player slots gameobject active in case it wasn't.
            case 4:
                m_levelCam.Priority = 1;
                m_carCam.Priority = 10;
                m_startCam.Priority = 1;
                m_midCam.Priority = 1;
                m_computer.SetActive(false);
                m_startCanvas.SetActive(true);
                m_playerSlots.SetActive(true);
                break;

            //Starts CartoLevel() Coroutine.
            //Camera then moves towards start camera position and sets level selection gameobject and player slots gameobject active in case they weren't.
            case 5:
                StartCoroutine (CartoLevel());
                m_levelCam.Priority = 1;
                m_carCam.Priority = 1;
                m_startCam.Priority = 10;
                m_midCam.Priority = 1;
                m_computer.SetActive(true);
                m_startCanvas.SetActive(false);
                m_playerSlots.SetActive(true);
                break;

            //Camera moves towards mid camera position and sets level selection gameobject and player slots gameobject active in case they weren't.
            case 6:
                m_levelCam.Priority = 1;
                m_carCam.Priority = 1;
                m_startCam.Priority = 1;
                m_midCam.Priority = 10;
                m_computer.SetActive(true);
                m_startCanvas.SetActive(false);
                m_playerSlots.SetActive(true);
                break;

            //Error case happend. Print error message to console.
            default:
                print("sceneNum Error");
                break;
        }
    }

    //Method to initiate selection change from outside of this script.
    public void NextSelection(int caseNumber)
    {
        m_sceneNum = caseNumber;
    }

    //Method to empty old player information when game is outside of vehicle selection.
    public void FlushCars()
    {
        for (int Yee = 0; Yee < 4; Yee++)
        {
            GameObject.Find("Playerslot").GetComponent <PlayerSlot>().FlushSlot(Yee);
        }
        CarSelection[] scriptlistofcars = GameObject.Find("Playerslot").GetComponentsInChildren<CarSelection>();

        foreach (CarSelection Carscript in scriptlistofcars)
        {
            print("flushes");
            Carscript.Release();

        }
        PlayerChoises.m_playerAmount = 0;
        PlayerChoises.m_playerJoinedAmount = 0; 
    }

    //Method to move to level selection area when entering scene fo the first time.
    IEnumerator GotoLevel()
    {
        yield return new WaitForSeconds(0.5f);
        NextSelection(2);
    }

    //Method to move to vehicle selection area when race track has been chosen.
    IEnumerator GotoCar()
    {
            
        yield return new WaitForSeconds(2f);
        NextSelection(6);
        yield return new WaitForSeconds(2f);
        NextSelection(4);
        yield return new WaitForSeconds(2f);
        m_joinCanvas.SetActive(true);
    }

    //Method to move to level selection area when returning from vehicle selection.
    IEnumerator CartoLevel()
    {
        m_joinCanvas.SetActive(false);
        PlayerChoises.m_playerAmount = 0;
        PlayerChoises.m_playerJoinedAmount = 0; 
        yield return new WaitForSeconds(2f);
        NextSelection(2);
    }
}
