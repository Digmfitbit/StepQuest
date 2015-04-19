using UnityEngine;
using System.Collections;

public class branchMapGen : MonoBehaviour {

	public int id;
	public float stepCost;
	public bool isUnlocked;
	public bool hasEvent = false;

	private GameObject[] prevNodes;
	private GUIText stepCost_text;

	void Awake () {
		//Check whether or not the node will be free.
		isUnlocked = false;

		//Sets a unique ID for each individual node.
		//prevNodes = GameObject.FindGameObjectsWithTag("Node");
		//id = prevNodes.Lengt

		//Gives a change to generate an event on each node
		if(Random.Range(0,100) < 4){
			hasEvent = true;
		}

		//Sets an ID based on the type of node
		switch(gameObject.name){
		case "node_Town(Clone)":
			id = 0;
			break;
		case "node_Empty(Clone)":
			id = 1;
			break;
		case "node_Dungeon(Clone)":
			id = 2;
			break;
		default:
			id = 1;
			break;
		}

		//Assign a random value for the node cost.
		stepCost = Random.Range (50,150);
	}

	public void Town(){
		Debug.Log ("Enter the Town");
	}

	public void Dungeon(){
		Debug.Log ("Enter the Dungeon");
	}
}
