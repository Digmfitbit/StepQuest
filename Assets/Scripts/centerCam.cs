using UnityEngine;
using System.Collections;

public class centerCam : MonoBehaviour {

	private GameObject mapGen;

	void Start () {
		mapGen = GameObject.Find("mapGen");
	}

	void Update () {
		transform.Translate(Input.GetAxis("Mouse ScrollWheel") * 10, 0, 0);
	}
}
