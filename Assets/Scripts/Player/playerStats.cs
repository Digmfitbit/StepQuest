using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class playerStats : MonoBehaviour {

	public float totalSteps;
	public float stepRate;

	//Overall level.
	public int playerLvl;
	//How much to level.
	public int expToNext;
	//How much do they currently have.
	public int currentExp;

	//How much damage.
	public int playerStrength;

	//How much hits can combo.
	public int playerStamina;

	//How much health.
	public int playerEndurance;

	//How fast you recover.
	public int playerRecovery;
	
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
