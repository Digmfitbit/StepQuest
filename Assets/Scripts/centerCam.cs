using UnityEngine;
using System.Collections;

public class centerCam : MonoBehaviour {
	void Update () {

		//Scroll to zoom
		Camera.main.orthographicSize += Input.GetAxis ("Mouse ScrollWheel") * -10;
		Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, 5, 1000);

		//Center the camera at player's position.
		Vector3 temp;
		temp = transform.position;
		temp.x = GameObject.FindWithTag("Player").transform.position.x;
		temp.y = GameObject.FindWithTag("Player").transform.position.y;
		transform.position = temp;
	}
}
