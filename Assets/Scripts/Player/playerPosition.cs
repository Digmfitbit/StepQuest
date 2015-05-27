using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Assets.Scripts.map;
using Assets.Scripts;

public class playerPosition : MonoBehaviour {

	public int worldID;
	public int dungeonID;
	public bool inDungeon = false;

	public GameObject[] nodes;
	public GameObject nextNode;

	public GameObject[] dungeonNodes;
	public GameObject nextDungeonNode;

	public float totalSteps;
	public Text nextStepCost;

	public Text townName;

	public Text interactText;

	void Awake () {
		nodes = GameObject.FindGameObjectsWithTag("Node");
		if(nodes.Length > 0){
			transform.position = nodes[0].transform.position;
		}
	}

	void Update () {
		//Set total steps to component from players stats, will be pulled from FitBit.
		totalSteps = StepController.totalSteps;

		switch(inDungeon){
		//The character is on the world map.
		case false:
			nodes = GameObject.FindGameObjectsWithTag("Node");

			switch(nodes[worldID].GetComponent<branchMapGen>().id){
			case 0:
				interactText.text = "Enter Town";
				break;
			case 2:
				interactText.text = "Enter Dungeon";
				break;
			default:
				interactText.text = "Nothing Here";
				break;
			}

			if(nodes.Length > 0){

				//Only set next node to this if you are not on the last node.
				if(worldID < nodes.Length - 1){
					nextNode = nodes[worldID + 1];
				}

				if(worldID == 0){
					transform.position = nodes[0].transform.position;
				}
					
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
				/*if(Input.GetMouseButtonDown(0) && worldID < nodes.Length - 1 && totalSteps > nextNode.GetComponent<branchMapGen>().stepCost){
                    StepController.totalSteps -= nextNode.GetComponent<branchMapGen>().stepCost;
					Debug.Log ("Subtract " + nextNode.GetComponent<branchMapGen>().stepCost.ToString() + " steps");
					nextNode.GetComponent<branchMapGen>().stepCost = 0;
					worldID ++;
				}

				//Right click.
				else if(Input.GetMouseButtonDown (1) && worldID > 0){
					//Move to the previous node, free of charge.
					worldID --;
				}*/


	
				//Draw the next cost on screen.
				nextStepCost.text = "Next Step Cost: " + nextNode.GetComponent<branchMapGen>().stepCost.ToString ();
			}
			break;
		//The character is in a dungeon.
		case true:

			dungeonNodes = GameObject.FindGameObjectsWithTag ("DungeonNode");

			switch(dungeonNodes[dungeonID].GetComponent<branchMapGen>().id){
			case 5:
				if(dungeonNodes[dungeonID].GetComponent<branchMapGen>().hasBeenUsed == false){
					Debug.Log ("This is an item");
					Debug.Log ("You received a " + dungeonNodes[dungeonID].GetComponent<itemGenerator>().itemName);
					dungeonNodes[dungeonID].GetComponent<branchMapGen>().hasBeenUsed = true;
				}
				else{
					Debug.Log ("Nothing Here");
				}
				break;
			case 4:
				if(dungeonNodes[dungeonID].GetComponent<branchMapGen>().hasBeenUsed == false){
					Debug.Log ("This is a battle");
					Switcheroo.disable();
					Application.LoadLevelAdditive("battleTest");
					dungeonNodes[dungeonID].GetComponent<branchMapGen>().hasBeenUsed = true;
				}
				else{
				
				}
				
				break;
			default:
				break;
			}

			if(dungeonNodes.Length > 0){
				
				//Only set next node to this if you are not on the last node.
				if(dungeonID < dungeonNodes.Length - 1){
					nextDungeonNode = dungeonNodes[dungeonID + 1];
				}
				
				//Set the player position to the current node.
				transform.position = Vector3.MoveTowards(transform.position, dungeonNodes[dungeonID].transform.position, .5f);
				Vector3 temp = transform.position;
				temp.z = -1;
				transform.position = temp;
				
				//Check if the node has an event
				/*
				if(dungeonNodes[dungeonID].GetComponent<branchMapGen>().hasEvent == true){
					Debug.Log ("This node has an event!");
				}
				*/

				/*
				//Left click and make sure the player has enough steps.
				if(Input.GetMouseButtonDown(0) && dungeonID < dungeonNodes.Length - 1 && totalSteps > nextDungeonNode.GetComponent<branchMapGen>().stepCost){
                    StepController.totalSteps -= nextDungeonNode.GetComponent<branchMapGen>().stepCost;
					Debug.Log ("Subtract " + nextDungeonNode.GetComponent<branchMapGen>().stepCost.ToString() + " steps");
					nextDungeonNode.GetComponent<branchMapGen>().stepCost = 0;
					dungeonID ++;
				}
				
				//Right click.
				else if(Input.GetMouseButtonDown (1) && dungeonID > 0){
					//Move to the previous node, free of charge.
					dungeonID --;
				}
				*/

				
				//Draw the next cost on screen.
				nextStepCost.text = "Next Step Cost: " + nextDungeonNode.GetComponent<branchMapGen>().stepCost.ToString ();

			}

			break;
		
		default: 
			break;
		}
	}

	public void Interact(){
			switch(inDungeon){

			// Not in Dungeon
			case false:
					switch(nodes[worldID].GetComponent<branchMapGen>().id){
					case 0:
						Debug.Log ("This is the town of " + nodes[worldID].GetComponent<townGen>().townName.ToString ());
						nodes[worldID].GetComponent<branchMapGen>().Town();
						townName.text = nodes[worldID].GetComponent<townGen>().townName.ToString();
						GameObject.Find ("EventSystem").GetComponent<buttonFunctions>().OpenTownMenu();
						break;
					case 1:
						Debug.Log ("This is nothing here!");
						break;
					case 2:
						Debug.Log ("This is a Dungeon");
						nodes[worldID].GetComponent<branchMapGen>().dunGen = true;
						inDungeon = true;
						break;
					default:
						break;
					}
				break;

			// In Dungeon
			case true:
				switch(dungeonNodes[dungeonID].GetComponent<branchMapGen>().id){
					case 5:
						Debug.Log ("This is an item");
						Debug.Log ("You received a " + dungeonNodes[dungeonID].GetComponent<itemGenerator>().itemName);
						break;
					case 4:
						Debug.Log ("This is a battle");
						Switcheroo.disable();
						Application.LoadLevelAdditive("battleTest");
						
						break;
					case 3:
						Debug.Log ("This is the Exit");
						//dungeonNodes[dungeonID].GetComponent<branchMapGen>().dunGen = false;
						foreach(GameObject dungeonNode in dungeonNodes){
							Destroy(dungeonNode);
						}
						dungeonID = 0;
						inDungeon = false;
						break;
					default:
						break;
				}
				break;

			default:
				break;
		}
	}
}
