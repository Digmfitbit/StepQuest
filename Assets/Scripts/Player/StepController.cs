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

    public const string STEPS_KEY = "TOTAL_STEPS";

	// Use this for initialization
	void Start () {
        totalSteps = PlayerPrefs.GetFloat(STEPS_KEY, 10000f);
        //comment out the above line and uncomment the below line for testing
        //totalSteps = 100000;
        //FitBit.getInstance().getUpdatedSteps();
    }

    void Update()
    {
        //This code is only for testing. This will eventually be pulled from the FitBit.
        totalSteps_int = (int)totalSteps;
        //totalSteps += stepRate * Time.deltaTime;
        totalSteps += FitBit.getInstance().getStepsSinceLastCall();
        totalSteps_text.text = "Total Steps: " + totalSteps_int.ToString();
    }

    void OnDestroy()
    {
        PlayerPrefs.SetFloat(STEPS_KEY, totalSteps);
    }
}
