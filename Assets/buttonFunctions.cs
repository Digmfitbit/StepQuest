using UnityEngine;
using System.Collections;

public class buttonFunctions : MonoBehaviour {
	public void RestartLevel () {
		Application.LoadLevel(Application.loadedLevel);
	}
	public void ExitApplication(){
		Application.Quit();
	}
}
