using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.fitbit;
using Assets.Scripts.networking;
using System.Threading;
using ResponseObjects;

public class PlayerManager : MonoBehaviour {

    public PlayerStats mainPlayer = null;
	public List<PlayerStats> fitBitFriends;
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
                mainPlayer = new PlayerStats(model);
                DatabaseController.updatePlayer(mainPlayer);
                /*string s = PlayerPrefs.GetString(PLAYER_MODEL_KEY, "");
                if (s == "")
                {
                    mainPlayer = new PlayerStats(model.encodedId);
                }
                else
                {
                    mainPlayer = new PlayerStats(new JSONObject(s));
                }*/
                Thread oThread2 = new Thread(new ThreadStart(() =>
                {
                    Thread.Sleep(3000);
                    fitBitFriends = DatabaseController.getFriends();
                    
                }));
                oThread2.Start();

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
