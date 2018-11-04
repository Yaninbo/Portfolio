public class DLHighscoreLeaderboard : MonoBehaviour
{

    public string m_name = "";
    public string m_score = "";
    public string m_playernum;
    public Text[] m_playerNamesFC;
    public Text[] m_PlayerTimesFC;
    public Text[] m_playerNamesNMC;
    public Text[] m_PlayerTimesNMC;
    //    public GameObject m_scoreboard;
    List<EndScoreFC> highscoreFC;
    List<EndScoreNMC> highscoreNMC;
    bool m_updated = false;

    // Use this for initialization
    void Start()
    {
        highscoreFC = new List<EndScoreFC>();
        highscoreNMC = new List<EndScoreNMC>();
    }   

    // Update is called once per frame
    void Update()
    {
        UpdateHighScore();

        highscoreFC = DLHighScoreManager._instance.GetHighScoreFC();
        highscoreNMC = DLHighScoreManager._instance.GetHighScoreNMC();
    }

    void UpdateHighScore()
    {
        for (int i=1; i<11; i++)
        {
            m_playerNamesFC[i-1].text =PlayerPrefs.GetString("HighScore"+ i +"m_nameFC");
            m_PlayerTimesFC[i-1].text =PlayerPrefs.GetString("HighScore"+ i +"m_scoreFC");
            m_playerNamesNMC[i-1].text =PlayerPrefs.GetString("HighScore"+ i +"m_nameNMC");
            m_PlayerTimesNMC[i-1].text =PlayerPrefs.GetString("HighScore"+ i +"m_scoreNMC");
        }
    }
}
