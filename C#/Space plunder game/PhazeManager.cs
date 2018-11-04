public class PhazeManager : MonoBehaviour
{
    [SerializeField]private CinemachineVirtualCamera m_explorationVirtualCamera;    //Cinemachine virtual camera set for exploration phaze.
    [SerializeField]private CinemachineVirtualCamera m_battleVirtualCamera;         //Cinemachine virtual camera set for battle phaze.
    static public bool m_playerBattleTurn;                                          //Bool that checks if its players turn in battle.
    
    // Use this for initialization
    void Start()
    {
        //At the start of game exploration camera has priority.
        m_explorationVirtualCamera.Priority = 2;
        m_battleVirtualCamera.Priority = 1;
        //Set cameras ortographic size according to what has been set in options and saved in PlayerPrefs.
        m_explorationVirtualCamera.m_Lens.OrthographicSize = PlayerPrefs.GetFloat("Camera");
        m_battleVirtualCamera.m_Lens.OrthographicSize = PlayerPrefs.GetFloat("Camera");
    }
    
    //Set exploration camera to higher priority.
    public void TransitionToExploration()
    {
        m_explorationVirtualCamera.Priority = 2;
        m_battleVirtualCamera.Priority = 1;
    }
    //Set battle camera to higher priority.
    public void TransitionToBattle()
    {
        m_battleVirtualCamera.Priority = 2;
        m_explorationVirtualCamera.Priority = 1;
    }
}
