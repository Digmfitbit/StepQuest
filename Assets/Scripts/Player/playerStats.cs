using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class playerStats : MonoBehaviour {

	public float totalSteps;
	public float stepRate;
	
	public int totalSteps_int;
	public Text totalSteps_text;

	void Start () {
		
	}

	void Update () {
		//This code is only for testing. This will eventually be pulled from the FitBit.
		totalSteps_int = (int)totalSteps;
		totalSteps += stepRate * Time.deltaTime;
		totalSteps_text.text = "Total Steps: " + totalSteps_int.ToString();
	}
}
