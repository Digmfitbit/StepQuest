 using UnityEngine;
using System;
using System.Collections;
using System.Net;
using System.Collections.Generic;
using UnityEngine.UI;
using Assets.Scripts.fitbit;

/**
 * Controls loading of persistent objects and setting up the FitBit singleton
 * */
public class LoadGame : MonoBehaviour {
    
    private OAuth.Manager manager;

    string gameScreen = "MapTest";
    FitBit fitBitManager;
    public static string PLAYED_BEFORE = "played_before";

    // Use this for initialization
    void Start()
    {
        Debug.Log("Start in Load Game");
        // set up fitBit singleton
        fitBitManager = FitBit.getInstance();
        if (fitBitManager.isAuthenticated())
        {
            foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("pinUI"))
            {
                Destroy(gameObject);
            }
        }
        else
        {
            PlayerPrefs.SetInt(PLAYED_BEFORE, 0);
        }
        if(PlayerPrefs.GetInt(PLAYED_BEFORE,0) == 0)
        {
            gameScreen = "InitializationScene";
            PlayerManager.isReady = true;
        }
    }

    public void OnMouseDown()
    {
        Debug.Log("Clicked start");
        if (FitBit.getInstance().isAuthenticated() && PlayerManager.isReady)
        {
            //FindObjectOfType<DragonScript>().ShootTarget();
            Application.LoadLevel(gameScreen);
        }
    }

    void Update()
    {
        fitBitManager.Update();
    }

    public void enterPin()
    {
        FitBit.getInstance().enterPin();
    }

    //TODO remove these. Only here for show for the button
    public void getStepsSinceLastCall()
    {
        Debug.Log(FitBit.getInstance().getStepsSinceLastCall());
    }

    public void clearCache()
    {
        //TODO clear program memory for things here.
        FitBit.getInstance().clearCache();
        //FitBit.getInstance().updateAll();
    }

    public void getFriends()
    {
        foreach (string str in FitBit.getInstance().getFriendIDs())
        {
            Debug.Log(str);
        }
    }

	public void tempLoadShowroom()
	{
		Application.LoadLevel ("showroom_v2");
	}

	public void tempLoadInitScene()
	{
		Application.LoadLevel ("InitializationScene");
	}

    public void tempLoadBattleScene()
    {
        Application.LoadLevel("battleTest");
    }

    public void tempLoadBossBattleScene()
    {
        Application.LoadLevel("bossBattleTest");
    }
}
