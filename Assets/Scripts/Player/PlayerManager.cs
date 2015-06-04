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
    public static bool isReady = false;
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
                //TODO CHANGE THIS
                DatabaseController.updatePlayer(mainPlayer);
                //sets the main player object here directly
                DatabaseController.getMainPlayer(model.encodedId);
                
                Thread startGameThread = new Thread(new ThreadStart(() =>
                {
                    Thread.Sleep(2500);
                    //start game
                    isReady = true;
                }));
                startGameThread.Start();
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
