using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class playerPosition : MonoBehaviour {

	public int currentID;
	public GameObject[] nodes;
	public GameObject nextNode;

	private float totalSteps;
	public Text nextStepCost;

	void Start () {
		nodes = GameObject.FindGameObjectsWithTag("Node");
	}

	void Update () {
		nodes = GameObject.FindGameObjectsWithTag("Node");

		if(nodes.Length > 0){
			if(currentID < nodes.Length - 1){
				nextNode = nodes[currentID + 1];
			}

			totalSteps = GetComponent<playerStats>().totalSteps;
			transform.position = nodes[currentID].transform.position;

			if(Input.GetMouseButtonDown(0) && currentID < nodes.Length - 1 && totalSteps > nextNode.GetComponent<branchMapGen>().stepCost){
				GetComponent<playerStats>().totalSteps -= nextNode.GetComponent<branchMapGen>().stepCost;
				Debug.Log ("Subtract " + nextNode.GetComponent<branchMapGen>().stepCost.ToString() + " steps");
				nextNode.GetComponent<branchMapGen>().stepCost = 0;
				currentID ++;
			}
			else if(Input.GetMouseButtonDown (1) && currentID > 0
		       	 //&& totalSteps > nodes[currentID - 1].GetComponent<branchMapGen>().stepCost
		       	 ){
				//GetComponent<playerStats>().totalSteps -= nodes[currentID - 1].GetComponent<branchMapGen>().stepCost;
				currentID --;
			}
			nextStepCost.text = "Next Step Cost: " + nextNode.GetComponent<branchMapGen>().stepCost.ToString ();
		}
	}
}
