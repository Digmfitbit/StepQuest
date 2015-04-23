using UnityEngine;
using System.Collections;
using System;
using ResponseObjects;

public class TestJSONParser : MonoBehaviour {

	// Use this for initialization
	void Start () {
        string line = "{\"friends\":[{\"user\":{\"aboutMe\":\"\",\"avatar\":\"http://www.fitbit.com/images/profile/defaultProfile_100_male.gif\",\"city\":\"\",\"country\":\"\",\"dateOfBirth\":\"\",\"displayName\":\"Fitbit U.\",\"encodedId\":\"2246K9\",\"fullName\":\"Fitbit User\",\"gender\":\"NA\",\"height\":190.7,\"nickname\":\"\",\"offsetFromUTCMillis\":14400000,\"state\":\"\",\"strideLengthRunning\":0,\"strideLengthWalking\":0,\"timezone\":\"Europe/Moscow\",\"weight\":0}}]}";
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
                    Debug.Log(model.ToString());
                });
            }
        });
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
