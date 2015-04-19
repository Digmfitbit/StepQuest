using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class playerPosition : MonoBehaviour {

	public int worldID;
	public int dungeonID;
	public bool inDungeon = false;

	public GameObject[] nodes;
	public GameObject nextNode;

	private float totalSteps;
	public Text nextStepCost;

	void Start () {
		nodes = GameObject.FindGameObjectsWithTag("Node");
		transform.position = nodes[0].transform.position;
	}

	void Update () {
		nodes = GameObject.FindGameObjectsWithTag("Node");

		switch(inDungeon){
		//The character is on the world map.
		case false:
			if(nodes.Length > 0){

				//Only set next node to this if you are not on the last node.
				if(worldID < nodes.Length - 1){
					nextNode = nodes[worldID + 1];
				}

				//Set total steps to component from players stats, will be pulled from FitBit.
				totalSteps = GetComponent<playerStats>().totalSteps;
					
				//Set the player position to the current node.
				transform.position = Vector3.MoveTowards(transform.position, nodes[worldID].transform.position, .5f);
				Vector3 temp = transform.position;
				temp.z = -1;
				transform.position = temp;
	
				//Check if the node has an event
				if(nodes[worldID].GetComponent<branchMapGen>().hasEvent == true){
					Debug.Log ("This node has an event!");
				}
	
				//Left click and make sure the player has enough steps.
				if(Input.GetMouseButtonDown(0) && worldID < nodes.Length - 1 && totalSteps > nextNode.GetComponent<branchMapGen>().stepCost){
					GetComponent<playerStats>().totalSteps -= nextNode.GetComponent<branchMapGen>().stepCost;
					Debug.Log ("Subtract " + nextNode.GetComponent<branchMapGen>().stepCost.ToString() + " steps");
					nextNode.GetComponent<branchMapGen>().stepCost = 0;
					worldID ++;
				}

				//Right click.
				else if(Input.GetMouseButtonDown (1) && worldID > 0){
					//Move to the previous node, free of charge.
					worldID --;
				}

				else if(Input.GetKeyDown(KeyCode.Return)){
					switch(nodes[worldID].GetComponent<branchMapGen>().id){
					case 0:
						Debug.Log ("This is a Town");
						nodes[worldID].GetComponent<branchMapGen>().Town();
						break;
					case 1:
						Debug.Log ("This is an Empty");
						break;
					case 2:
						Debug.Log ("This is a Dungeon");
						nodes[worldID].GetComponent<branchMapGen>().Dungeon();
						inDungeon = true;
						break;
					default:
						break;
					}
				}
	
				//Draw the next cost on screen.
				nextStepCost.text = "Next Step Cost: " + nextNode.GetComponent<branchMapGen>().stepCost.ToString ();
			}
			break;
		//The character is in a dungeon.
		case true:
			break;
		
		default: 
			break;
		}
	}
}
