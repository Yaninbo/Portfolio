public class BasketLocation : MonoBehaviour 
{
    //Script that paints marker on UI canvas on top of the in game location of current goal basket.

	[SerializeField]private Transform m_basket;         //Goal basket set from inspector.
	[SerializeField]private Gameobject m_indicator;     //UI indicator that moves according the basket location.
    
	void Update () 
	{
        //Moveing indicator
		paint ();
	}

	void paint ()
	{
        //Baskets world position transformed to screen position.
        Vector3 screenpos = Camera.current.WorldToScreenPoint (m_basket.transform.position);

        //If screenpos is on screen set indicator active and to correct position.
        if (screenpos.z > 0 && screenpos.x > 0 && screenpos.x < Screen.width && screenpos.y > 0 && screenpos.y < Screen.height)
        {
            m_indicator.SetActive(true);
            m_indicator.transform.position = screenpos;
        }
        //Else set indicator inactive.
        else
        {
            m_indicator.SetActive(false);
        }
	}

}
