public class ControllerRollAngleSlider : MonoBehaviour
{
    //Script that shows the roll angle of disc in Disc golf Experience.
    //Sript takes the direction of gamepad joystic and shows it as an angle value.

	public GameObject m_angleArrow;                     //Arrow that is rotated so that it shows on screen meter which direction joystic is pressed.
	private float m_currangle;                          //Current roll angle value as a float.
	[SerializeField]private GameObject m_backhandText;  //Text that is set active if angle corresponding backhand style throw is chosen by player.
	[SerializeField]private GameObject m_forearmText;   //Text that is set active if angle corresponding forearm style throw is chosen by player.
    [SerializeField]private GameObject m_puttText;      //Text that is set active if angle corresponding putt style throw is chosen by player.


    // Update is called once per frame
    void Update()
	{
        //Current roll angle taken from ThrowDisc script
        m_currangle = ThrowDisc.m_AngleInput;

        //Arrow rotation according current roll angle
        m_angleArrow.transform.localRotation = Quaternion.Euler (0, 0, m_currangle);

        //If angle is between 0 and 125 the throw style is backhand.
		if (m_currangle >= 0 && m_currangle <= 125)
		{
			m_backhandText.SetActive (true);
			m_forearmText.SetActive (false);
			m_puttText.SetActive (false);

		}
        //Else if angle is between less than 0 and -125 the throw style is forearm.
        else if (m_currangle < 0 && m_currangle >= -125)
		{
			m_backhandText.SetActive (false);
			m_forearmText.SetActive (true);
			m_puttText.SetActive (false);
		}
        //Else if angle is less than -125 and more than 125 the throw style is putt.
        else if ((m_currangle < -125 && m_currangle > -180)|| (m_currangle > 125 && m_currangle < 180))
		{
			m_backhandText.SetActive (false);
			m_forearmText.SetActive (false);
			m_puttText.SetActive (true);
		}
	}
}
