
using UnityEngine;
using System.Collections;

public class mapGen : MonoBehaviour {

	public GameObject[] nodeTypeSelect;
	public GameObject[] nodeType;

	public GameObject nodeEmpty;
	public GameObject nodeTown;
	public GameObject nodeDungeon;

	private int randomNodeType;
	private Vector3 randomDirect;

	public int numOfNodes;

	private GameObject[] prevNodes;

	void Start(){
		numOfNodes = Random.Range (8, 50);
		//Camera.main.orthographicSize = numOfNodes;
		
		LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();
		//lineRenderer.material = new Material(Shader.Find("Materials/Line"));
		lineRenderer.SetVertexCount(numOfNodes);
		lineRenderer.SetWidth(0.2f, 0.2f);

		nodeType = new GameObject[3];
		nodeType[0] = nodeTown;
		nodeType[1] = nodeEmpty;
		nodeType[2] = nodeDungeon;
		nodeTypeSelect = new GameObject[numOfNodes];
		GenerateMap();
	}

	void GenerateMap () {

		LineRenderer lineRenderer = GetComponent<LineRenderer>();

		for(int i = 0; i < nodeTypeSelect.Length; i++){

			randomNodeType = Random.Range(1, nodeType.Length);

			if(i == 0){
				nodeTypeSelect[i] = nodeType[0];
			}

			else if(i % Random.Range (5,6) == 0){
				nodeTypeSelect[i] = nodeType[0];
			}

			else{
				nodeTypeSelect[i] = nodeType[randomNodeType];
			}

			prevNodes = GameObject.FindGameObjectsWithTag("Node");
			Debug.Log (prevNodes.Length);


			if(prevNodes.Length > 0){
				//Instantiate(nodeTypeSelect[i], transform.position + (randomDirect + new Vector3(i*2,0,0)), Quaternion.identity);
				randomDirect = new Vector3(Random.Range (-1, 2), Random.Range(-1, 2), 0) + prevNodes[prevNodes.Length-1].transform.position;
				Instantiate(nodeTypeSelect[i], randomDirect, Quaternion.identity);
				lineRenderer.SetPosition(i,  randomDirect );
			}
			else{
				//randomDirect = new Vector3(Random.Range (-1, 2), Random.Range (-2,3), 0);
				Instantiate (nodeTypeSelect[i], transform.position, Quaternion.identity);
				lineRenderer.SetPosition(i, transform.position);
			}
			//lineRenderer.SetPosition(i, transform.position + (randomDirect + new Vector3(i*2,0,0)));
			//lineRenderer.SetPosition(i, i * 2 * Vector3.right);

		}
	}
}
