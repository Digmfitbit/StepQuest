using UnityEngine;
using System.Collections;

public class dungeonGen : MonoBehaviour {
	
	public GameObject[] nodeTypeSelect;
	public GameObject[] nodeType;

	public GameObject nodeEmpty;
	public GameObject nodeItem;
	public GameObject nodeEnemy;
	
	private int randomNodeType;
	private Vector3 randomDirect;
	private Vector3 newPos;

	private Vector3[] randomDirects = new []{new Vector3(-1,0,0), new Vector3(1,0,0), new Vector3(0,-1,0), new Vector3(0,1,0), new Vector3(1,1,0), new Vector3(-1,-1,0)};
	
	public int numOfNodes;
	
	private GameObject[] prevNodes;
	
	void Awake(){
		//Create a number of nodes.
		numOfNodes = Random.Range (8, 20);
		//numOfNodes = 10;
		
		//Create a line renderer to connect the nodes.
		LineRenderer lineRenderer = gameObject.GetComponent<LineRenderer>();
		
		//Initial settings for the line renderer.
		lineRenderer.SetVertexCount(numOfNodes);
		lineRenderer.SetWidth(0.2f, 0.2f);
		
		//Create an array for the types of nodes.
		nodeType = new GameObject[3];
		nodeType[0] = nodeItem;
		nodeType[1] = nodeEmpty;
		nodeType[2] = nodeEnemy;
		
		//Create an array for the nodes.
		nodeTypeSelect = new GameObject[numOfNodes];
		
		//Begin the map generator.
		GenerateMap();
	}
	
	//Creates a map on a straight path with varied node types.
	void GenerateMap () {
		
		LineRenderer lineRenderer = GetComponent<LineRenderer>();
		
		//Runs a loop for each entry in the node array.
		for(int i = 0; i < nodeTypeSelect.Length; i++){
			
			//Choose random number for the node type.
			randomNodeType = Random.Range(1, nodeType.Length);
			
			//Create an exit node as the beginning point.
			if(i == 0){
				nodeTypeSelect[i] = nodeType[1];
			}
			
			//Only create an item node every 5 or 6 nodes.
			else if(i % 7 == 0){
				nodeTypeSelect[i] = nodeType[0];
			}
			
			//Otherwise, pick a random node.
			else{
				nodeTypeSelect[i] = nodeType[randomNodeType];
			}
			
			//Checks all previous nodes.
			prevNodes = GameObject.FindGameObjectsWithTag("DungeonNode");
			Debug.Log (prevNodes.Length);
			
			if(prevNodes.Length > 0){
				//Creating an integer to use in a while loop.
				//int e = 0;
				
				//while(e < 1){
				for(int e = 0; e < randomDirects.Length; e++){
					//Select a random direction for the next node.
					//randomDirect = new Vector3(Random.Range (-1, 2), Random.Range(-1, 2), 0);
					randomDirect = randomDirects[e];

					//Set the next nodes position to the random direction.
					newPos = randomDirect + prevNodes[prevNodes.Length-1].transform.position;
					
					//Check to make sure that it will not collide with another node, if it does, reset the loop.
					if(Physics.Raycast(prevNodes[prevNodes.Length-1].transform.position, randomDirect, 1) == false && randomDirect != new Vector3(0,0,0)){
						(Instantiate(nodeTypeSelect[i], newPos, Quaternion.identity) as GameObject).transform.parent = this.transform;
						lineRenderer.SetPosition(i,  newPos);
						break;
						//e = 1;
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
}
