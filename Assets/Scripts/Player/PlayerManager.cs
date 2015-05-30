using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.fitbit;
using Assets.Scripts.networking;
using System.Threading;
using ResponseObjects;

public class PlayerManager : MonoBehaviour {

    public PlayerStats mainPlayer = new PlayerStats("");
	public List<PlayerStats> fitBitFriends = new List<PlayerStats>();

    void Awake()
    {
        DontDestroyOnLoad(this.transform.gameObject);
        //Do load??

        //TODO: REMOVE THIS JUST FOR TESTING
        Thread oThread = new Thread(new ThreadStart(() =>
            {
                Thread.Sleep(4000);
                FriendModel model = FitBit.getInstance().getUserModel();
                DatabaseController.updateFriendsList(FitBit.getInstance().getFriendIDs());
                DatabaseController.updatePlayer(model, new PlayerStats(""));
            }));
        oThread.Start();
    }
	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
