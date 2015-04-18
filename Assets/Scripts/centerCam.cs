using UnityEngine;
using System.Collections;

public class centerCam : MonoBehaviour {

	private GameObject mapGen;

	void Start () {
		mapGen = GameObject.Find("mapGen");
	}

	void Update () {

		Camera.main.orthographicSize += Input.GetAxis ("Mouse ScrollWheel") * -10;
		Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, 5, 1000);
		//transform.Translate(Input.GetAxis("Mouse ScrollWheel") * 10, 0, 0);
	}
}
