using UnityEngine;
using System.Collections;

public class branchMapGen : MonoBehaviour {

	public int id;
	public float stepCost;
	public bool isUnlocked;

	private GameObject[] prevNodes;
	private GUIText stepCost_text;

	void Awake () {
		//Check whether or not the node will be free.
		isUnlocked = false;

		//Sets a unique ID for each individual node.
		prevNodes = GameObject.FindGameObjectsWithTag("Node");
		id = prevNodes.Length;

		//Assign a random value for the node cost.
		stepCost = Random.Range (50,150);
	}
}
