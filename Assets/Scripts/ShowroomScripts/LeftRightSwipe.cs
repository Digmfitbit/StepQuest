using UnityEngine;
using System.Collections;

public class LeftRightSwipe : MonoBehaviour {

	private Vector3 objectHome;
	private Vector3 objectPositionOld;
	private float mouseOffset;
	private float positionLeft;
	private float positionRight;

	private bool goRight, goLeft = false;
	private bool draging;
	private bool lerpToHomePos = true;
	private float lerpSpeedLeftRight = 6f;

	// Use this for initialization
	void Start ()
	{
		objectHome = transform.position;
		positionLeft = objectHome.x - 10f;
		positionRight = objectHome.x + 10f;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetMouseButtonDown (0)) 
		{
			mouseOffset = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10)).x - transform.position.x;
			Debug.Log("MouseOffset"+ mouseOffset);
		}

		if (!draging) 
		{
			if (goRight) 
			{
				transform.position = new Vector2 (Mathf.Lerp (transform.position.x, positionRight, Time.deltaTime * lerpSpeedLeftRight), 0f);
				if (Mathf.Abs (transform.position.x - positionRight) < 0.2) 
				{
					transform.position = new Vector2(positionRight,0f);
					objectHome = transform.position;
					goRight = false;
					SetNewHome(transform.position.x);
				}
			} 
			else if (goLeft) 
			{
				transform.position = new Vector2 (Mathf.Lerp (transform.position.x, positionLeft, Time.deltaTime * lerpSpeedLeftRight), 0f);
				if (Mathf.Abs (transform.position.x - positionLeft) < 0.2) 
				{
					transform.position = new Vector2(positionLeft,0f);
					objectHome = transform.position;
					goLeft = false;
					SetNewHome(transform.position.x);
				}
			} 

			if(lerpToHomePos)
			{
				transform.position = new Vector3( Mathf.Lerp (transform.position.x, objectHome.x, Time.deltaTime * lerpSpeedLeftRight),0f);
			}
		}
	}

	void OnMouseDrag()
	{
		draging = true;
		Vector3 mousePos =  Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
		Vector3 objPosNew = new Vector3(mousePos.x - mouseOffset, 0f, 0f);
		transform.position =  objPosNew;
	}

	void OnMouseDown()
	{
		draging = true;


	}

	void OnMouseUp()
	{
		if (transform.position.x > positionRight-5) 
		{
			goRight = true;
			lerpToHomePos = false;
		} 
		else if (transform.position.x < positionLeft+5) 
		{
			goLeft = true;
			lerpToHomePos = false;

		}
		draging = false;
	}

	void SetNewHome(float newHomeX_)
	{
		objectHome.x = newHomeX_;
		positionLeft = objectHome.x - 10f;
		positionRight = objectHome.x + 10f;

		lerpToHomePos = true;

		Debug.Log("Home: "+ objectHome.x +"Left: " + positionLeft + "Right: " + positionRight);
	}
}
