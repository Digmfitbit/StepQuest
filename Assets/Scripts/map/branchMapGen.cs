using UnityEngine;
using System.Collections;

public class branchMapGen : MonoBehaviour {

	public int id;
	public float stepCost;
	public bool isUnlocked;

	private GameObject[] prevNodes;
	private GUIText stepCost_text;

	void Awake () {
		isUnlocked = false;
		prevNodes = GameObject.FindGameObjectsWithTag("Node");
		id = prevNodes.Length;

		stepCost = Random.Range (50,150);
	}
}
