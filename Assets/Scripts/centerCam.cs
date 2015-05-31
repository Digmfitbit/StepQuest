using UnityEngine;
using System.Collections;

public class centerCam : MonoBehaviour {
	void Update () {

		//Scroll to zoom
		Camera.main.fieldOfView += Input.GetAxis ("Mouse ScrollWheel") * -10;
		Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView, 90, 1000);

		//Center the camera at player's position.
		Vector3 temp;
		temp = transform.position;
		temp.x = GameObject.FindWithTag("Player").transform.position.x;
		temp.y = GameObject.FindWithTag("Player").transform.position.y;
		transform.position = temp;
	}
}
