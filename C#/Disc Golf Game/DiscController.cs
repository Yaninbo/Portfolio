public class DiscController : MonoBehaviour
{
    //Script that calculates Besier curve points according to throw parameters.
    //DiscController takes throw values of throw speed, angle of roll and angle of pitch
    //as well as gets discs own attributes and palces them into algorithm that calculates flight path.

    private GameObject activeDisc;                          //Currently active disc of which attributes are being used.
    private DiscBase activeDiscScript;                      //Reference to DiscBase Script to get disc attributes.

    public float m_throwspeed;                              //Float value of disc throws initial velocity.
    public float m_angleofattack;                           //Float value of disc throws initial pitch angle.
    public float m_angleofroll;                             //Float value of disc throws initial roll angle

    [SerializeField] private float m_speedmultiS;           //Speed multiplier caused by inital speed value. Effects throw lenght.
    [SerializeField] private float m_heightmultiS;          //Height multiplier caused by inital speed value. Effects throw height.
    [SerializeField] private float m_turnmultiS;            //Turn multiplier caused by inital speed value. Effects throw start turn.
    [SerializeField] private float m_fademultiS;            //Fade multiplier caused by inital speed value. Effects throw end fade.

    [SerializeField] private float m_speedmultiP;           //Speed multiplier caused by inital pitch angle value. Effects throw lenght.
    [SerializeField] private float m_heightmultiP;          //Height multiplier caused by inital pitch angle value. Effects throw height.
    [SerializeField] private float m_turnmultiP;            //Turn multiplier caused by inital pitch angle value. Effects throw start turn.
    [SerializeField] private float m_fademultiP;            //Fade multiplier caused by inital pitch angle value. Effects throw end fade.

    [SerializeField] private float m_speedmultiR;           //Speed multiplier caused by inital roll angle value. Effects throw lenght.
    [SerializeField] private float m_heightmultiR;          //Speed multiplier caused by inital roll angle value. Effects throw lenght.
    [SerializeField] private float m_turnmultiR;            //Speed multiplier caused by inital roll angle value. Effects throw lenght.
    [SerializeField] private float m_fademultiR;            //Speed multiplier caused by inital roll angle value. Effects throw lenght.

    public int m_throwStyle = 1;                            //Integer representing the throwing style.
    public float m_durationmulti = 2;                       //Duration multiplier adjusting fow fast disc is moveing along the Bezier curve.       

    [HideInInspector]public float z1 = 0f;                  //Bezier curves 2nd point Z-axis value.
    [HideInInspector]public float z2 = 0f;                  //Bezier curves 3rd point Z-axis value.
    [HideInInspector]public float z3 = 0f;                  //Bezier curves 4th point Z-axis value.
    [HideInInspector]public float y1 = 0f;                  //Bezier curves 2nd point Y-axis value.
    [HideInInspector]public float y2 = 0f;                  //Bezier curves 3rd point Y-axis value.
    [HideInInspector]public float y3 = 0f;                  //Bezier curves 4th point Y-axis value.
    [HideInInspector]public float x1 = 0f;                  //Bezier curves 2nd point X-axis value.
    [HideInInspector]public float x2 = 0f;                  //Bezier curves 3rd point X-axis value.
    [HideInInspector]public float x3 = 0f;                  //Bezier curves 4th point X-axis value.
    public Vector3 m_point0;                                //Bezier curves starting point. Always starts from player.
    public Vector3 m_point1;                                //Bezier curves 2nd point.
    public Vector3 m_point2;                                //Bezier curves 3rd point.
    public Vector3 m_point3;                                //Bezier curves end point.
    static public Vector3[] pointsToGive = new Vector3[4];  //Static Vector 3 array to pass to Bezier script.

    private float m_getSpeed = 1f;                          //Current discs speed attribute value.
    private float m_getGlide = 1f;                          //Current discs glide attribute value.
    private float m_getTurn = 1f;                           //Current discs turn attribute value.
    private float m_getFade = 1f;                           //Current discs fade attribute value.


    void Update()
    {
        //Get current active disc reference.
        activeDisc = DiscSelect.m_activeDisc;

        //Get throw speed and angles from DiscThrow script.
        m_throwspeed = DiscThrow.m_completePower;
        m_angleofroll = DiscThrow.m_angle;
        m_angleofattack = DiscThrow.m_finalAngleP;

        //Get Disc attribute values.
        GetValues();

        //Throw Speed Area of Code
        //Get correct set of multipliers according to throw speed value.
        //Can be optimized to not use if else statements, but havent had time to design that.
        if (m_throwspeed > 30f)
        {
            m_speedmultiS = 2f;
            m_heightmultiS = 2f;
            m_turnmultiS = 6f;
            m_fademultiS = 4.5f;

            m_durationmulti = 7f;

        }
        else if (m_throwspeed < 30f && m_throwspeed > 25f)
        {
            m_speedmultiS = 1.75f;
            m_heightmultiS = 1.8f;
            m_turnmultiS = 5f;
            m_fademultiS = 5f;

            m_durationmulti = 6f;
        }
        else if (m_throwspeed < 25f && m_throwspeed > 20f)
        {
            m_speedmultiS = 1.25f;
            m_heightmultiS = 1.6f;
            m_turnmultiS = 4.5f;
            m_fademultiS = 5.2f;

            m_durationmulti = 5f;
        }
        else if (m_throwspeed < 20f && m_throwspeed > 15f)
        {
            m_speedmultiS = 1f;
            m_heightmultiS = 1.4f;
            m_turnmultiS = 4f;
            m_fademultiS = 5.4f;

            m_durationmulti = 4f;
        }
        else if (m_throwspeed < 15f && m_throwspeed > 10f)
        {
            m_speedmultiS = 0.75f;
            m_heightmultiS = 1.2f;
            m_turnmultiS = 3.5f;
            m_fademultiS = 5.6f;

            m_durationmulti = 3f;
        }
        else if (m_throwspeed < 10f)
        {
            m_speedmultiS = 0.5f;
            m_heightmultiS = 1f;
            m_turnmultiS = 3f;
            m_fademultiS = 5.8f;

            m_durationmulti = 2f;
        }


        //Angle of Attack Area of Code
        //Get correct set of multipliers according to pitch angle value.
        //Can be optimized to not use if else statements, but havent had time to design that.
        if (m_angleofattack > 23.5f)
        {
            m_speedmultiP = 0.8f;
            m_heightmultiP = 6f;
            m_turnmultiP = 1f;
            m_fademultiP = 4f;
        }
        else if (m_angleofattack < 23.5f && m_angleofattack > 15f)
        {
            m_speedmultiP = 0.9f;
            m_heightmultiP = 4f;
            m_turnmultiP = 1f;
            m_fademultiP = 3f;
        }
        else if (m_angleofattack < 15f && m_angleofattack > 7.5f)
        {
            m_speedmultiP = 0.95f;
            m_heightmultiP = 2f;
            m_turnmultiP = 1f;
            m_fademultiP = 2f;
        }
        else if (m_angleofattack < 7.5f && m_angleofattack > 0f)
        {
            m_speedmultiP = 1f;
            m_heightmultiP = 1f;
            m_turnmultiP = 1f;
            m_fademultiP = 1f;
        }
        else if (m_angleofattack < 0f && m_angleofattack > -7.5f)
        {
            m_speedmultiP = 1f;
            m_heightmultiP = -1f;
            m_turnmultiP = 1f;
            m_fademultiP = 1f;
        }
        else if (m_angleofattack < -7.5f && m_angleofattack > -15f)
        {
            m_speedmultiP = 1f;
            m_heightmultiP = -2f;
            m_turnmultiP = 1f;
            m_fademultiP = 1f;
        }
        else if (m_angleofattack < -15f && m_angleofattack > -23.5f)
        {
            m_speedmultiP = 1f;
            m_heightmultiP = -4f;
            m_turnmultiP = 1f;
            m_fademultiP = 1f;
        }
        else if (m_angleofattack < -23.5f)
        {
            m_speedmultiP = 1f;
            m_heightmultiP = -6f;
            m_turnmultiP = 1f;
            m_fademultiP = 1f;
        }


        //Angle of Roll Area of Code
        //Get correct set of multipliers according to roll angle value.
        //Can be optimized to not use if else statements, but havent had time to design that.
        if ((m_angleofroll > 130f && m_angleofroll < 140f)||( m_angleofroll < -130f && m_angleofroll > -140f))
        {
            m_speedmultiR = 1f;
            m_heightmultiR = 1f;
            m_turnmultiR = 20f;
            m_fademultiR = -4f;
        }
        else if ((m_angleofroll > 120f && m_angleofroll < 130f)||( m_angleofroll < -120f && m_angleofroll > -130f))
        {
            m_speedmultiR = 1f;
            m_heightmultiR = 1f;
            m_turnmultiR = 7f;
            m_fademultiR = -3f;
        }
        else if ((m_angleofroll > 110f && m_angleofroll < 120f)||( m_angleofroll < -110f && m_angleofroll > -120f))
        {
            m_speedmultiR = 1f;
            m_heightmultiR = 1f;
            m_turnmultiR = 4f;
            m_fademultiR = -2f;
        }
        else if ((m_angleofroll > 100f && m_angleofroll < 110f)||( m_angleofroll < -100f && m_angleofroll > -110f))
        {
            m_speedmultiR = 1f;
            m_heightmultiR = 1f;
            m_turnmultiR = 2f;
            m_fademultiR = -1f;

        }
        else if ((m_angleofroll > 90f && m_angleofroll < 100f)||( m_angleofroll < -90f && m_angleofroll > -100f))//
        {
            m_speedmultiR = 1f;
            m_heightmultiR = 1f;
            m_turnmultiR = 0f;
            m_fademultiR = 0f;
        }
        else if ((m_angleofroll > 80f && m_angleofroll < 90f)||( m_angleofroll < -80f && m_angleofroll > -90f))
        {
            m_speedmultiR = 1f;
            m_heightmultiR = 1f;
            m_turnmultiR = -1f;
            m_fademultiR = 2f;
        }
        else if ((m_angleofroll > 70f && m_angleofroll < 80f)||( m_angleofroll < -70f && m_angleofroll > -80f))
        {
            m_speedmultiR = 1f;
            m_heightmultiR = 1f;
            m_turnmultiR = -2f;
            m_fademultiR = 4f;
        }
        else if ((m_angleofroll > 60f && m_angleofroll < 70f)||( m_angleofroll < -60f && m_angleofroll > -70f))
        {
            m_speedmultiR = 1f;
            m_heightmultiR = 1f;
            m_turnmultiR = -3f;
            m_fademultiR = 7f;
        }
        else if ((m_angleofroll > 50f && m_angleofroll < 60f)||( m_angleofroll < -50f && m_angleofroll > -60f))
        {
            m_speedmultiR = 1f;
            m_heightmultiR = 1f;
            m_turnmultiR = -4f;
            m_fademultiR = 20f;
        }
    }


    public Vector3 Point0()
    {
        //Get Beziers curves first Vector3 point depending what sort of throw player has used.
        m_point0 = DiscThrow.m_startPosition;
        return m_point0;
    }
    public Vector3 Point1()
    {
        //Calculate Beziers curves second Vector3 point depending what throw values were given.
        //This should be between 1/3 and 1/2 of the full lenght and not have additional height to mimic realistic disc throw.
        //Works well on standars shots, but needs to be immproved in ordet to count for special and irregular shots as well as uphill shots.
        z1 = ( m_throwspeed * m_speedmultiS)* DiscThrow.m_stylespeedmulti;
        x1 = m_point0.x;
        y1 = 0f;
        m_point1 = new Vector3 (x1,y1,z1);
        return m_point1;
    }
    public Vector3 Point2()
    {
        //Calculate Beziers curves third Vector3 point depending what throw values were given.
        //This should be between 2/3 and 3/4 of the full lenght and be above throws actual maximum height to mimic realistic disc throw.
        //Works well on standars shots, but needs to be immproved in ordet to count for special and irregular shots.
        z2 = (m_throwspeed * m_speedmultiS + ((m_getSpeed * m_getGlide) * (2f/3f)))* DiscThrow.m_stylespeedmulti;
        x2 = ((m_getTurn * m_turnmultiS)+m_turnmultiR)* DiscThrow.m_stylemulti;
        y2 = (m_getGlide * m_heightmultiP)* m_heightmultiS* DiscThrow.m_stylespeedmulti;
        m_point2 = new Vector3 (x2,y2,z2);
        return m_point2;
    }
    public Vector3 Point3()
    {
        //Calculate Beziers curves second Vector3 point depending what throw values were given.
        //This should be full lenght of throw and flight height decends to players ground level to mimic realistic disc throw.
        //Works well on standars shots but needs to be immproved in ordet to count for special and irregular shots as well as down hill shots.
        z3 = (((m_throwspeed * m_speedmultiS) + (m_getSpeed * m_getGlide))*m_speedmultiP)* DiscThrow.m_stylespeedmulti;
        x3 = x2+(((-m_getFade * m_fademultiS)-m_fademultiR)* DiscThrow.m_stylemulti);
        y3 = -3f;
        m_point3 = new Vector3 (x3,y3,z3);
        return m_point3;
    }

    //Setting points for Vector3 array.
    public void SetPointsToGive()
    {
        pointsToGive[0] = Point0();
        pointsToGive[1] = Point1();
        pointsToGive[2] = Point2();
        pointsToGive[3] = Point3();

    }

    //Giving array of points to bezier curve after they have been set.
    public Vector3 [] Getpoints()
    {
        SetPointsToGive();
        return pointsToGive;
    }

    //Getting Discs own Disc specific attribute values.
    public void GetValues()
    {
        m_getSpeed = GetComponentInChildren<DiscBase>().m_speed;
        m_getGlide = GetComponentInChildren<DiscBase>().m_glide;
        m_getTurn = GetComponentInChildren<DiscBase>().m_turn;
        m_getFade = GetComponentInChildren<DiscBase>().m_fade;
    }
}
