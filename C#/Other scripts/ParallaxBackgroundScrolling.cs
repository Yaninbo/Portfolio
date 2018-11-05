public class ParallaxBackgroundScrolling : MonoBehaviour
{
    //This script is attached to gameobject that has single layers scrollable or parallax sprites as its childs.
    //This script handels single layers scrolling and/ or parallax effext in a 2D game.

    public bool m_scrolling, m_parallax;                    //Boolean lock for if the layer this script is attached is scrollable and is if it has parallax effect.
    public float m_layerElementSize;                        //Float value determening the layer elements width.
    public float m_parallaxSpeed;                           //Float value of how much parallax effect there is.

    [SerializeField]private Transform m_cameraTransform;    //Cameras transform.
    [SerializeField]private Transform [] m_layers;          //Array of scrollable layer sprites.
    private float m_viewZone = 5f;                         //
    private float m_lastCameraX;                            //Cameras last position on the X-axis.
    private int m_leftIndex;                                //
    private int m_rightIndex;                               //


    // Start is called before the first frame update
    void Start()
    {
        //Lets give camera transform Main cameras transform component.
        m_cameraTransform = Camera.main.transform;
        //LastCameraX gets cameraTransforms X-axis value.
        m_lastCameraX = m_cameraTransform.position.x;
        //Layers Transform array gets the transforms of child objects. 
        m_layers = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            m_layers[i] = transform.GetChild(i);
        }
        //Left index is the array value of first child and right index is the array value od last child. 
        m_leftIndex = 0;
        m_rightIndex = m_layers.Length - 1;
    }

    // Update is called once per frame
    void Update()
    {
        //If parallax effect is on.
        if (m_parallax)
        {
            //Create temp variable deltaX.
            float deltaX = m_cameraTransform.position.x - m_lastCameraX;
            //Use deltaX and parallax speed to change X-axis movement speed of layer to create the parallax effect.
            transform.position += Vector3.right * (deltaX * m_parallaxSpeed);
        }

        //Update lastCameraX position
        m_lastCameraX = m_cameraTransform.position.x;

        //If scrolling is on.
        if (m_scrolling)
        {
            //If camera view is about to get to the end of most left layer element.
            if (m_cameraTransform.position.x < (m_layers[m_leftIndex].transform.position.x + m_viewZone))
            {
                //Scroll playarea to the left.
                ScrollLeft();
            }

            //If camera view is about to get to the end of most right layer element.
            if (m_cameraTransform.position.x > (m_layers[m_rightIndex].transform.position.x - m_viewZone))
            {
                //Scroll playarea to the right.
                ScrollRight();
            }
        }
    }

    void ScrollLeft()
    {
        int lastRight = m_rightIndex;
        m_layers[m_rightIndex].position = Vector3.right * (m_layers[m_leftIndex].position.x - m_layerElementSize);
        m_leftIndex = m_rightIndex;
        m_rightIndex--;
        if (m_rightIndex < 0)
        {
            m_rightIndex = m_layers.Length - 1;
        }
    }
    void ScrollRight()
    {
        int lastLeft = m_leftIndex;
        m_layers[m_leftIndex].position = Vector3.right * (m_layers[m_rightIndex].position.x + m_layerElementSize);
        m_rightIndex = m_leftIndex;
        m_leftIndex++;
        if (m_leftIndex == m_layers.Length)
        {
            m_leftIndex = 0;
        }
    }
}
