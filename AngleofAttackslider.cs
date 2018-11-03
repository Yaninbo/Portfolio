public class AngleofAttackslider : MonoBehaviour
{
    //Script that moves Disc golf experiences pitch angle sliders and gives throw script the angle value of pitch value.
        
    public Slider Aoa1Slider;                       //Slider that shows pitch angle value on the right side of screen
    public Slider Aoa2Slider;                       //Slider that shows pitch angle value on the left side of screen
    [SerializeField] private float m_camAngle;      //Float number that moves both sliders according camera X axis
    [SerializeField] private Transform m_camera;    //Transform of main camera
    static public float m_throwAngle;               //Throw angle value set by camera transform and used by DiscThrow script


    // Update is called once per frame
    void Update() 
    {
        //Changing camera transform X axis value to float values that move sliders.

        if (m_camera.eulerAngles.x > 180f)
        {
            m_camAngle = ((m_camera.eulerAngles.x * -1f)+360f);
        }
        else
        {
            m_camAngle = (m_camera.eulerAngles.x)*-1f;
        }
            
        //Moving sliders
        Aoa1Slider.value = -m_camAngle;
        Aoa2Slider.value = -m_camAngle;
        //Setting current pitch value
        m_throwAngle = m_camAngle;
    }
}

