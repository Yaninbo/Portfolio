public class SceneMovementManager : MonoBehaviour
{
    public CinemachineVirtualCamera m_levelCam;
    public CinemachineVirtualCamera m_carCam;
    public CinemachineVirtualCamera m_midCam;
    public CinemachineVirtualCamera m_midderCam;

    public GameObject m_computer;
    public GameObject m_joinCanvas;
    public GameObject m_startCanvas;
    public GameObject m_playerSlots;

    [SerializeField]private int m_sceneNum = 0;

    // Use this for initialization
    void Start()
    {
        FlushCars();
        m_computer.SetActive(true);

        m_playerSlots.SetActive(false);
        NextSelection(1);

    }

    void Update()
    {

        switch (m_sceneNum)
        {
            case 1:
                m_levelCam.Priority = 1;
                m_carCam.Priority = 1;
                m_midCam.Priority = 10;
                m_midderCam.Priority = 1;
                m_computer.SetActive(true);
                m_startCanvas.SetActive(false);
                m_playerSlots.SetActive(false);
                StartCoroutine (GotoLevel());
                break;
            case 2:
                m_levelCam.Priority = 10;
                m_carCam.Priority = 1;
                m_midCam.Priority = 1;
                m_midderCam.Priority = 1;
                m_computer.SetActive(true);
                m_startCanvas.SetActive(false);
                m_playerSlots.SetActive(false);
                break;
            case 3:
                m_levelCam.Priority = 1;
                m_carCam.Priority = 1;
                m_midCam.Priority = 10;
                m_midderCam.Priority = 1;
                m_computer.SetActive(true);
                m_startCanvas.SetActive(false);
                m_playerSlots.SetActive(true);
                StartCoroutine (GotoCar());
                break;
            case 4:
                m_levelCam.Priority = 1;
                m_carCam.Priority = 10;
                m_midCam.Priority = 1;
                m_midderCam.Priority = 1;
                m_computer.SetActive(false);
                m_startCanvas.SetActive(true);
                m_playerSlots.SetActive(true);
                break;
            case 5:
                StartCoroutine (CartoLevel());
                m_levelCam.Priority = 1;
                m_carCam.Priority = 1;
                m_midCam.Priority = 10;
                m_midderCam.Priority = 1;
                m_computer.SetActive(true);
                m_startCanvas.SetActive(false);
                m_playerSlots.SetActive(true);

                break;
            case 6:
                m_levelCam.Priority = 1;
                m_carCam.Priority = 1;
                m_midCam.Priority = 1;
                m_midderCam.Priority = 10;
                m_computer.SetActive(true);
                m_startCanvas.SetActive(false);
                m_playerSlots.SetActive(true);
                break;
            default:
                print("sceneNum Error");
                break;
        }
    }

    public void NextSelection(int caseNumber)
    {
        m_sceneNum = caseNumber;
    }
    public void FlushCars()
    {
        for (int Yee = 0; Yee < 4; Yee++)
        {
            GameObject.Find("Playerslot").GetComponent <PlayerSlot>().FlushSlot(Yee);
        }
        CarSelection1[] scriptlistofcars = GameObject.Find("Playerslot").GetComponentsInChildren<CarSelection1>();

        foreach (CarSelection1 Carscript in scriptlistofcars)
        {
            print("flushes");
            Carscript.Release();

        }
        PlayerChoises.m_playerAmount = 0;
        PlayerChoises.m_playerJoinedAmount = 0; 
    }

    IEnumerator GotoLevel()
    {
        yield return new WaitForSeconds(0.5f);
        NextSelection(2);
    }

    IEnumerator GotoCar()
    {
            
        yield return new WaitForSeconds(2f);
        NextSelection(6);
        yield return new WaitForSeconds(2f);
        NextSelection(4);
        yield return new WaitForSeconds(2f);
        m_joinCanvas.SetActive(true);
    }

    IEnumerator CartoLevel()
    {
        m_joinCanvas.SetActive(false);
        PlayerChoises.m_playerAmount = 0;
        PlayerChoises.m_playerJoinedAmount = 0; 
        yield return new WaitForSeconds(2f);
        NextSelection(2);
    }
}
