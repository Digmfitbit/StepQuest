
using UnityEngine;
using System.Collections;

public class mapGen : MonoBehaviour {

	public GameObject[] nodeTypeSelect;
	public GameObject[] nodeType;

	public GameObject nodeEmpty;
	public GameObject nodeTown;
	public GameObject nodeDungeon;

	private int randomNodeType;
	private Vector2 randomDirect;
	private Vector3 newPos;

	public int numOfNodes;

	private GameObject[] prevNodes;

	
	public int seed;
	private System.Random rand;

	void Start(){
		//Begin the map generator.
		//GenerateMap();
        OnLevelWasLoaded();
	}

	//Creates a map on a straight path with varied node types.
	void GenerateMap () {
		rand = new System.Random(seed);
		
		//Create a random number of nodes.
		numOfNodes = rand.Next (8, 100);
		//numOfNodes = 50;
		
		//Create a line renderer to connect the nodes.
		LineRenderer lineRenderer = gameObject.GetComponent<LineRenderer>();
		
		//Initial settings for the line renderer.
		lineRenderer.SetVertexCount(numOfNodes);
		lineRenderer.SetWidth(0.2f, 0.2f);
		
		//Create an array for the types of nodes.
		nodeType = new GameObject[3];
		nodeType[0] = nodeTown;
		nodeType[1] = nodeEmpty;
		nodeType[2] = nodeDungeon;
		
		//Create an array for the nodes.
		nodeTypeSelect = new GameObject[numOfNodes];

		//

		//LineRenderer lineRenderer = GetComponent<LineRenderer>();

		//Runs a loop for each entry in the node array.
		for(int i = 0; i < nodeTypeSelect.Length; i++){

			//Choose random number for the node type.
			randomNodeType = rand.Next(1, nodeType.Length);

			//Create a town node as the beginning point.
			if(i == 0){
				nodeTypeSelect[i] = nodeType[0];
			}

			//Only create a town node every 5 or 6 nodes.
			else if(i % 7 == 0){
				nodeTypeSelect[i] = nodeType[0];
			}

			//Otherwise, pick a random node.
			else{
				nodeTypeSelect[i] = nodeType[randomNodeType];
			}

			//Checks all previous nodes.
			prevNodes = GameObject.FindGameObjectsWithTag("Node");
			//Debug.Log (prevNodes.Length);

			if(prevNodes.Length > 0){
				//Creating an integer to use in a while loop.
				int e = 0;

				while(e < 1){
					//Select a random direction for the next node.

					//randomDirect = new Vector3(Random.Range (-1, 2), Random.Range(-1, 2), 0);

					randomDirect = new Vector2(rand.Next(-1, 2), rand.Next(-1, 2));
					//Set the next nodes position to the random direction.
					newPos = new Vector3(randomDirect.x, randomDirect.y, 0) + prevNodes[prevNodes.Length-1].transform.position;

					//Check to make sure that it will not collide with another node, if it does, reset the loop.
					if(Physics2D.Raycast(prevNodes[prevNodes.Length-1].transform.position, randomDirect, 60) == false && randomDirect != new Vector2(0,0)){
						(Instantiate(nodeTypeSelect[i], newPos, Quaternion.identity) as GameObject).transform.parent = this.transform;
						lineRenderer.SetPosition(i,  newPos);
						e = 1;
					}
				}
			}
			//For the first node only, spawn at (0,0,0).
			else{
				(Instantiate (nodeTypeSelect[i], transform.position, Quaternion.identity) as GameObject).transform.parent = this.transform;
				lineRenderer.SetPosition(i, transform.position);
			}
		}
	}

	void OnLevelWasLoaded(){
		foreach(GameObject node in GameObject.FindGameObjectsWithTag("Node")){
			Destroy(node);
		}
		GenerateMap();
	}
}
