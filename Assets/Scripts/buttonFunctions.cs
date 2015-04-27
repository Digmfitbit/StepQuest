using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class buttonFunctions : MonoBehaviour {

	public InputField seedInput;

	public void RestartLevel () {
		GameObject.Find ("mapGen").GetComponent<mapGen>().seed = int.Parse(seedInput.text);
		Application.LoadLevel(Application.loadedLevel);
	}
	public void ExitApplication(){
		Application.Quit();
	}
}
