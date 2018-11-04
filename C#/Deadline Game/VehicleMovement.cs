public class VehicleMovement : MonoBehaviour
{
    
    public float m_Speed;
    //The current forward speed of the ship
    [SerializeField]private CinemachineVirtualCamera m_playerCamera;
    
    [Header("Player Inputs")]
    public string m_playerSelected;                             //Player controller inputmanager name.
    public string m_OriginalPlayerSelected;
    public string m_PlayerSlot;

    public string m_VerticalAxisName = "Vertical";              //The name of the thruster axis
    public string m_HorizontalAxisName = "Horizontal";          //The name of the rudder axis
    public string m_BrakingKey = "Brake";                       //The name of the brake button
    
    [HideInInspector] public float m_Thruster;                  //The current thruster value
    [HideInInspector] public float m_Rudder;                    //The current rudder value
    [HideInInspector] public bool isBraking;                    //The current brake value
    
    [Header("Drive Settings")]
    public float m_DriveForce = 50f;                            //The force that the engine generates
    public float m_SlowingVelFactor = .90f;                     //The percentage of velocity the ship maintains when not thrusting (e.g., a value of .99 means the ship loses 1% velocity when not thrusting)
    public float m_BrakingVelFactor = .95f;                     //The percentage of velocty the ship maintains when braking
    public float m_AngleOfTurn = 45f;                           //The angle that the ship "banks" into a turn
    public float m_NoDrift = 1f;
    public float m_MaxDrift = 100f;
    public float m_DriftIntencity = 1f;
    public Transform m_trapBeedleBody;
    
    [Header("Hover Settings")]
    public float m_HoverHeight = .6f;                           //The height the ship maintains when hovering
    public float m_MaxGroundDist = 1f;                          //The distance the ship can be above the ground before it is "falling"
    public float m_HoverForce = 400f;                           //The force of the ship's hovering
    public LayerMask whatisGround;                              //A layer mask to determine what layer the ground is on
    public PIDController hoverPID;                              //A PID controller to smooth the ship's hovering
    
    [Header("Physics Settings")]
    public GameObject m_carsVisibleBody;
    public Transform m_CarBody;                                 //A reference to the ship's body, this is for cosmetics
    public float m_TerminalVelocity = 250f;                     //The max speed the ship can go
    public float m_HoverGravity = 40f;                          //The gravity applied to the ship while it is on the ground
    public float m_FallGravity = 60f;                           //The gravity applied to the ship while it is falling
    Rigidbody rigidBody;                                        //A reference to the ship's rigidbody
    float drag;                                                 //The air resistance the ship recieves in the forward direction
    bool isOnGround;                                            //A flag determining if the ship is currently on the ground
    bool isDrifting;
    public float m_crashTreshhold = 150f;
    private bool m_crashPossibility = false;

    [Header("Respawn Settings")]
    [SerializeField]private GameObject m_lastCheckpoint;  

    public int m_playerNum;

    [Header("Particle Settings")]
    public ParticleSystem[] dustTrail = new ParticleSystem[0];
    private int m_emissionRate = 0;
    [SerializeField] private bool hasHitted = false;
    [SerializeField] private GameObject effect;
    [SerializeField] private float m_Effectdestroytime = 2f;

    [Header("Ability Settings")]    
    public List<SkillP2> skills;

    [Header("Weapon Settings")]
    [SerializeField] private float m_bulletSpeed = 500f;
    [SerializeField] private GameObject m_bullet;
    [SerializeField] private Transform m_bulletSpawn;

    [Header("Audio Settings")]
    [SerializeField] private float m_audioDivider = 800f;
    [SerializeField] private bool m_engineStarted = false;
    [SerializeField] private MultiAudioSource m_EngineAudio;
    [SerializeField] private MultiAudioSource m_BoostStartAudio;
    [SerializeField] private MultiAudioSource m_FlameAudio;

    [SerializeField] private float m_EngineMinVolume = .3f;
    [SerializeField] private float m_EngineMaxVolume = .45f;
    [SerializeField] private float m_EngineMinPitch = .1f;          
    [SerializeField] private float m_EngineMaxPitch = .8f;

    [SerializeField] private float m_FlamesMinVolume = .3f;
    [SerializeField] private float m_FlamesMaxVolume = .8f;
    [SerializeField] private float m_FlamesMinPitch = .3f;
    [SerializeField] private float m_FlamesMaxPitch = .7f;

    
    // Use this for initialization
    void OnEnable()
    {
        RaceHolding();
    
        rigidBody = GetComponent<Rigidbody>();

        if (m_trapBeedleBody.name == "BubbleBody")
        {
            m_trapBeedleBody = GameObject.Find("BubbleBody").transform;
        }
    
        //Calculate the ship's drag value
        drag = m_DriveForce / m_TerminalVelocity;


    }
        
    // Update is called once per frame
    void Update()
    {
        if (RaceStartup.m_raceStarted == false)
        {
            m_engineStarted = false;
            m_PlayerSlot = GetComponentInParent<DLPlayerSettings>().m_player;
            return;
        }
        else
        {
            //Get the values of the thruster, rudder, and brake from the input class
            m_Thruster = Input.GetAxis(m_playerSelected + "Throttle");
            m_Rudder = Input.GetAxis(m_playerSelected + m_HorizontalAxisName);
            isBraking = Input.GetButton("Player1Brake");

            if (!m_engineStarted)
            {
                m_EngineAudio.Play();
                m_FlameAudio.Play();
                m_engineStarted = true;
            }
        }
        if (m_Thruster >= 0f)
        {
        }
        else if (m_Thruster < 0f)
        {
            m_Rudder = -(Input.GetAxis(m_playerSelected + m_HorizontalAxisName));
        }

        float m_SpeedPercent = GetSpeedPercentage();

        m_EngineAudio.Volume = Mathf.Lerp(m_EngineMinVolume, m_EngineMaxVolume, m_SpeedPercent);            //++++++++
        m_EngineAudio.Pitch = Mathf.Lerp(m_EngineMinPitch, m_EngineMaxPitch, m_SpeedPercent);

        m_FlameAudio.Volume = Mathf.Lerp(m_FlamesMinVolume, m_FlamesMaxVolume, m_SpeedPercent);
        m_FlameAudio.Pitch = Mathf.Lerp(m_FlamesMinPitch, m_FlamesMaxPitch, m_SpeedPercent);

    
    
        //Starts to slowly increase drift value if drift button is pressed
        if (Input.GetButton(m_playerSelected + "Drift") && m_DriftIntencity < m_MaxDrift)
        {
            m_DriftIntencity = m_DriftIntencity * 1.05f;
            print("Drift on");
            isDrifting = true;
        }
        else if (Input.GetButton(m_playerSelected + "BoostDrift"))
        {
            m_DriftIntencity = 1000f;
            m_BrakingVelFactor = 1f;
            m_SlowingVelFactor = 1f;
            print("BoostDrift on");
            isDrifting = true;
        }
        else if (Input.GetButtonUp(m_playerSelected + "BoostDrift"))
        {
            m_DriftIntencity = 1f;
            m_BrakingVelFactor = .95f;
            m_SlowingVelFactor = .90f;
    
            print("BoostDrift off");
            isDrifting = true;
        }        
    
        //...and slowly decrease the value when drift button isn't pressed
        else if (isDrifting == true && !Input.GetButton(m_playerSelected + "Drift"))
        {
            m_DriftIntencity = m_DriftIntencity * .90f;
    
            if (m_DriftIntencity < 1f)
            {
                isDrifting = false;
                print("dRIFT False");
                m_DriftIntencity = 1f;
            }
        }
        else if (Input.GetButton(m_playerSelected + "Respawn"))
        {
            StartCoroutine(Respawn());
        }

        foreach (SkillP2 s in skills)
        {
            if (s.m_CurrentCooldown < s.m_Cooldown)                                  //Current cooldown while it's running. Goes from 0 to skill Cooldown.
            {
                s.m_CurrentCooldown += Time.deltaTime;                              //Remaining cooldown goes from 0 to skill Cooldown.
                s.m_SkillIcon.fillAmount = s.m_CurrentCooldown / s.m_Cooldown;      //Fills the icon with 360 radial fill
                s.m_TimeText.text = s.m_CurrentCooldown.ToString("F0");             //Updates remaining cooldown Text to skill icon
            }
            else if (s.m_CurrentCooldown >= s.m_Cooldown)                           //Cooldown finished
            {
                s.m_TimeText.enabled = false;                                       //Hide cooldown text while skill cooldown isn't happening
            }
        }
    

        if (Input.GetButton(m_playerSelected + "Boost"))                              //When key is pressed
        {
            if (skills[0].m_CurrentCooldown >= skills[0].m_Cooldown)                 //Cooldown is happening?
            {
                StartCoroutine(Nitro());//Do something
                skills[0].m_TimeText.enabled = true;                                //Enable cooldown time Text
                skills[0].m_CurrentCooldown = 0;
            }
        }
        else if (Input.GetButtonDown(m_playerSelected + "Fire"))
        {
            if (skills[1].m_CurrentCooldown >= skills[1].m_Cooldown)
            {
                Fire();
                skills[1].m_TimeText.enabled = true;
                skills[1].m_CurrentCooldown = 0;
            }
        }
        if (DLSetFinish.m_raceFinished == true)
        {
            RaceHolding();
        }
    }

    private void FixedUpdate()
    {
    
        //Calculate the current speed by using the dot product. This tells us
        //how much of the ship's velocity is in the "forward" direction 
        m_Speed = Vector3.Dot(rigidBody.velocity, transform.forward);
    
        //Calculate the forces to be applied to the ship
        CalculateHover();
        CalculatePropulsion();  
    
        //Particle Effects
        if (isOnGround)
        {
            m_emissionRate = 1 * ((int)m_Speed / 1);
            //print("emission Rate: " + m_emissionRate);
        }
        else
        {  
        }
    
        for (int i = 0; i < dustTrail.Length; i++)
        {
            var emission = dustTrail[i].emission;
            emission.rate = new ParticleSystem.MinMaxCurve(m_emissionRate);
        }
    
    }

    public void Controller()
    {
            
        m_OriginalPlayerSelected = GetComponentInParent<DLPlayerSettings>().m_ActiveController;
        m_playerSelected = m_OriginalPlayerSelected;

    }

    void CalculateHover()
    {
        //This variable will hold the "normal" of the ground. Think of it as a line
        //the points "up" from the surface of the ground
        Vector3 groundNormal;
    
        //Calculate a ray that points straight down from the ship
        Ray ray = new Ray(transform.position, -transform.up);
    
        //Declare a variable that will hold the result of a raycast
        RaycastHit hitInfo;
    
        //Determine if the ship is on the ground by Raycasting down and seeing if it hits 
        //any collider on the whatIsGround layer
        isOnGround = Physics.Raycast(ray, out hitInfo, m_MaxGroundDist, whatisGround);
    
        //If the ship is on the ground...
        if (isOnGround)
        {
            //...determine how high off the ground it is...
            float height = hitInfo.distance;
            //...save the normal of the ground...
            groundNormal = hitInfo.normal.normalized;
            //...use the PID controller to determine the amount of hover force needed...
            float forcePercent = hoverPID.Seek(m_HoverHeight, height);
    
            //...calulcate the total amount of hover force based on normal (or "up") of the ground...r
            Vector3 force = groundNormal * m_HoverForce * forcePercent;
            //...calculate the force and direction of gravity to adhere the ship to the 
            //track (which is not always straight down in the world)...
            Vector3 gravity = -groundNormal * m_HoverGravity * height;
    
            //...and finally apply the hover and gravity forces
            rigidBody.AddForce(force, ForceMode.Acceleration);
            rigidBody.AddForce(gravity, ForceMode.Acceleration);
        }
        //...Otherwise...
        else
        {
            //...use Up to represent the "ground normal". This will cause our ship to
            //self-right itself in a case where it flips over
            groundNormal = Vector3.up;
    
            //Calculate and apply the stronger falling gravity straight down on the ship
            Vector3 gravity = -groundNormal * m_FallGravity;
            rigidBody.AddForce(gravity, ForceMode.Acceleration);
        }
    
        //Calculate the amount of pitch and roll the ship needs to match its orientation
        //with that of the ground. This is done by creating a projection and then calculating
        //the rotation needed to face that projection
        Vector3 projection = Vector3.ProjectOnPlane(transform.forward, groundNormal);
        Quaternion rotation = Quaternion.LookRotation(projection, groundNormal);
    
        //Move the ship over time to match the desired rotation to match the ground. This is 
        //done smoothly (using Lerp) to make it feel more realistic
        rigidBody.MoveRotation(Quaternion.Lerp(rigidBody.rotation, rotation, Time.deltaTime * 10f));

        if (this.name == "TrapBeedle")
        {
            float m_Angle = m_AngleOfTurn * m_Rudder;
            Quaternion m_BodyRotation = transform.rotation * Quaternion.Euler(0f, 180f, m_Angle);
            m_trapBeedleBody.rotation = Quaternion.Lerp(m_trapBeedleBody.rotation, m_BodyRotation, Time.deltaTime * 5f);
        }
    }

        
    
    void CalculatePropulsion()
    {
        //Calculate the yaw torque based on the rudder and current angular velocity.
        float rotationTorque = m_Rudder - rigidBody.angularVelocity.y;

        //Apply the torque to the car's Y axis
        rigidBody.AddRelativeTorque(0f, rotationTorque, 0f, ForceMode.VelocityChange);
    
        //Calculate the current sideways speed by using the dot product. This tells us
        //how much of the ship's velocity is in the "right" or "left" direction
        float sidewaysSpeed = Vector3.Dot(rigidBody.velocity, transform.right);
    
        //Calculate the desired amount of friction to apply to the side of the vehicle. This
        //is what keeps the ship from drifting into the walls during turns. If you want to add
        //drifting to the game, divide Time.fixedDeltaTime by some amount
        Vector3 sideFriction = -transform.right * (sidewaysSpeed / Time.fixedDeltaTime / m_DriftIntencity);        
    
        //Apply the sideways friction
        rigidBody.AddForce(sideFriction, ForceMode.Acceleration);
    
        //If not propelling the ship, slow the ships velocity
        if (m_Thruster <= 0f)
            rigidBody.velocity *= m_SlowingVelFactor;
    
        //Braking or driving requires being on the ground, so if the ship
        //isn't on the ground, exit this method
        if (!isOnGround)
        {
            return;        
        }
    
        //If the ship is braking, apply the braking velocty reduction
        if (isBraking)
            rigidBody.velocity *= m_BrakingVelFactor;
    
        //Calculate and apply the amount of propulsion force by multiplying the drive force
        //by the amount of applied thruster and subtracting the drag amount
        float propulsion = m_DriveForce * m_Thruster - drag * Mathf.Clamp(m_Speed, 0f, m_TerminalVelocity);
        rigidBody.AddForce(transform.forward * propulsion, ForceMode.Acceleration);   
    }

    public float GetSpeedPercentage()
    {
        //Returns the total percentage of speed the car is traveling.
        return rigidBody.velocity.magnitude / m_audioDivider;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Killzone"))
        {
            StartCoroutine(Respawn());
        }
        else if (other.CompareTag("Checkpoint"))
        {
            m_lastCheckpoint = other.gameObject;

        }
        else if (other.CompareTag("Boulder"))
        {
            GetComponent<ReceiverforSwitch>().Hit();
        }
        else if (other.CompareTag("Goal"))
        {
            RaceHolding();
        }
        else if (other.CompareTag("NotPlayer"))
        {
            m_crashPossibility = true;
            print("CP on");
        }
    }


    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == ("Cactus"))
        {
            StartCoroutine(Slowdown());
        }
                
        if (collision.relativeVelocity.magnitude > m_crashTreshhold)
        {
            Crashed();
        }
        else
        {
            m_crashPossibility = false;
        }
    }
       
    void Fire()
    {
        var bulletClone = (GameObject)Instantiate(m_bullet, m_bulletSpawn.position, m_bulletSpawn.rotation);
        bulletClone.GetComponent<Rigidbody>().velocity = m_bulletSpawn.transform.forward * m_bulletSpeed;
    }

    public void Crashed()
    {
        print("Crashed");
        if (m_crashPossibility == true)
        {
            GetComponent<ReceiverforSwitch>().Hit();
            m_crashPossibility = false;
        }
    }

    public void RaceHolding()
    {
        print("Holding");
        string PlayerCont = "Nope";
        m_playerSelected = PlayerCont;
    }

    public void RaceStarted()
    {
        print("Started");
        m_playerSelected = m_OriginalPlayerSelected;
    }
            

    private IEnumerator Gothit()
    {            
        m_Thruster = 0f;
        yield return new WaitForSeconds(1f);
        m_Thruster = Input.GetAxis(m_playerSelected + "Throttle");    
    }

    public IEnumerator Slowdown()
    {
        rigidBody.velocity = new Vector3(0f, 0f, 0f);
        m_Thruster = 0f;
        m_TerminalVelocity = 0f;
        yield return new WaitForSeconds(0.5f);
        m_Thruster = Input.GetAxis(m_playerSelected + "Throttle");
        m_TerminalVelocity = 500f;
    }

    public IEnumerator Respawn()
    {
        m_carsVisibleBody.SetActive(false);
        rigidBody.velocity = new Vector3(0f, 0f, 0f);
        m_Thruster = 0f;
        m_TerminalVelocity = 0f;
        yield return new WaitForSeconds(1f);
        m_carsVisibleBody.SetActive(true);
        transform.position = m_lastCheckpoint.transform.position;
        transform.localRotation = m_lastCheckpoint.transform.localRotation;
        rigidBody.velocity = new Vector3(0f, 0f, 0f);
        m_Thruster = 0f;
        m_TerminalVelocity = 0f;
        yield return new WaitForSeconds(1f);
        m_Thruster = Input.GetAxis(m_playerSelected + "Throttle");
        m_TerminalVelocity = 500f;    
    }

    private IEnumerator Nitro()
    {
        m_BrakingVelFactor = 1f;
        m_SlowingVelFactor = 1f;
        m_DriveForce = 100f;
        m_TerminalVelocity = 400f;
        SendMessage("Detach");
        m_BoostStartAudio.Play();                                                          
    
        yield return new WaitForSeconds(4);
        m_DriveForce = 50f;
        m_TerminalVelocity = 250f;
     
        m_BrakingVelFactor = .95f;
        m_SlowingVelFactor = .90f;

    }
}
