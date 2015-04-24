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

    private static string access_token = "";
    private static string access_secret = "";
    
    private OAuth.Manager manager;
    private DateTime lastUpdated;

    string gameScreen = "MapTest";
    FitBit fitBitManager;

    // Use this for initialization
    void Start()
    {
        string dateTimeString;
        Debug.Log("starting");
        if ((dateTimeString = PlayerPrefs.GetString("LastUpdated")) != "")
        {
            lastUpdated = Convert.ToDateTime(dateTimeString);
        }
        else
        {
            lastUpdated = DateTime.MinValue;
        }
        // set up fitBit singleton
        fitBitManager = FitBit.getInstance();
        /*if (fitBitManager.isAuthenticated())
        {
            foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("pinUI"))
            {
                Destroy(gameObject);
            }
        }
        else
        {
            //fitBitManager.getToken();
        }*/
    }

    public void OnMouseDown()
    {
        Debug.Log("click start");
        if (FitBit.getInstance().isAuthenticated())
        {
            Application.LoadLevel(gameScreen);
        }
        Debug.Log("asdasd: " + FitBit.getInstance().isAuthenticated());
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

    public void getFriends()
    {
        foreach (string str in FitBit.getInstance().getFriendIDs())
        {
            Debug.Log(str);
        }
    }
}
