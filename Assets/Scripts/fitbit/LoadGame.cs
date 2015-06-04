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

    // Use this for initialization
    void Start()
    {
        // set up fitBit singleton
        fitBitManager = FitBit.getInstance();
        if (fitBitManager.isAuthenticated())
        {
            foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("pinUI"))
            {
                Destroy(gameObject);
            }
        }
    }

    public void OnMouseDown()
    {
        Debug.Log("Clicked start");
        if (FitBit.getInstance().isAuthenticated() && PlayerManager.isReady)
        {
            FindObjectOfType<DragonScript>().ShootTarget();
            //Application.LoadLevel(gameScreen);

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
        FitBit.getInstance().updateAll();
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

}
