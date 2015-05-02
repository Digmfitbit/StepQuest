using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine.UI;
using UnityEngine;
using System.Threading;
using ResponseObjects;
using Assets.Scripts.networking;
using System.IO;

namespace Assets.Scripts.fitbit
{
    class FitBit
    {
        //URLs for endpoints
        private static string API_BASE = "https://api.fitbit.com";
        private string LAST_CALL_SINCE_URL = API_BASE + "/1/user/-/activities/steps/date/today/7d.json";
        private string FRIENDS_URL = API_BASE + "/1/user/-/friends.json";
        private string PROFILE_URL = API_BASE + "/1/user/-/profile.json";

        //Key Stuff
        private static string CONSUMER_KEY = "32f9320af9f1c74d9abae8c2eeb01fce";
        private static string CONSUMER_SECRET = "85d48cce8a2cfef8173993ef027d4000";
        private static string RequestTokenURL = "https://api.fitbit.com/oauth/request_token";
        private static string AccessTokenURL = "https://api.fitbit.com/oauth/access_token";
        private static string authorizeURL = "https://www.fitbit.com/oauth/authorize";
        private static string SERVICE_SPECIFIC_AUTHORIZE_URL_STUB = authorizeURL + "?oauth_token=";

        //Important variables
        private static FitBit instance = null;
        private static OAuth.Manager manager;

        private static bool authenticated;
        private static string access_token;
        private static string access_secret;

        bool isAuthenticating;

        //Objects that are updated from the threads
        static int steps = 0;
        static List<FriendModel> friends = new List<FriendModel>();
        static FriendModel userModel;

        //Random other constants
        private static string LAST_UPDATED_KEY = "LastUpdated";
        bool gotURL = false;
        bool openedURL = false;
        private string pin;

        private const float UPDATE_INTERVAL = 600;//update every ten minutes
        private static float updateCounter = 601f;
        public void Update()
        {
            updateCounter += Time.deltaTime;
            if (gotURL && !openedURL)
            {
                Debug.Log("Opening URL");
                openedURL = true;
                var url = SERVICE_SPECIFIC_AUTHORIZE_URL_STUB + manager["token"];
                Application.OpenURL(url);
            }
            if (authenticated)
            {
                PlayerPrefs.SetString("token", manager["token"]);
                PlayerPrefs.SetString("token_secret", manager["token_secret"]);
                if (updateCounter > UPDATE_INTERVAL)
                {
                    updateAll();
                }

            }
        }

        public void updateAll()
        {
            Thread threadFriends = new Thread(new ThreadStart(getFriends));
            Thread threadSteps = new Thread(new ThreadStart(getFriends));
            getProfileInfo();
            threadFriends.Start();
            threadSteps.Start();
            updateCounter = 0;
        }

        private FitBit()
        {
            Debug.Log("Making new Fitbit");
            manager = new OAuth.Manager();
            manager["consumer_key"] = CONSUMER_KEY;
            manager["consumer_secret"] = CONSUMER_SECRET;
            authenticated = false;
            isAuthenticating = false;
            //if stored.
            if ((access_token = PlayerPrefs.GetString("token")) != "")
            {
                authenticated = true;
                access_secret = PlayerPrefs.GetString("token_secret");

                manager["token"] = access_token;
                manager["token_secret"] = access_secret;
            }
            else
            { // Need to verify. Launch browser.
                isAuthenticating = true;
                Thread oThread = new Thread(new ThreadStart(getToken));
                // Start the thread
                oThread.Start();
            }
        }

        void getToken(){
            Debug.Log("Fetching token");
            Debug.Log(manager.AcquireRequestToken(RequestTokenURL, "POST").AllText);
            Debug.Log("token: " + manager["token"]);
            gotURL = true;
        }

        public static FitBit getInstance()
        {
            if(instance == null){
                instance = new FitBit();
            }
            return instance;
        }

        public OAuth.Manager getManager(){
            return manager;
        }

        public bool isAuthenticated()
        {
            return authenticated;
        }

        public void enterPin()
        {
            pin = GameObject.Find("PinInputField").GetComponent<InputField>().text;
            Thread oThread = new Thread(new ThreadStart(sendPin));
            // Start the thread
            oThread.Start();
        }

        /**
         * Relies on having an InputField in scene called PinInputField
         * */
        void sendPin()
        {
            
            Debug.Log("pin: " + pin);
            OAuth.OAuthResponse response = manager.AcquireAccessToken(AccessTokenURL,
                             "POST",
                             pin);
            Debug.Log(response.AllText) ;
            
            isAuthenticating = false;
            authenticated = true;

        }

        /**
         * Gets the string identifiers for the authenticated user's friends list
         * */
        public List<string> getFriendIDs()
        {
            List<string> toReturn = new List<string>();
            foreach (FriendModel model in friends)
            {
                toReturn.Add(model.encodedId);
            }
            return toReturn;
        }
        /**
         * Returns hte latest FriendModel representing the current user
         * */
        public FriendModel getUserModel()
        {
            return userModel;
        }

        void getProfileInfo()
        {
            Thread oThread = new Thread(new ThreadStart(() =>
            {
                var authzHeader = manager.GenerateAuthzHeader(PROFILE_URL, "GET");
                var request = (HttpWebRequest)WebRequest.Create(PROFILE_URL);
                setUpHeaders(request, authzHeader);
                try
                {
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
                            string line = Utilities.getStringFromResponse(response);
                            JSONObject user = new JSONObject(line);
                            Debug.Log("Fetched userModel: " + line);
                            user.GetField("user", delegate(JSONObject info)
                            {
                                userModel = new FriendModel(info);
                            });
                        }
                    }
                }
                catch (Exception e)
                {
                    Debug.Log("Exception in getProfileInfo(): "+e);
                }
            }));
            oThread.Start();
        }

        void getFriends()
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
                    string line = Utilities.getStringFromResponse(response);
                    JSONObject list = new JSONObject(line);
                    list.GetField("friends", delegate(JSONObject hits)
                    {
                        foreach (JSONObject user in hits.list)
                        {
                            user.GetField("user", delegate(JSONObject info)
                            {
                                //TODO extract more info here if we want
                                FriendModel model = new FriendModel(info);
                                friends.Add(model);
                            });
                        }
                    });
                }
                // Example for someone with no friends:
                //{
                //"friends":  []
                //}
            }
        }

        /**
        * Gets the number of steps since the last time this was called
        * 
        * */
        public int getStepsSinceLastCall()
        {
            int temp = steps;
            steps = 0;
            return temp;
        }

        /**
        * Gets the number of steps uploaded to fitbit since the last time this was called
        * DOESNT WORK YET
        * */
        private void getUpdatedSteps()
        {
            
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
                    string line = Utilities.getStringFromResponse(response);
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
        }

        private void setUpHeaders(HttpWebRequest request, string authzHeader){
            request.Method = "GET";
            request.PreAuthenticate = true;
            request.AllowWriteStreamBuffering = true;
            request.Headers.Add("Authorization", authzHeader);
        }
    }
}
