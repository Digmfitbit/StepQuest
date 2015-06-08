using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.fitbit;
using Assets.Scripts.networking;
using System.Threading;
using System;
using ResponseObjects;

public class PlayerManager : MonoBehaviour {

    public static PlayerStats mainPlayer;
	public static List<PlayerStats> fitBitFriends;
    public static bool isReady = false;
    //private const string PLAYER_MODEL_KEY = "PLAYER_MODEL";

	public static PlayerManager Instance;

    void Awake()
    {
		if(Instance){
			DestroyImmediate (gameObject);
		}
		else{
			DontDestroyOnLoad(gameObject);
			Instance = this;
		}
        //Do load??
        try
        {
            updatePlayers();
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    public static void updatePlayers()
    {
        Thread oThread = new Thread(new ThreadStart(() =>
        {
            Thread.Sleep(4000);
            FriendModel model = FitBit.getInstance().getUserModel();
            DatabaseController.updateFriendsList(FitBit.getInstance().getFriendIDs());
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

    void OnDestroy()
    {
        //PlayerPrefs.SetString(PLAYER_MODEL_KEY, mainPlayer.);
    }

	// Update is called once per frame
	void Update () {
	
	}
}
