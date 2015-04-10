using UnityEngine;
using System.Collections;

public class playerPosition : MonoBehaviour {

	public int currentID;
	public GameObject[] nodes;

	void Start () {
		nodes = GameObject.FindGameObjectsWithTag("Node");
	}

	void Update () {
		nodes = GameObject.FindGameObjectsWithTag("Node");

		transform.position = nodes[currentID].transform.position;
		if(Input.GetMouseButtonDown(0) && currentID < nodes.Length - 1){
			currentID ++;
		}
		else if(Input.GetMouseButtonDown (1) && currentID > 0){
			currentID --;
		}
	}
}
