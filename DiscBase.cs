public class DiscBase : MonoBehaviour
{
    //Script that is attached to all Discs in Disc golf Experience.
    //Handels discs movement along Bezier curve, collision with terrain and checks where disc landed.

    public Bezier m_curve;                      //Reference to current Bezier curve.
    public float m_duration;                    //Float value representing how long disc flight should take.
    private float m_progress;                   //Float value representing how far of the Bezier curve disc has progressed.
    public bool m_lookForward;                  //Bool if disc should face direction of Bezier point or not.
    public GameObject m_disc;                   //Reference to disc
    [SerializeField]private Rigidbody m_rigb;   //Discs rigidbody
    public int m_caseNum;                       //Int number that represents where disc has landed.
    private bool m_Collided = false;            //Bool Has disc collided with anything.

    public float m_speed;                       //Discs attribute speed. Used for path calculation.
    public float m_glide;                       //Discs attribute glide. Used for path calculation.
    public float m_turn;                        //Discs attribute turn. Used for path calculation.
    public float m_fade;                        //Discs attribute fade. Used for path calculation. 

    public float m_timemulti;                   //Float multiplier that is used to change speed that disc travels through the Bezier path.
    private bool m_ended = false;               //Bool that turns true if disc reaches end of Bezier curve and hasn't collided with anything.


    // Update is called once per frame
    void Update()
    {
        //Get flight duration from DiscController and DiscThrow sripts.
        m_duration = (GetComponentInParent<DiscController>().m_durationmulti*DiscThrow.m_styledurationmulti);

        //If disc hasn't collided...
        if (!m_Collided) 
        {
            //...move disc along the curve
            Vector3 tepm = m_curve.GetVelocity(0.98f).normalized*m_duration;
            m_progress += Time.deltaTime / (m_duration*m_timemulti);
            Vector3 position = m_curve.GetPoint(m_progress);

            //If disc has nearly reached end of Bezier curve change m_ended to true and set velocity of disc.
            if (m_progress >= 0.99f && !m_ended)
            {
                m_ended = true;
                m_rigb.velocity = tepm;
                print("m_rigb: "+m_rigb.velocity);
            }
            //Move disc with rigidbody and velocity to last known position on the Bezier path and let physics take over. 
            else if (m_progress <= 1f)
            {
                transform.position = position;
                print("m_rigb normal: "+m_rigb);
            }
            //If bool look forward is true, transform disc along with the direction of curve.
            if (m_lookForward) 
            {
                transform.LookAt (position + m_curve.GetDirection (m_progress));
            }
        } 
    }

    //Disc has collided.
    public void OnCollisionStay(Collision m_collision)
    {
        if (m_collision != null) 
        {
            m_Collided = true;
        }

    }

    //Disc has landed. Check which trigger zone disc has landed in.
    void OnTriggerStay(Collider m_collision)
    {
        switch (m_collision.gameObject.tag)
        {
            //Disc landed out of bounds.
            case "Out":
                //print("out");
                m_caseNum = 1;
                break;

            //Disc landed inside the goal basket.
            case "Basket":
                // print("Win");
                m_caseNum = 2;
                break;

            //Disc landed on top of goal basket.
            case "NearBasket":
                //print("not greatWin");
                m_caseNum = 3;
                break;

            //Disc landed on fairway.
            default:
                //  print("NormalCase");
                m_caseNum = 4;
                break;
        }
    }
}
