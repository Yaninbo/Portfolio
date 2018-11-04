public class DLHighScoreManager : MonoBehaviour
{
    // High score manager.
    // Local highScore manager for LeaderboardLength number of entries
    // eg: HighScoreManager._instance.SaveHighScore("meh",1232);
    // No need to attach this to any game object, thought it would create errors attaching.

    private static DLHighScoreManager m_highScoreInstance;
    private const int m_LeaderboardLength = 10;
    public DLMenuControllerFC m_MenuContFC;
    public DLMenuControllerNMC m_MenuContNMC;
    private int m_selectedStage = 0;

    public static DLHighScoreManager _instance
    {
        get
        {
            if (m_highScoreInstance == null)
            {
                m_highScoreInstance = new GameObject("HighScoreManager").AddComponent<DLHighScoreManager>();                
            }
            return m_highScoreInstance;
        }
    }

    public void SaveHighScoreFC(string inputControllerName, float inputScore)
    {
        m_MenuContFC = GetComponent<DLMenuControllerFC>();
        List<EndScoreFC> HighScores = new List<EndScoreFC>();
    
        int i = 1;
        string minutes = ((int)inputScore / 60).ToString();
        string seconds = (inputScore % 60).ToString(":00.00");
        string tempScore = minutes + seconds;
        while (i <= m_LeaderboardLength && PlayerPrefs.HasKey("HighScore" + i + "m_scoreFC"))
        {
            EndScoreFC temp = new EndScoreFC();
            temp.m_scoreFC = PlayerPrefs.GetString("HighScore" + i + "m_scoreFC");
            temp.m_nameFC = PlayerPrefs.GetString("HighScore" + i + "m_nameFC");
            temp.scoreValue = PlayerPrefs.GetFloat("HighScore" + i + "scoreValue");
            HighScores.Add(temp);
            i++;
        }
        if (HighScores.Count == 0)
        {
            EndScoreFC _temp = new EndScoreFC();
            _temp.m_nameFC = inputControllerName;
            _temp.m_scoreFC = tempScore;
            _temp.scoreValue = inputScore;
            m_MenuContFC.AskName(inputControllerName);
            HighScores.Add(_temp);
        }
        else
        {
            for (i = 1; i <= HighScores.Count && i <= m_LeaderboardLength; i++)
            {
                if (inputScore < HighScores[i - 1].scoreValue)
                {
                    EndScoreFC _temp = new EndScoreFC();
                    _temp.m_nameFC = inputControllerName;
                    _temp.m_scoreFC = tempScore;
                    _temp.scoreValue = inputScore;
                    m_MenuContFC.AskName(inputControllerName);
                    HighScores.Insert(i - 1, _temp);                   
                    break;
                }            
                if (i == HighScores.Count && i < m_LeaderboardLength)
                {
                    EndScoreFC _temp = new EndScoreFC();
                    _temp.m_nameFC = inputControllerName;
                    _temp.m_scoreFC = tempScore;
                    _temp.scoreValue = inputScore;
                    m_MenuContFC.AskName(inputControllerName);
                    HighScores.Add(_temp);
                    break;
                }
            }
        }
    
        i = 1;
        while (i <= m_LeaderboardLength && i <= HighScores.Count)
        {
            PlayerPrefs.SetString("HighScore" + i + "m_nameFC", HighScores[i - 1].m_nameFC);
            PlayerPrefs.SetString("HighScore" + i + "m_scoreFC", HighScores[i - 1].m_scoreFC);
            PlayerPrefs.SetFloat("HighScore" + i + "scoreValue", HighScores[i - 1].scoreValue);
            i++;
        }
    
    }

    public List<EndScoreFC> GetHighScoreFC()
    {
        List<EndScoreFC> HighScores = new List<EndScoreFC>();
    
        int i = 1;
        while (i <= m_LeaderboardLength && PlayerPrefs.HasKey("HighScore" + i + "m_scoreFC"))
        {
            EndScoreFC temp = new EndScoreFC();
            temp.m_scoreFC = PlayerPrefs.GetString("HighScore" + i + "m_scoreFC");
            temp.m_nameFC = PlayerPrefs.GetString("HighScore" + i + "m_nameFC");
            HighScores.Add(temp);
            i++;
        }
    
        return HighScores;
    }

    public void ClearLeaderBoardFC()
    {
        List<EndScoreFC> HighScores = GetHighScoreFC();
    
        for (int i = 1; i <= HighScores.Count; i++)
        {
            PlayerPrefs.DeleteKey("HighScore" + i + "m_nameFC");
            PlayerPrefs.DeleteKey("HighScore" + i + "m_scoreFC");
            PlayerPrefs.DeleteKey("HighScore" + i + "scoreValue");
        }
    }

    public void RenameEndScoreFC(string inputController, string inputName)
    {
        int i = 1;
        while (i <= m_LeaderboardLength)
        {
            string tempName = PlayerPrefs.GetString("HighScore" + i + "m_nameFC");
            if (tempName == inputController)
            {
                PlayerPrefs.SetString("HighScore" + i + "m_nameFC", inputName);
                print("renamed endscore FC: "+inputName);
            }
            i++;
        }
    }

//NMC highscore area

    public void SaveHighScoreNMC(string inputControllerName, float inputScore)
    {
        m_MenuContNMC = GetComponent<DLMenuControllerNMC>();
        List<EndScoreNMC> HighScores = new List<EndScoreNMC>();

        int i = 1;
        string minutes = ((int)inputScore / 60).ToString();
        string seconds = (inputScore % 60).ToString(":00.00");
        string tempScore = minutes + seconds;
        while (i <= m_LeaderboardLength && PlayerPrefs.HasKey("HighScore" + i + "m_scoreNMC"))
        {
            EndScoreNMC temp = new EndScoreNMC();
            temp.m_scoreNMC = PlayerPrefs.GetString("HighScore" + i + "m_scoreNMC");
            temp.m_nameNMC = PlayerPrefs.GetString("HighScore" + i + "m_nameNMC");
            temp.scoreValue = PlayerPrefs.GetFloat("HighScore" + i + "scoreValue");
            HighScores.Add(temp);
            i++;
        }
        if (HighScores.Count == 0)
        {
            EndScoreNMC _temp = new EndScoreNMC();
            _temp.m_nameNMC = inputControllerName;
            _temp.m_scoreNMC = tempScore;
            _temp.scoreValue = inputScore;
            m_MenuContNMC.AskName(inputControllerName);
            HighScores.Add(_temp);
        }
        else
        {
            for (i = 1; i <= HighScores.Count && i <= m_LeaderboardLength; i++)
            {
                if (inputScore < HighScores[i - 1].scoreValue)
                {
                    EndScoreNMC _temp = new EndScoreNMC();
                    _temp.m_nameNMC = inputControllerName;
                    _temp.m_scoreNMC = tempScore;
                    _temp.scoreValue = inputScore;
                    m_MenuContNMC.AskName(inputControllerName);
                    HighScores.Insert(i - 1, _temp);                   
                    break;
                }            
                if (i == HighScores.Count && i < m_LeaderboardLength)
                {
                    EndScoreNMC _temp = new EndScoreNMC();
                    _temp.m_nameNMC = inputControllerName;
                    _temp.m_scoreNMC = tempScore;
                    _temp.scoreValue = inputScore;
                    m_MenuContNMC.AskName(inputControllerName);
                    HighScores.Add(_temp);
                    break;
                }
            }
        }

        i = 1;
        while (i <= m_LeaderboardLength && i <= HighScores.Count)
        {
            PlayerPrefs.SetString("HighScore" + i + "m_nameNMC", HighScores[i - 1].m_nameNMC);
            PlayerPrefs.SetString("HighScore" + i + "m_scoreNMC", HighScores[i - 1].m_scoreNMC);
            PlayerPrefs.SetFloat("HighScore" + i + "scoreValue", HighScores[i - 1].scoreValue);
            i++;
        }

    }

    public List<EndScoreNMC> GetHighScoreNMC()
    {
        List<EndScoreNMC> HighScores = new List<EndScoreNMC>();

        int i = 1;
        while (i <= m_LeaderboardLength && PlayerPrefs.HasKey("HighScore" + i + "m_scoreNMC"))
        {
            EndScoreNMC temp = new EndScoreNMC();
            temp.m_scoreNMC = PlayerPrefs.GetString("HighScore" + i + "m_scoreNMC");
            temp.m_nameNMC = PlayerPrefs.GetString("HighScore" + i + "m_nameNMC");
            HighScores.Add(temp);
            i++;
        }

        return HighScores;
    }

    public void ClearLeaderBoardNMC()
    {
        //for(int i=0;i<HighScores.
        List<EndScoreNMC> HighScores = GetHighScoreNMC();

        for (int i = 1; i <= HighScores.Count; i++)
        {
            PlayerPrefs.DeleteKey("HighScore" + i + "m_nameNMC");
            PlayerPrefs.DeleteKey("HighScore" + i + "m_scoreNMC");
            PlayerPrefs.DeleteKey("HighScore" + i + "scoreValue");
        }
    }

    public void RenameEndScoreNMC(string inputController, string inputName)
    {
        int i = 1;
        while (i <= m_LeaderboardLength)
        {
            string tempName = PlayerPrefs.GetString("HighScore" + i + "m_nameNMC");
            if (tempName == inputController)
            {
                PlayerPrefs.SetString("HighScore" + i + "m_nameNMC", inputName);
                print("renamed endscore NMC: "+inputName);
            }
            i++;
        }
    }
    void OnApplicationQuit()
    {
        PlayerPrefs.Save();
    }
}

public class EndScoreFC
{
    public string m_scoreFC;
    public float scoreValue;
    public string m_nameFC;
}

public class EndScoreNMC
{
    public string m_scoreNMC;
    public float scoreValue;
    public string m_nameNMC;
}