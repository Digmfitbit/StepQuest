using UnityEngine;
using System;
using System.Collections;
using System.Net;
using System.IO;
using System.Collections.Generic;
using UnityEngine.UI;

public class TestFitbit : MonoBehaviour {

    private string CONSUMER_KEY = "e80b47146ecb43c2b91767db41509a07";
    private string CONSUMER_SECRET = "427258ee3dd04ee78f53376ee209d26a";
    private string RequestTokenURL = "https://api.fitbit.com/oauth/request_token";
    private string AccessTokenURL = "https://api.fitbit.com/oauth/access_token";
    private static string authorizeURL = "https://www.fitbit.com/oauth/authorize";
    private static string API = "https://api.fitbit.com";
    private string NONCE = "awesdft";
    private string SERVICE_SPECIFIC_AUTHORIZE_URL_STUB = authorizeURL + "?oauth_token=";
    
    private string LAST_CALL_SINCE_URL = API + "/1/user/-/activities/steps/date/today/7d.json";

    private OAuth.Manager manager;

    // Use this for initialization
    //based off of: http://forum.unity3d.com/threads/how-do-i-request-an-oauth-token-in-fitbit-with-www.258034/
    void Start()
    {
        //if not stored.
        manager = new OAuth.Manager();
        manager["consumer_key"] = CONSUMER_KEY;
        manager["consumer_secret"] = CONSUMER_SECRET;
        manager.AcquireRequestToken(RequestTokenURL, "POST");
        Debug.Log("token: "+manager["token"]);

        var url = SERVICE_SPECIFIC_AUTHORIZE_URL_STUB + manager["token"];
        Application.OpenURL(url);

        //else if stored
        /*var oauth = new OAuth.Manager();
        oauth["consumer_key"] = CONSUMER_KEY;
        oauth["consumer_secret"] = CONSUMER_SECRET;
        oauth["token"] = your_stored_access_token;
        oauth["token_secret"] = your_stored_access_secret;*/
        
    }

    public void enterPin() {
        string pin = GameObject.Find("PinInputField").GetComponent<InputField>().text;
        manager.AcquireAccessToken(AccessTokenURL,
                         "POST",
                         pin);
    }

    public void getStepsSinceLastCall()
    {
        int steps = 0;

        Debug.Log(LAST_CALL_SINCE_URL);
        var authzHeader = manager.GenerateAuthzHeader(LAST_CALL_SINCE_URL, "GET");
        var request = (HttpWebRequest)WebRequest.Create(LAST_CALL_SINCE_URL);
        request.Method = "GET";
        request.PreAuthenticate = true;
        request.AllowWriteStreamBuffering = true;
        request.Headers.Add("Authorization", authzHeader);


        using (var response = (HttpWebResponse)request.GetResponse())
        {
            if (response.StatusCode != HttpStatusCode.OK)
                Debug.Log("There's been a problem trying to tweet:" +
                                Environment.NewLine +
                                response.StatusDescription);
            else
            {
                List<string> lines = new List<string>();
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        Debug.Log(line);
                        lines.Add(line);
                    }
                }
                Debug.Log(lines);
            }
        }

        //return steps;
    }
}
