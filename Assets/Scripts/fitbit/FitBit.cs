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
        private string LAST_CALL_SINCE_URL = API_BASE + "/1/user/-/activities/steps/date/today/1d.json";
        private string FRIENDS_URL = API_BASE + "/1/user/-/friends.json";
        private string PROFILE_URL = API_BASE + "/1/user/-/profile.json";

        //STUFF FOR KEYS For PlayerPrefs
        public static string TOKEN_KEY = "token";
        public static string TOKEN_SECRET_KEY = "token_secret";
        public static string TIME_UPDATED_STEPS_KEY = "timeUpdated";
        public static string TIME_UPDATED_PROFILE_KEY = "timeUpdatedProfile";
        public static string USER_MODEL_KEY = "USER_MODEL";
        public static string USER_FRIENDS_KEY = "USER_FRIENDS";

        //Key Stuff
        private static string CONSUMER_KEY = "09c24eab9e15ab8ba06114c374c3f9a0";
        //old: "32f9320af9f1c74d9abae8c2eeb01fce";
        private static string CONSUMER_SECRET = "442c84bc3fd739e39ed6d3c161261d1f";
        //old: "85d48cce8a2cfef8173993ef027d4000";
        private static string RequestTokenURL = "https://api.fitbit.com/oauth/request_token";
        private static string AccessTokenURL = "https://api.fitbit.com/oauth/access_token";
        private static string authorizeURL = "https://www.fitbit.com/oauth/authorize";
        private static string SERVICE_SPECIFIC_AUTHORIZE_URL_STUB = authorizeURL + "?oauth_token=";

        //Important variables
        private static FitBit instance = null;
        private static OAuth.Manager manager;

        private static bool authenticated;

        bool isAuthenticating;

        //Objects that are updated from the threads
        static int steps = 0;
        static List<string> friends = new List<string>();
        static FriendModel userModel;

        //Random other constants
        bool gotURL = false;
        bool openedURL = false;
        private string pin;

        private static bool shouldUpdate = false;
        private static bool updateUserModel = false;
        private static bool updateUserFriends = false;

        private static DateTime lastUpdatedStepTime;
        private static DateTime lastUpdatedProfileTime;

        private static float multiplier = 1f;
        private static string MULTIPLIER_KEY = "MULTIPLIER";
        private static float MULTIPLY_DAILY_ADDITION = 0.1f;
        private static float MAX_MULTIPLIER = 2f;
        private static int MINUTES_OF_STALE_DATA = 30;

        private static char DELIMITER = '\t';

        /**
         * Call from update loop in every scene.
         * At least in first scene
         * */
        public void Update()
        {
            if (DateTime.MinValue == lastUpdatedProfileTime || DateTime.Now - lastUpdatedProfileTime > TimeSpan.FromMinutes(MINUTES_OF_STALE_DATA))
            {
                shouldUpdate = true;
            }

            if (gotURL && !openedURL)
            {
                Debug.Log("Opening URL");
                openedURL = true;
                var url = SERVICE_SPECIFIC_AUTHORIZE_URL_STUB + manager[TOKEN_KEY];
                Application.OpenURL(url);
            }
            if (authenticated)
            {
                PlayerPrefs.SetString(TOKEN_KEY, manager[TOKEN_KEY]);
                PlayerPrefs.SetString(TOKEN_SECRET_KEY, manager[TOKEN_SECRET_KEY]);
                if (shouldUpdate)
                {
                    updateAll();
                }
            }

            if (updateUserModel)
            {
                PlayerPrefs.SetString(USER_MODEL_KEY, userModel.ToString());
                updateUserModel = false;
            }
            if (updateUserFriends)
            {
                string friendsString = "";
                foreach (string model in friends)
                {
                    friendsString += model + DELIMITER;
                }
                PlayerPrefs.SetString(USER_FRIENDS_KEY, friendsString);
                updateUserFriends = false;
            }
        }

        public void updateAll()
        {
            Debug.Log("updating Fitbit");
            getUpdatedSteps();
            getFriends();
            getProfileInfo();

            shouldUpdate = false ;
            lastUpdatedProfileTime = DateTime.Now;
            PlayerPrefs.SetString(TIME_UPDATED_PROFILE_KEY, lastUpdatedProfileTime.ToString());
        }

        private FitBit()
        {
            Debug.Log("Making new Fitbit");
            manager = new OAuth.Manager();
            manager["consumer_key"] = CONSUMER_KEY;
            manager["consumer_secret"] = CONSUMER_SECRET;
            authenticated = false;
            isAuthenticating = false;
            string access_token;
            string access_secret;

            //if stored.
            if ((access_token = PlayerPrefs.GetString(TOKEN_KEY)) != "")
            {
                authenticated = true;
                access_secret = PlayerPrefs.GetString(TOKEN_SECRET_KEY);

                manager[TOKEN_KEY] = access_token;
                manager[TOKEN_SECRET_KEY] = access_secret;
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
            try
            {
                Debug.Log("Fetching token");
                Thread.Sleep(1000);
                Debug.Log(manager.AcquireRequestToken(RequestTokenURL, "POST").AllText);
                Debug.Log("token: " + manager[TOKEN_KEY]);
                gotURL = true;
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }

        public static FitBit getInstance()
        {
            if(instance == null){
                instance = new FitBit();
                multiplier = PlayerPrefs.GetFloat(MULTIPLIER_KEY, 1f);

                DateTime minTime = DateTime.MinValue;
                lastUpdatedStepTime = Convert.ToDateTime(PlayerPrefs.GetString(TIME_UPDATED_STEPS_KEY, minTime.ToString()));
                lastUpdatedProfileTime = Convert.ToDateTime(PlayerPrefs.GetString(TIME_UPDATED_PROFILE_KEY, minTime.ToString()));
                if (minTime == lastUpdatedProfileTime)
                {// Set to the min value if we do not have the time
                    multiplier = 1f;// reset multiplier
                }//TODO make this work for leap years
                else if (lastUpdatedProfileTime.DayOfYear == DateTime.Now.DayOfYear - 1)
                {
                    multiplier += MULTIPLY_DAILY_ADDITION;
                    multiplier = Mathf.Min(multiplier, MAX_MULTIPLIER);
                    PlayerPrefs.SetFloat(MULTIPLIER_KEY, multiplier);
                    Debug.Log("multiplier updated: " + multiplier);
                }
                else if (lastUpdatedProfileTime.DayOfYear != DateTime.Now.DayOfYear)
                {
                    multiplier = 1f;
                }

                //Load from PlayerPrefs the userMOdel
                string userModelString = PlayerPrefs.GetString(USER_MODEL_KEY, "");
                if (userModelString != "")
                {
                    userModel = new FriendModel(new JSONObject(userModelString));
                }

                string friendsString = PlayerPrefs.GetString(USER_FRIENDS_KEY, "");
                if (friendsString!="")
                {
                    friends = new List<string>();
                    string[] friendsSplit = friendsString.Split(DELIMITER);
                    foreach (string s in friendsSplit)
                    {
                        friends.Add(s);
                    }
                }
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

        /**
         * Clears the cache
         * */
        public void clearCache()
        {
            PlayerPrefs.DeleteAll();

            if (userModel != null)
            {
                DatabaseController.clearRecord(userModel.encodedId);
            }

            manager[TOKEN_KEY] = "";
            manager[TOKEN_SECRET_KEY] = "";
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
            /*List<string> toReturn = new List<string>();
            foreach (FriendModel model in friends)
            {
                toReturn.Add(model.encodedId);
                Debug.Log("friendModel"+model);
            }*/
            return friends;
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
            Debug.Log("Getting Profile Info");
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
                            user.GetField("user", delegate(JSONObject info)
                            {
                                Debug.Log("USER FRIEND MODEL " + info);
                                userModel = new FriendModel(info);
                                updateUserModel = true;
                            });
                        }
                    }
                }
                catch (Exception e)
                {
                    Debug.Log("Exception in getProfileInfo(): "+e);
                    Thread.Sleep(1000);
                    getProfileInfo();
                }
            }));
            oThread.Start();
        }

        void getFriends()
        {
            Thread oThread = new Thread(new ThreadStart(() =>
            {
                var authzHeader = manager.GenerateAuthzHeader(FRIENDS_URL, "GET");
                var request = (HttpWebRequest)WebRequest.Create(FRIENDS_URL);
                setUpHeaders(request, authzHeader);

                HttpWebResponse response;
                try
                {
                    response = (HttpWebResponse)request.GetResponse();
                }
                catch (Exception e)
                {
                    Debug.Log("Exception in getFriends(): " + e);
                    Thread.Sleep(1000);
                    getFriends();
                    return;
                }
                using (response)
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
                                    friends.Add(model.encodedId);
                                    Debug.Log("Adding friend: " + model);
                                    updateUserFriends = true;
                                });
                            }
                        });
                    }
                    // Example for someone with no friends:
                    //{
                    //"friends":  []
                    //}
                }
            }));
            oThread.Start();
        }

        /**
        * Gets the number of steps since the last time this was called
        * Refreshes the information if it is stale.
        * Call from game loop for best results.
        * */
        public int getStepsSinceLastCall()
        {
            int temp = steps;
            steps = 0;
            PlayerPrefs.SetString(TIME_UPDATED_STEPS_KEY, lastUpdatedStepTime.ToString());
            return temp;
        }

        /**
        * Gets the number of steps uploaded to fitbit since the last time this was called
        * 
        * */
        public void getUpdatedSteps()
        {
            
            Thread oThread = new Thread(new ThreadStart(() =>
            {
                var authzHeader = manager.GenerateAuthzHeader(LAST_CALL_SINCE_URL, "GET");
                var request = (HttpWebRequest)WebRequest.Create(LAST_CALL_SINCE_URL);
                setUpHeaders(request, authzHeader);
                Debug.Log("updating Steps");
                HttpWebResponse response;
                try
                {
                    response = (HttpWebResponse)request.GetResponse();
                }
                catch (Exception e)
                {
                    Debug.Log("Exception in getUpdatedSteps(): " + e);
                    Thread.Sleep(1000);
                    getUpdatedSteps();
                    return;
                }
                using (response)
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
                        DateTime dateTime = lastUpdatedStepTime;
                        JSONObject list = new JSONObject(line);
                        DateTime day = new DateTime();
                        list.GetField("activities-steps", delegate(JSONObject hits)
                        {
                            foreach (JSONObject hit in hits.list)
                            {
                                hit.GetField("dateTime", delegate(JSONObject date)
                                {
                                    day = Utilities.ConvertToDateTime(date.ToString());
                                });
                            }
                            
                        });
                        list.GetField("activities-steps-intraday", delegate(JSONObject hits1)
                        {
                            hits1.GetField("dataset", delegate(JSONObject hits2)
                            {
                                foreach (JSONObject timeObj in hits2.list)
                                {
                                    timeObj.GetField("time", delegate(JSONObject time)
                                    {
                                        DateTime hoursMinutes = Utilities.ConvertToDateTime(time.ToString());
                                        //TODO CHECK THE TIME;
                                        dateTime = new DateTime(day.Year, day.Month, day.Day,hoursMinutes.Hour,
                                            hoursMinutes.Minute,hoursMinutes.Second);

                                    });
                                    timeObj.GetField("value", delegate(JSONObject val)
                                    {
                                        if (dateTime > lastUpdatedStepTime)
                                        {
                                            steps += (int)(multiplier * Convert.ToInt32(val));
                                        }
                                    });
                                }
                            });
                        });
                        Debug.Log("steps: " + steps);
                        lastUpdatedStepTime = dateTime;
                    }
                }
            }));
            oThread.Start();
        }

        private void setUpHeaders(HttpWebRequest request, string authzHeader){
            request.Method = "GET";
            request.PreAuthenticate = true;
            request.AllowWriteStreamBuffering = true;
            request.Headers.Add("Authorization", authzHeader);
        }
    }
}
