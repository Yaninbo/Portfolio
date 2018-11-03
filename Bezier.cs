public class Bezier : MonoBehaviour 
{
    //Script that is used to create bezier path that discs follow in Disc golf Experience.
    //Bezier path points are taken from DiscController after throw parameters have been set.

    public GameObject m_player;                     //Reference to player
    public LineRenderer m_lineRenderer; 			//Line renderer that is used for debugging purposes.
    public Vector3[] m_points;                      //Array of Vector 3 points retrived from DiscController.

    public void FixedUpdate()
    {            
        SetPoints();
    }

    //Getting any point along created Bezier path.
	public Vector3 GetPoint (float t)
	{
		return transform.TransformPoint (Bezier.GetPoint (points [0], points [1], points [2], points [3], t));
	}

    //Getting velocity on any part of created Bezier path
    public Vector3 GetVelocity (float t)
	{
		return transform.TransformPoint (Bezier.GetFirstDerivate (points [0], points [1], points [2], points [3], t)) - transform.position;
	}

    //Getting normalized direction on any part of created Bezier path
	public Vector3 GetDirection (float t)
	{
		return GetVelocity (t).normalized;
	}

    //Reset Bezier between uses (unnecessary in later version).
    public void Reset ()
	{
		points = new Vector3[] {
			new Vector3 (1f, 0f, 0f),
			new Vector3 (2f, 0f, 0f),
			new Vector3 (3f, 0f, 0f),
			new Vector3 (4f, 0f, 0f)
		};								
	}
    
    //Getting current points for Bezier path.
    public void SetPoints ()
    {
        m_points = GetComponent<DiscController>().Getpoints();            
    }

    //Creating the Bezier curve from given points.
	public static Vector3 GetPoint (Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
	{
		t = Mathf.Clamp01 (t);
		float u = 1f - t;
		float tt = t * t;
		float uu = u * u;
		float uuu = uu * u;
		float ttt = tt * t;
		return  
			uuu * p0 +
			3f * uu * t * p1 +
			3f * u * tt * p2 +
			ttt * p3;

	}

	public static Vector3 GetFirstDerivate (Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
	{
		t = Mathf.Clamp01 (t);
		float u = 1f - t;
		float tt = t * t;
		float uu = u * u;

		return  
			3f * uu *(p1 - p0) +
			6f * u * t * (p2 - p1) +
			3f * tt * (p3 - p2);
	}

    //Easy access method for other scripts to retrive information on current Bezier curve.
	public void GetBezier (float duration)
	{
		GetPoint (duration);
		GetVelocity (duration);
		GetDirection (duration);
	}
}
