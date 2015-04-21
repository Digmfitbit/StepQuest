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


    // Use this for initialization
    //based off of: http://forum.unity3d.com/threads/how-do-i-request-an-oauth-token-in-fitbit-with-www.258034/
    void Start()
    {
        string dateTimeString;
        if ((dateTimeString = PlayerPrefs.GetString("LastUpdated")) != "")
        {
            lastUpdated = Convert.ToDateTime(dateTimeString);
        }
        else
        {
            lastUpdated = DateTime.MinValue;
        }
        // set up fitBit singleton
        FitBit fitBitManager = FitBit.getInstance();
    }

    //TODO remove this. Only here for show for the button
    public void getStepsSinceLastCall()
    {
        Debug.Log(FitBit.getInstance().getStepsSinceLastCall());
    }

    public void enterPin()
    {
        FitBit.getInstance().enterPin();
    }
}
