using UnityEngine;
using System;
using System.Collections;
using System.Net;
using System.IO;
using System.Collections.Generic;
using UnityEngine.UI;

public class TestFitbit : MonoBehaviour {

    private static string access_token = "";
    private static string access_secret = "";

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

        manager = new OAuth.Manager();
        manager["consumer_key"] = CONSUMER_KEY;
        manager["consumer_secret"] = CONSUMER_SECRET;
        //if stored.
        if ((access_token = PlayerPrefs.GetString("token"))!= "")
        {
            access_secret = PlayerPrefs.GetString("token_secret");

            manager["token"] = access_token;
            manager["token_secret"] = access_secret;
        }
        else
        { // Need to verify. Launch browser.
            manager.AcquireRequestToken(RequestTokenURL, "POST");
            Debug.Log("token: " + manager["token"]);

            var url = SERVICE_SPECIFIC_AUTHORIZE_URL_STUB + manager["token"];
            Application.OpenURL(url);
        }
    }

    public void enterPin() {
        string pin = GameObject.Find("PinInputField").GetComponent<InputField>().text;
        manager.AcquireAccessToken(AccessTokenURL,
                         "POST",
                         pin);
        PlayerPrefs.SetString("token", manager["token"]);
        PlayerPrefs.SetString("token_secret", manager["token_secret"]);
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
                string line = "";
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    while (!reader.EndOfStream)
                    {
                        line = reader.ReadLine();
                    }
                }
                
                string[] list = line.Split(new char[] {'[','{','}'});
                //ignore first and last
                for (int i = 1; i < list.Length - 1; i++)
                {
                    if (!list[i].StartsWith("\"dateTime"))
                    {
                        continue;
                    }
                    string[] itemInfo = list[i].Split(new char[] {'\"'});
                    string dateTime = itemInfo[3];
                    string value = itemInfo[7];
                    steps += Convert.ToInt32(value);
                }
                //Example response with whitespace for clarity
                //{"activities-steps":[
                //  {"dateTime":"2015-04-13","value":"0"},
                //  {"dateTime":"2015-04-14","value":"0"},
                //  ...,
                //  {"dateTime":"2015-04-19","value":"0"}
                //]}

                Debug.Log("steps: " + steps);
            }
        }

        //return steps;
    }
}
