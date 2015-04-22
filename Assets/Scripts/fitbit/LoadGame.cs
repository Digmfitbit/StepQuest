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
