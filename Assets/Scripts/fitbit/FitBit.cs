using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using UnityEngine.UI;
using UnityEngine;
//using Assets.Scripts;

using System.IO;

namespace Assets.Scripts.fitbit
{
    class FitBit
    {
        private static string API_BASE = "https://api.fitbit.com";
        private string LAST_CALL_SINCE_URL = API_BASE + "/1/user/-/activities/steps/date/today/7d.json";
        private string FRIENDS_URL = API_BASE + "/1/user/-/friends.json";

        private static FitBit instance;
        private static OAuth.Manager manager;

        private static string CONSUMER_KEY = "e80b47146ecb43c2b91767db41509a07";
        private static string CONSUMER_SECRET = "427258ee3dd04ee78f53376ee209d26a";
        private static string RequestTokenURL = "https://api.fitbit.com/oauth/request_token";
        private static string AccessTokenURL = "https://api.fitbit.com/oauth/access_token";
        private static string authorizeURL = "https://www.fitbit.com/oauth/authorize";
        private static string SERVICE_SPECIFIC_AUTHORIZE_URL_STUB = authorizeURL + "?oauth_token=";

        static string access_token;
        static string access_secret;

        private static string LAST_UPDATED_KEY = "LastUpdated";

        private FitBit()
        {
            manager = new OAuth.Manager();
            manager["consumer_key"] = CONSUMER_KEY;
            manager["consumer_secret"] = CONSUMER_SECRET;
            //if stored.
            if ((access_token = PlayerPrefs.GetString("token")) != "")
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

        public static FitBit getInstance()
        {
            if(instance==null){
                instance = new FitBit();
            }
            return instance;
        }

        public OAuth.Manager getManager(){
            return manager;
        }
        /**
         * Relies on having an InputField in scene called PinInputField
         * */
        public void enterPin()
        {
            string pin = GameObject.Find("PinInputField").GetComponent<InputField>().text;
            manager.AcquireAccessToken(AccessTokenURL,
                             "POST",
                             pin);
            PlayerPrefs.SetString("token", manager["token"]);
            PlayerPrefs.SetString("token_secret", manager["token_secret"]);
        }

        /**
         * Gets the string identifiers for the authenticated user's friends list
         * */
        public List<string> getFriendIDs()
        {
            var authzHeader = manager.GenerateAuthzHeader(FRIENDS_URL, "GET");
            var request = (HttpWebRequest)WebRequest.Create(FRIENDS_URL);
            setUpHeaders(request, authzHeader);

            List<string> toReturn = new List<string>();

            using (var response = (HttpWebResponse)request.GetResponse())
            {
                //TODO do better error catching
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    Debug.Log("There's been a problem trying to access fitbit:" +
                                    Environment.NewLine +
                                    response.StatusDescription);
                }
                else
                {
                    string line = getStringFromResponse(response);
                    JSONObject list = new JSONObject(line);
                    list.GetField("friends", delegate(JSONObject hits)
                    {
                        foreach (JSONObject user in hits.list)
                        {
                            user.GetField("user", delegate(JSONObject info)
                            {
                                //TODO extract more info here if we want
                                info.GetField("encodedId", delegate(JSONObject encodedId)
                                {
                                    toReturn.Add(encodedId.ToString());
                                });
                            });
                        }
                    });
                }
                // Example for someone with no friends:
                //{
                //"friends":  []
                //}
            }
            return toReturn;
        }

        /**
        * Gets the number of steps uploaded to fitbit since the last time this was called
        * 
        * */
        public int getStepsSinceLastCall()
        {
            int steps = 0;
            var authzHeader = manager.GenerateAuthzHeader(LAST_CALL_SINCE_URL, "GET");
            var request = (HttpWebRequest)WebRequest.Create(LAST_CALL_SINCE_URL);
            setUpHeaders(request, authzHeader);

            using (var response = (HttpWebResponse)request.GetResponse())
            {
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    Debug.Log("There's been a problem trying to access fitbit:" +
                                    Environment.NewLine +
                                    response.StatusDescription);
                }
                else
                {
                    string line = getStringFromResponse(response);
                    //TODO check time
                    PlayerPrefs.SetString(LAST_UPDATED_KEY, System.DateTime.Now.ToString());

                    string[] list = line.Split(new char[] { '[', '{', '}' });
                    //ignore first and last
                    //TODO ignore dateTimes since last updated
                    for (int i = 1; i < list.Length - 1; i++)
                    {
                        if (!list[i].StartsWith("\"dateTime"))
                        {
                            continue;
                        }
                        string[] itemInfo = list[i].Split(new char[] { '\"' });
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
                }
            }

            return steps;
        }

        private void setUpHeaders(HttpWebRequest request, string authzHeader){
            request.Method = "GET";
            request.PreAuthenticate = true;
            request.AllowWriteStreamBuffering = true;
            request.Headers.Add("Authorization", authzHeader);
        }

        private string getStringFromResponse(HttpWebResponse response)
        {
            string line = "";
            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                while (!reader.EndOfStream)
                {
                    line = reader.ReadLine();
                }
            }
            return line;
        }
    }
}
