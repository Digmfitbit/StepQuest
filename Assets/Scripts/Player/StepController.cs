using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Assets.Scripts.networking;
using ResponseObjects;

public class StepController : MonoBehaviour {

    public static float totalSteps;
    public static float stepRate = 100;
    public static int totalSteps_int;
    public Text totalSteps_text;

	// Use this for initialization
	void Start () {
        //TODO: REMOVE THIS JUST FOR TESTING
        string line = "{\"friends\":[{\"user\":{\"aboutMe\":\"\",\"avatar\":\"http://www.fitbit.com/images/profile/defaultProfile_100_male.gif\",\"city\":\"\",\"country\":\"\",\"dateOfBirth\":\"\",\"displayName\":\"Fitbit U.\",\"encodedId\":\"ABABAB\",\"fullName\":\"Fitbit User\",\"gender\":\"NA\",\"height\":190.7,\"nickname\":\"\",\"offsetFromUTCMillis\":14400000,\"state\":\"\",\"strideLengthRunning\":0,\"strideLengthWalking\":0,\"timezone\":\"Europe/Moscow\",\"weight\":0}}]}";
        JSONObject list = new JSONObject(line);
        list.GetField("friends", delegate(JSONObject hits)
        {
            foreach (JSONObject user in hits.list)
            {
                Debug.Log(user);
                user.GetField("user", delegate(JSONObject info)
                {
                    Debug.Log("friends: ");
                    FriendModel model = new FriendModel(info);
                    DatabaseController.updatePlayer(model, new playerStats());
                    Debug.Log(model.ToString());
                });
            }
        });
    }

    void Update()
    {
        //This code is only for testing. This will eventually be pulled from the FitBit.
        totalSteps_int = (int)totalSteps;
        totalSteps += stepRate * Time.deltaTime;
        totalSteps_text.text = "Total Steps: " + totalSteps_int.ToString();
    }
}
