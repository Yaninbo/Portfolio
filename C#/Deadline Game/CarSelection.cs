public class CarSelection1 : MonoBehaviour
{
    //Commenting to this script is coming soon.
    public int m_playerNum;
    [SerializeField] private GameObject m_spotlight;
    [SerializeField] private GameObject[] m_carList;
    [SerializeField] private GameObject[] m_iconList;
    public int m_carIndex;
    public int m_iconIndex;
    private float m_up;
    private float m_left;
    private bool m_toggleCar = false;
    private bool m_toggleIcon = false;
    private bool m_confirm = false;
    private bool m_letConfirm = false;
    public bool m_cangoBack= true;
    private bool m_canExitToMenu = true;
    [SerializeField] private string m_CN;

    private void Start()
    {
        m_letConfirm = false;

        m_spotlight.SetActive(false);

    
        foreach (GameObject go in m_carList)
        {
            go.SetActive(false);
        }
        foreach (GameObject go in m_iconList)
        {
            go.SetActive(false);
        }
    
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
        m_CN = PlayerSlot.m_playerslots[m_playerNum-1];

        print("Player"+ m_playerNum +" controller: "+m_CN);



        if (m_CN == "Player1"||m_CN == "Player2"||m_CN == "Player3"||m_CN == "Player4")
        {
            m_up = Input.GetAxisRaw(m_CN+"Vertical");
            m_left = Input.GetAxisRaw(m_CN+"Horizontal");
             
            if (m_up > 0.2f && m_toggleIcon == false && m_confirm == false)
            {
                StartCoroutine(Up());
                m_toggleIcon = true;
            }

            else if (m_up < -0.2f && m_toggleIcon == false && m_confirm == false)
            {
                StartCoroutine(Down());
                m_toggleIcon = true;
            }

            if (m_left > 0.2f && m_toggleCar == false && m_confirm == false)
            {
                StartCoroutine(Left());
                m_toggleCar = true;
            }

            else if (m_left < -0.2f && m_toggleCar == false && m_confirm == false)
            {
                StartCoroutine(Right());
                m_toggleCar = true;
            }


            if (m_letConfirm == false && Input.GetButtonDown(m_CN+"Drift")) //To prevent player instantly confirming car choise.
            {  
                StartCoroutine(WaitToConfirm());
            }

            else if (m_letConfirm == true && Input.GetButtonDown(m_CN+"Drift") && m_confirm == false) //player confirming car choise.
            {                
                Confirm();
                GameObject.Find("Playerslot").GetComponent <PlayerSlot>().RemoveRotSpeed(1);
                m_confirm = true;
            }



            if (Input.GetButtonDown(m_CN+"Boost")&& m_confirm == true) //Player un-confirming car choise.
            {                
                Release();
                GameObject.Find("Playerslot").GetComponent <PlayerSlot>().AddRotSpeed(1);

            }

            else if (Input.GetButtonDown(m_CN+"Boost")&& m_confirm == false)
            {
                    
                GameObject.Find("Playerslot").GetComponent <PlayerSlot>().RemoveplayerFromSlot(m_playerNum - 1);                  
            }
                    
        }

        else if(m_CN == "Player5"||m_CN == "Player6")
        {
            m_up = Input.GetAxisRaw(m_CN+"Vertical");
            m_left = Input.GetAxisRaw(m_CN+"Horizontal");


            if (m_up > 0.2f && m_toggleIcon == false && m_confirm == false)
            {
                StartCoroutine(Up());
                m_toggleIcon = true;
            }

            else if (m_up < -0.2f && m_toggleIcon == false && m_confirm == false)
            {
                StartCoroutine(Down());
                m_toggleIcon = true;
            }

            if (m_left > 0.2f && m_toggleCar == false && m_confirm == false)
            {
                StartCoroutine(Left());
                m_toggleCar = true;
            }

            else if (m_left < -0.2f && m_toggleCar == false && m_confirm == false)
            {
                StartCoroutine(Right());
                m_toggleCar = true;
            }

            if (m_letConfirm == false && Input.GetButtonDown(m_CN+"Drift"))
            {                
                StartCoroutine(WaitToConfirm());
            }

            else if (m_letConfirm == true && Input.GetButtonDown(m_CN+"Drift") && m_confirm == false)
            {                
                Confirm();
                GameObject.Find("Playerslot").GetComponent <PlayerSlot>().RemoveRotSpeed(1);
                m_confirm = true;
            }

            if (Input.GetButtonDown(m_CN+"Deselect")&& m_confirm == true)
            {                
                Release();
                GameObject.Find("Playerslot").GetComponent <PlayerSlot>().AddRotSpeed(1);
                m_letConfirm = false;
                m_confirm = false;
            }

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

    IEnumerator WaitToConfirm()
    {
        yield return new WaitForSeconds(0.1f);
        m_letConfirm = true;
    }
}

