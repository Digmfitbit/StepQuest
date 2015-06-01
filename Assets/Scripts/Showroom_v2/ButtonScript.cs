using UnityEngine;
using System.Collections;

public class ButtonScript : MonoBehaviour {

	public void OnLeft_Button()
	{
		GameObject.Find ("ShowroomManager").SendMessage ("ChangeShowroom",-1);
		Debug.Log ("Button Pressed");
	}

	public void OnRight_Button()
	{
		GameObject.Find ("ShowroomManager").SendMessage ("ChangeShowroom",1);
		Debug.Log ("Button Pressed");
	}

	public void OnBack_Button()
	{
		Application.LoadLevel ("TitleScreen");
	}
}
