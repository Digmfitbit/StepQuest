using UnityEngine;
using System.Collections;

public class branchMapGen : MonoBehaviour {

    public enum NodeType {TOWN=0, EMPTY=1, DUNGEON=2, EMPTY_D=3, ENEMY=4, ITEM=5, DUNGEON_EMPTY=6};

    const int LOWER_STEP_BOUND = 50;
    const int UPPER_STEP_BOUND = 150;

    public NodeType nodeType;
	public int u_id;
	public float stepCost;
	public bool isUnlocked;
	public bool hasEvent = false;

	public bool dunGen = false;

	public bool hasBeenUsed = false;

	private GameObject[] prevNodes;
	private GUIText stepCost_text;

	public int seed;
	private System.Random rand;

	void Awake () {
		//Check whether or not the node will be free.
		isUnlocked = false;

		//Sets an ID based on the type of node
		switch(gameObject.name){
		case "node_Town(Clone)":
			nodeType = NodeType.TOWN;
			break;
		case "node_Empty(Clone)":
            nodeType = NodeType.EMPTY;
			break;
		case "node_Dungeon(Clone)":
            nodeType = NodeType.DUNGEON;
			break;
		case "node_d_Empty(Clone)":
            nodeType = NodeType.EMPTY_D;
			break;
		case "node_Enemy(Clone)":
            nodeType = NodeType.ENEMY;
			break;
		case "node_Item(Clone)":
            nodeType = NodeType.ITEM;
			break;
		case "node_Dungeon_Empty(Clone)":
            nodeType = NodeType.DUNGEON_EMPTY;
			break;
		default:
            nodeType = NodeType.EMPTY;
            break;
		}

		//Sets a unique ID for each individual node.
		if(nodeType < NodeType.EMPTY_D){
			prevNodes = GameObject.FindGameObjectsWithTag("Node");
		}
		else{
			prevNodes = GameObject.FindGameObjectsWithTag("DungeonNode");
		}
        if (nodeType == NodeType.EMPTY_D)
        {
			u_id = 0;
		}
		else{
			u_id = prevNodes.Length - 1;
		}

		seed = u_id + GameObject.FindWithTag("Player").GetComponent<playerPosition>().worldID;
		rand = new System.Random(seed);

		//Gives a change to generate an event on each node
		if(rand.Next(0,100) < 4){
			hasEvent = true;
		}

		//Assign a random value for the node cost.
        stepCost = rand.Next(LOWER_STEP_BOUND, UPPER_STEP_BOUND);
	}

	void Update(){
		if(dunGen == true){
			(Instantiate(Resources.Load("Prefabs/DungeonPrefabs/node_d_Empty") as GameObject, transform.position, Quaternion.identity) as GameObject).transform.parent = this.transform;
			//GameObject.Find ("node_d_Empty(Clone)").GetComponent<dungeonGen>().seed = u_id;
			Debug.Log("Created Dungeon");
			dunGen = false;
		}
	}

	public void Town(){
		Debug.Log ("Enter the Town");
	}

	public void Dungeon(){
		Debug.Log ("Enter the Dungeon");
	}

	void OnMouseDown(){
		Debug.Log("Node " + u_id.ToString());
        playerPosition playerPos = GameObject.FindWithTag("Player").GetComponent<playerPosition>();
        if (playerPos.inDungeon)
        {
            if (StepController.totalSteps > stepCost && (playerPos.dungeonID - 1 == u_id || playerPos.dungeonID + 1 == u_id))
            {
                StepController.totalSteps -= stepCost;
                playerPos.dungeonID = u_id;
                stepCost = 0;
            }
        }
        else
        {
            if (StepController.totalSteps > stepCost && (playerPos.worldID - 1 == u_id || playerPos.worldID + 1 == u_id))
            {
                StepController.totalSteps -= stepCost;
                playerPos.worldID = u_id;
                stepCost = 0;
            }
        }
	}
}
