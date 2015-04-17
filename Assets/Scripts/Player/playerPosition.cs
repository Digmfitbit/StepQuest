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
		transform.position = nodes[0].transform.position;
	}

	void Update () {
		nodes = GameObject.FindGameObjectsWithTag("Node");

		if(nodes.Length > 0){

			//Only set next node to this if you are not on the last node.
			if(currentID < nodes.Length - 1){
				nextNode = nodes[currentID + 1];
			}

			//Set total steps to component from players stats, will be pulled from FitBit.
			totalSteps = GetComponent<playerStats>().totalSteps;

			//Set the player position to the current node.
			transform.position = Vector3.MoveTowards(transform.position, nodes[currentID].transform.position, .5f);
			Vector3 temp = transform.position;
			temp.z = -1;
			transform.position = temp;

			//Left click and make sure the player has enough steps.
			if(Input.GetMouseButtonDown(0) && currentID < nodes.Length - 1 && totalSteps > nextNode.GetComponent<branchMapGen>().stepCost){
				GetComponent<playerStats>().totalSteps -= nextNode.GetComponent<branchMapGen>().stepCost;
				Debug.Log ("Subtract " + nextNode.GetComponent<branchMapGen>().stepCost.ToString() + " steps");
				nextNode.GetComponent<branchMapGen>().stepCost = 0;
				currentID ++;
			}

			//Right click.
			else if(Input.GetMouseButtonDown (1) && currentID > 0){
				//Move to the previous node, free of charge.
				currentID --;
			}

			//Draw the next cost on screen.
			nextStepCost.text = "Next Step Cost: " + nextNode.GetComponent<branchMapGen>().stepCost.ToString ();
		}
	}
}
