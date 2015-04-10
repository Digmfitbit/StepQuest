using UnityEngine;
using System.Collections;

public class branchMapGen : MonoBehaviour {

	public int id;

	private GameObject[] prevNodes;

	void Awake () {
		prevNodes = GameObject.FindGameObjectsWithTag("Node");
		id = prevNodes.Length;
	}
}
