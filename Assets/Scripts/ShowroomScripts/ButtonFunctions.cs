using UnityEngine;
using System.Collections;

public class ButtonFunctions : MonoBehaviour 
{
	public void OnGoBackButton()
	{
		Destroy (GameObject.Find ("ShowroomHolder").gameObject);
//		Switcheroo.reEnable();
	}
}
