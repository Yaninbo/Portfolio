public class MKDiscThrow : MonoBehaviour
{
    
	public GameObject m_disc;
    [SerializeField] private Transform m_trans;
    [SerializeField] private Transform m_backHandFP;
    [SerializeField] private Transform m_foreArmFP;
    [SerializeField] private Transform m_puttFP;
    [SerializeField] private Transform m_backHand;
    [SerializeField] private Transform m_foreArm;
    [SerializeField] private Transform m_putt;
	static public float m_power = 1f;
	public float m_maxPower= 100f;
	static public float m_completePower = 1f;
    static public float m_angle = 1f;
    public float m_maxAngle= 100f;
    static public float m_finalAngleR = 1f;
    static public float m_finalAngleP = 1f;
    static public float m_stylemulti = 1f;
    static public float m_stylespeedmulti = 1f;
    static public float m_styledurationmulti = 1f;
	private bool m_swingActive = false;
	private bool m_increasing = false;
	private bool m_throw = false;
    private int m_throwstyle;
    static public Vector3 m_startPosition;
    private bool m_powerslide = false;
    private bool m_AllowFire;
    public int DiscShot;
    public float m_turnDir;
    public float m_fadeDir;

    void Update()
    {
        m_disc = DiscSelect.m_activeDisc;
        m_AllowFire = DiscRestriction.m_canthrow;
        if (Input.GetButtonDown("Fire1"))
        {
            BackHand();
        }
        else if (Input.GetButtonDown("Fire2"))
        {
            ForeArm();
        }
        else if (Input.GetButtonDown("Fire3"))
        {
            Putt();
        }


        if (Input.GetButtonDown("Fire3")^Input.GetButtonDown("Fire2")^Input.GetButtonDown("Fire1") && !m_powerslide)
        {
            if (m_AllowFire == true)
            {
                if (m_swingActive == false)
                {
                    m_swingActive = true;
                    m_increasing = true; 
                }
                else if (m_swingActive == true)
                {
                    if (m_increasing == true)
                    {
                        m_power = m_maxPower;
                        m_increasing = false;
                    }
                    else if (m_swingActive == false)
                    {
                        m_completePower = m_power;
                        m_swingActive = false;
                            
                    }
                }
            }
        }
        if (Input.GetButtonUp("Fire3")&& !m_powerslide||Input.GetButtonUp("Fire2")&& !m_powerslide||Input.GetButtonUp("Fire1")&& !m_powerslide)
        {
            if (m_swingActive)
            {
                m_swingActive = false;
                m_completePower = ((m_power /100f)*35f);
                // DiscRestriction.m_canthrow = false;
                m_powerslide = true;                   
            }

        }
        if (m_powerslide && Input.GetButtonUp("Fire3")^Input.GetButtonUp("Fire2")^Input.GetButtonUp("Fire1") )
        {
            if (m_AllowFire == true)
            {
                if (m_swingActive == false)
                {
                    m_swingActive = true;
                    m_increasing = true; 
                }
                else if (m_swingActive == true)
                {
                    if (m_increasing == true)
                    {
                        m_angle = m_maxAngle;
                        m_increasing = false;
                    }
                    else if (m_swingActive == false)
                    {
                        m_finalAngleR = m_angle;
                        m_swingActive = false;

                    }
                }
            }
        }
        if (Input.GetButtonDown("Fire3")&& m_powerslide||Input.GetButtonDown("Fire2")&& m_powerslide||Input.GetButtonDown("Fire1")&& m_powerslide)
        {
            if (m_swingActive)
            {
                m_swingActive = false;
                m_finalAngleR = ((m_angle /100f)*90f);
                m_finalAngleP = AngleofAttackslider.m_throwAngle;
                DiscRestriction.m_canthrow = false;
                Fire();
                m_powerslide = false;
                m_power = 1f;
                m_angle = 1f;

            }

        }
    }

	void FixedUpdate()
    {
            
        if (m_swingActive == false)
        {
            return; 
        }
        else if (m_swingActive == true)
        {                        
            if (!m_powerslide)
            {
                if (m_increasing == true && m_power <= m_maxPower)
                {
                    ++m_power;
                    //   print("m_power rising");
                }

                else
                {
                    m_increasing = false;
                }

                if (m_increasing == false && m_power >= 1f)
                {
                    --m_power;
                //    print("m_power going down");
                }

                else
                {
                    m_increasing = true;
                }

            }

            if (m_powerslide)
            {
                if (m_increasing == true && m_angle <= m_maxAngle)
                {
                    ++m_angle;
                //     print("m_angle rising");
                }

                else
                {
                    m_increasing = false;
                }

                if (m_increasing == false && m_angle >= 1f)
                {
                    --m_angle;
                    //   print("m_angle going down");
                }

                else
                {
                    m_increasing = true;
                }
            }
        }
    }  

    private void Fire()
	{
		m_AllowFire = false;
		GameObject BulletInstance = Instantiate(m_disc, transform.position, transform.rotation) as GameObject;
		m_AllowFire = true;
	}

    public void BackHand()
    {        
        m_startPosition = m_backHand.localPosition;
        m_stylemulti = 1f;
        m_stylespeedmulti = 1f;
        m_styledurationmulti = 1f;
    }

    public void ForeArm()
    {                   
        m_startPosition = m_foreArm.localPosition;
        m_stylemulti = -1f;
        m_stylespeedmulti = 1f;
        m_styledurationmulti = 1f;
    }

    public void Putt()
    {                      
        m_startPosition = m_putt.localPosition;
        m_stylemulti = 0.5f;
        m_stylespeedmulti = 0.3f;
        m_styledurationmulti = 0.5f;
    }
}