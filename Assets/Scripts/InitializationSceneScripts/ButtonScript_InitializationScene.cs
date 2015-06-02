using UnityEngine;
using System.Collections;

public class ButtonScript_InitializationScene : MonoBehaviour {

	public void OnSet_Button () 
	{
		GameObject.Find ("InitializationManager").SendMessage ("SetEverthing");
	}

	public void OnCharacterDown_Button()
	{
		GameObject.Find ("InitializationManager").SendMessage ("SetSelected", gameObject.name);
	}
}
