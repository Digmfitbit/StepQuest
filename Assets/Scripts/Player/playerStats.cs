using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class playerStats : MonoBehaviour {

    public static float totalSteps;
    public static float stepRate;

	//Overall level.
	public static int playerLvl;
	//How much to level.
    public static int expToNext;
	//How much do they currently have.
    public static int currentExp;

	//How much damage.
    public static int playerStrength;

	//How much hits can combo.
    public static int playerStamina;

	//How much health.
    public static int playerEndurance;

	//How fast you recover.
    public static int playerRecovery;

    public static int totalSteps_int;
	public Text totalSteps_text;

	void Start () {
        //TODO load this from playerPrefs/database
        playerLvl = 1;
        expToNext = 100;
        currentExp = 0;
        playerStrength = 5;
        playerStamina = 5;
        playerEndurance = 5;
        playerRecovery = 5;

        stepRate = 100;

	}

	void Update () {
		//This code is only for testing. This will eventually be pulled from the FitBit.
		totalSteps_int = (int)totalSteps;
		totalSteps += stepRate * Time.deltaTime;
		totalSteps_text.text = "Total Steps: " + totalSteps_int.ToString();
	}


}
