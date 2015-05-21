using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Assets.Scripts.networking;
using ResponseObjects;
using Assets.Scripts.fitbit;

public class StepController : MonoBehaviour {

    public static float totalSteps;
    public static float stepRate = 100;
    public static int totalSteps_int;
    public Text totalSteps_text;

	// Use this for initialization
	void Start () {
        //TODO: REMOVE THIS JUST FOR TESTING
        //TODO MAKE THIS AN ACTUAL ID
        DatabaseController.updatePlayer(FitBit.getInstance().getUserModel(), new PlayerStats(""));
    }

    void Update()
    {
        //This code is only for testing. This will eventually be pulled from the FitBit.
        totalSteps_int = (int)totalSteps;
        totalSteps += stepRate * Time.deltaTime;
        totalSteps_text.text = "Total Steps: " + totalSteps_int.ToString();
    }
}
