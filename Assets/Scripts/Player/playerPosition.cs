using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Assets.Scripts.map;
using Assets.Scripts;

public class playerPosition : MonoBehaviour {

	public int worldID;
	public int dungeonID;
	public bool inDungeon = false;

	PlayerStats playerIcon;

	public GameObject[] nodes;
	public GameObject nextNode;

	public GameObject[] dungeonNodes;
	public GameObject nextDungeonNode;

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
		if(!inDungeon){
		//The character is on the world map.
			nodes = GameObject.FindGameObjectsWithTag("Node");

			switch(nodes[worldID].GetComponent<branchMapGen>().nodeType){
			case branchMapGen.NodeType.TOWN:
				interactText.text = "Enter Town";
				break;
            case branchMapGen.NodeType.DUNGEON:
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
				transform.position = Vector3.MoveTowards(transform.position, nodes[worldID].transform.position, .1f);
				Vector3 temp = transform.position;
				temp.z = -0.1f;
				transform.position = temp;
	
				//Check if the node has an event
				if(nodes[worldID].GetComponent<branchMapGen>().hasEvent == true){
					Debug.Log ("This node has an event!");
				}
	
				//Draw the next cost on screen.
				nextStepCost.text = "Next Step Cost: " + nextNode.GetComponent<branchMapGen>().stepCost.ToString ();
			}
        } else {
		    //The character is in a dungeon.
			dungeonNodes = GameObject.FindGameObjectsWithTag ("DungeonNode");

			foreach(GameObject node in dungeonNodes){
				if(node.GetComponent<branchMapGen>().u_id == dungeonID){
					switch(node.GetComponent<branchMapGen>().nodeType){
					case branchMapGen.NodeType.ITEM:
						if(node.GetComponent<branchMapGen>().hasBeenUsed == false){
							Debug.Log ("This is an item");
							node.GetComponent<itemGenerator>().GrantItem();
							node.GetComponent<branchMapGen>().hasBeenUsed = true;
						}
						else{
							Debug.Log ("Nothing Here");
						}
						break;
                    case branchMapGen.NodeType.ENEMY:
						if(node.GetComponent<branchMapGen>().hasBeenUsed == false){
							Debug.Log ("This is a battle");
							Switcheroo.disable();
							Application.LoadLevelAdditive("battleTest");
							node.GetComponent<branchMapGen>().hasBeenUsed = true;
						}						
						break;
					default:
						break;
					}
				}
			}

			if(dungeonNodes.Length > 0){
				
				//Only set next node to this if you are not on the last node.
				if(dungeonID < dungeonNodes.Length - 1){
					nextDungeonNode = dungeonNodes[dungeonID + 1];
				}
				
				//Set the player position to the current node.
				foreach(GameObject node in dungeonNodes){
					if(node.GetComponent<branchMapGen>().u_id == dungeonID){
						transform.position = Vector3.MoveTowards(transform.position, node.transform.position, .1f);

					}
				}
				//transform.position = Vector3.MoveTowards(transform.position, dungeonNodes[dungeonID].transform.position, .5f);
				Vector3 temp = transform.position;
				temp.z = -0.1f;
				transform.position = temp;
				
				//Check if the node has an event
				/*
				if(dungeonNodes[dungeonID].GetComponent<branchMapGen>().hasEvent == true){
					Debug.Log ("This node has an event!");
				}
				*/

				//Draw the next cost on screen.
				nextStepCost.text = "Next Step Cost: " + nextDungeonNode.GetComponent<branchMapGen>().stepCost.ToString ();
			}
		}
	}

	public void Interact(){
		if(!inDungeon){
			// Not in Dungeon
			switch(nodes[worldID].GetComponent<branchMapGen>().nodeType){
			case branchMapGen.NodeType.TOWN:
				Debug.Log ("This is the town of " + nodes[worldID].GetComponent<townGen>().townName.ToString ());
				nodes[worldID].GetComponent<branchMapGen>().Town();
				townName.text = nodes[worldID].GetComponent<townGen>().townName.ToString();
				GameObject.Find ("EventSystem").GetComponent<buttonFunctions>().OpenTownMenu();
				break;
            case branchMapGen.NodeType.EMPTY:
				Debug.Log ("This is nothing here!");
				break;
            case branchMapGen.NodeType.DUNGEON:
				Debug.Log ("This is a Dungeon");
				nodes[worldID].GetComponent<branchMapGen>().dunGen = true;
				inDungeon = true;
				break;
			default:
				break;
			}
		} else {
			// In Dungeon
			foreach(GameObject node in dungeonNodes){
				if(node.GetComponent<branchMapGen>().u_id == dungeonID){
					switch(node.GetComponent<branchMapGen>().nodeType){
                        case branchMapGen.NodeType.ITEM:
							Debug.Log ("This is an item");
							Debug.Log ("You received a " + node.GetComponent<itemGenerator>().itemName);
							node.GetComponent<itemGenerator>().GrantItem();	
						break;
                        case branchMapGen.NodeType.ENEMY:
							Debug.Log ("This is a battle");
							Switcheroo.disable();
							Application.LoadLevelAdditive("battleTest");
							
							break;
                        case branchMapGen.NodeType.EMPTY_D:
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
				}
            }
		}
	}
}
