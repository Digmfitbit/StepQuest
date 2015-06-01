using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.fitbit;
using Assets.Scripts.networking;
using System.Threading;
using ResponseObjects;

public class PlayerManager : MonoBehaviour {

    public static PlayerStats mainPlayer;
	public static List<PlayerStats> fitBitFriends;
    //private const string PLAYER_MODEL_KEY = "PLAYER_MODEL";

    void Awake()
    {
        DontDestroyOnLoad(this.transform.gameObject);
        //Do load??

        //TODO: Load from local and store local
        Thread oThread = new Thread(new ThreadStart(() =>
            {
                Thread.Sleep(4000);
                FriendModel model = FitBit.getInstance().getUserModel();
                DatabaseController.updateFriendsList(FitBit.getInstance().getFriendIDs());
                //TODO take this out from here and move it to Jonas's new scene
                mainPlayer = new PlayerStats(model);
                DatabaseController.updatePlayer(mainPlayer);
                //sets the main player object here directly
                DatabaseController.getMainPlayer(model.encodedId);
                /*string s = PlayerPrefs.GetString(PLAYER_MODEL_KEY, "");
                if (s == "")
                {
                    mainPlayer = new PlayerStats(model.encodedId);
                }
                else
                {
                    mainPlayer = new PlayerStats(new JSONObject(s));
                }*/
            }));
        oThread.Start();
    }
	// Use this for initialization
	void Start () {

    }

    void OnDestroy()
    {
        //PlayerPrefs.SetString(PLAYER_MODEL_KEY, mainPlayer.);
    }

	// Update is called once per frame
	void Update () {
	
	}
}
