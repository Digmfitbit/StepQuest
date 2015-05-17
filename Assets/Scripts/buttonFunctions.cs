using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class buttonFunctions : MonoBehaviour {

	public InputField seedInput;

	public void RestartLevel () {
		GameObject.Find ("mapGen").GetComponent<mapGen>().seed = int.Parse(seedInput.text);
		foreach(GameObject node in GameObject.FindGameObjectsWithTag("Node")){
			Destroy(node);
		}
		Application.LoadLevel(Application.loadedLevel);
	}
	public void ExitApplication(){
		Application.Quit();
	}

	public bool Interact(){
		return true;
	}
}
