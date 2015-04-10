
using UnityEngine;
using System.Collections;

public class mapGen : MonoBehaviour {

	public GameObject[] nodeSelect;
	public GameObject[] nodes;

	public GameObject nodeEmpty;
	public GameObject nodeTown;
	public GameObject nodeDungeon;

	private int randomNode;
	private Vector3 randomDirect;

	public int numOfNodes;

	void Start(){
		numOfNodes = Random.Range (8, 50);
		//Camera.main.orthographicSize = numOfNodes;
		
		LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();
		lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
		lineRenderer.SetVertexCount(numOfNodes);
		lineRenderer.SetWidth(0.2f, 0.2f);

		nodes = new GameObject[3];
		nodes[0] = nodeTown;
		nodes[1] = nodeEmpty;
		nodes[2] = nodeDungeon;
		nodeSelect = new GameObject[numOfNodes];
		GenerateMap();
	}

	void GenerateMap () {

		LineRenderer lineRenderer = GetComponent<LineRenderer>();

		for(int i = 0; i < nodeSelect.Length; i++){

			randomNode = Random.Range(1,nodes.Length);
			randomDirect = new Vector3(1, Random.Range (-1,1), 0);

			if(i == 0){
				nodeSelect[i] = nodes[0];
			}

			else if(i % Random.Range (5, 10) == 0){
				nodeSelect[i] = nodes[0];
			}

			else{
				nodeSelect[i] = nodes[randomNode];
			}

			Instantiate(nodeSelect[i], transform.position + (randomDirect + new Vector3(i*2,0,0)), Quaternion.identity);
		
			lineRenderer.SetPosition(i, transform.position + (randomDirect + new Vector3(i*2,0,0)));

		}
	}
}
