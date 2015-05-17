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
        public static string TIME_UPDATED_KEY = "timeUpdated";


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
        private DateTime lastUpdatedTime;
        private bool updateTime = false;

        private Thread dispatcher;

        public void Update()
        {
            updateCounter += Time.deltaTime;
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
                if (updateCounter > UPDATE_INTERVAL)
                {
                    updateAll();
                }
            }
            if (updateTime)
            {
                updateTime = false;
                PlayerPrefs.SetString(TIME_UPDATED_KEY, lastUpdatedTime.ToString());
            }
        }

        public void updateAll()
        {
            dispatcher = Thread.CurrentThread;
            Debug.Log("updating Fitbit");
            getProfileInfo();

            DateTime now = System.DateTime.Now;
            lastUpdatedTime = Convert.ToDateTime(PlayerPrefs.GetString(TIME_UPDATED_KEY, now.ToString()));
            if (now == lastUpdatedTime)
            {// Set to the min value
                lastUpdatedTime = DateTime.MinValue;
            }
            getUpdatedSteps();
            getFriends();

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
            Debug.Log("Fetching token");
            Debug.Log(manager.AcquireRequestToken(RequestTokenURL, "POST").AllText);
            Debug.Log("token: " + manager[TOKEN_KEY]);
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

        public void clearCache()
        {
            PlayerPrefs.SetString(TOKEN_KEY, "");
            PlayerPrefs.SetString(TOKEN_SECRET_KEY, "");
            PlayerPrefs.SetString(TIME_UPDATED_KEY, "");
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
            Thread oThread = new Thread(new ThreadStart(() =>
            {
                var authzHeader = manager.GenerateAuthzHeader(FRIENDS_URL, "GET");
                var request = (HttpWebRequest)WebRequest.Create(FRIENDS_URL);
                setUpHeaders(request, authzHeader);

                List<string> toReturn = new List<string>();

                HttpWebResponse response;
                try
                {
                    response = (HttpWebResponse)request.GetResponse();
                }
                catch (Exception e)
                {
                    Debug.Log("Exception in getFriends(): " + e);
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
            }));
            oThread.Start();
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
        * 
        * */
        private void getUpdatedSteps()
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
                        DateTime dateTime = lastUpdatedTime;
                        Debug.Log(line);
                        JSONObject list = new JSONObject(line);
                        DateTime day = new DateTime();
                        list.GetField("activities-steps", delegate(JSONObject hits)
                        {
                            foreach (JSONObject hit in hits.list)
                            {
                                hit.GetField("dateTime", delegate(JSONObject date)
                                {
                                    Debug.Log(date);
                                    day = Utilities.ConvertToDateTime(date.ToString());
                                    Debug.Log(day);
                                });
                            }
                            
                        });
                        list.GetField("activities-steps-intraday", delegate(JSONObject hits1)
                        {
                            Debug.Log(hits1);
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
                                        if (dateTime > lastUpdatedTime)
                                        {
                                            steps += Convert.ToInt32(val);
                                        }
                                    });
                                }
                            });
                        });
                        Debug.Log("steps: " + steps);
                        updateTime = true;
                        lastUpdatedTime = dateTime;

                        //Example response with whitespace for clarity
                        //{"activities-steps":[
                        //  {"dateTime":"2015-04-13","value":"0"},
                        //  {"dateTime":"2015-04-14","value":"0"},
                        //  ...,
                        //  {"dateTime":"2015-04-19","value":"0"}
                        //]}


                        //{"activities-steps":[{"dateTime":"2015-05-14","value":"0"}],
                        //"activities-steps-intraday":{"dataset":[{"time":"00:00:00","value":0},
                        //{"time":"00:01:00","value":0},{"time":"00:02:00","value":0},
                        //{"time":"00:03:00","value":0},{"time":"00:04:00","value":0},{"time":"00:05:00","value":0},{"time":"00:06:00","value":0},{"time":"00:07:00","value":0},{"time":"00:08:00","value":0},{"time":"00:09:00","value":0},{"time":"00:10:00","value":0},{"time":"00:11:00","value":0},{"time":"00:12:00","value":0},{"time":"00:13:00","value":0},{"time":"00:14:00","value":0},{"time":"00:15:00","value":0},{"time":"00:16:00","value":0},{"time":"00:17:00","value":0},{"time":"00:18:00","value":0},{"time":"00:19:00","value":0},{"time":"00:20:00","value":0},{"time":"00:21:00","value":0},{"time":"00:22:00","value":0},{"time":"00:23:00","value":0},{"time":"00:24:00","value":0},{"time":"00:25:00","value":0},{"time":"00:26:00","value":0},{"time":"00:27:00","value":0},{"time":"00:28:00","value":0},{"time":"00:29:00","value":0},{"time":"00:30:00","value":0},{"time":"00:31:00","value":0},{"time":"00:32:00","value":0},{"time":"00:33:00","value":0},{"time":"00:34:00","value":0},{"time":"00:35:00","value":0},{"time":"00:36:00","value":0},{"time":"00:37:00","value":0},{"time":"00:38:00","value":0},{"time":"00:39:00","value":0},{"time":"00:40:00","value":0},{"time":"00:41:00","value":0},{"time":"00:42:00","value":0},{"time":"00:43:00","value":0},{"time":"00:44:00","value":0},{"time":"00:45:00","value":0},{"time":"00:46:00","value":0},{"time":"00:47:00","value":0},{"time":"00:48:00","value":0},{"time":"00:49:00","value":0},{"time":"00:50:00","value":0},{"time":"00:51:00","value":0},{"time":"00:52:00","value":0},{"time":"00:53:00","value":0},{"time":"00:54:00","value":0},{"time":"00:55:00","value":0},{"time":"00:56:00","value":0},{"time":"00:57:00","value":0},{"time":"00:58:00","value":0},{"time":"00:59:00","value":0},{"time":"01:00:00","value":0},{"time":"01:01:00","value":0},{"time":"01:02:00","value":0},{"time":"01:03:00","value":0},{"time":"01:04:00","value":0},{"time":"01:05:00","value":0},{"time":"01:06:00","value":0},{"time":"01:07:00","value":0},{"time":"01:08:00","value":0},{"time":"01:09:00","value":0},{"time":"01:10:00","value":0},{"time":"01:11:00","value":0},{"time":"01:12:00","value":0},{"time":"01:13:00","value":0},{"time":"01:14:00","value":0},{"time":"01:15:00","value":0},{"time":"01:16:00","value":0},{"time":"01:17:00","value":0},{"time":"01:18:00","value":0},{"time":"01:19:00","value":0},{"time":"01:20:00","value":0},{"time":"01:21:00","value":0},{"time":"01:22:00","value":0},{"time":"01:23:00","value":0},{"time":"01:24:00","value":0},{"time":"01:25:00","value":0},{"time":"01:26:00","value":0},{"time":"01:27:00","value":0},{"time":"01:28:00","value":0},{"time":"01:29:00","value":0},{"time":"01:30:00","value":0},{"time":"01:31:00","value":0},{"time":"01:32:00","value":0},{"time":"01:33:00","value":0},{"time":"01:34:00","value":0},{"time":"01:35:00","value":0},{"time":"01:36:00","value":0},{"time":"01:37:00","value":0},{"time":"01:38:00","value":0},{"time":"01:39:00","value":0},{"time":"01:40:00","value":0},{"time":"01:41:00","value":0},{"time":"01:42:00","value":0},{"time":"01:43:00","value":0},{"time":"01:44:00","value":0},{"time":"01:45:00","value":0},{"time":"01:46:00","value":0},{"time":"01:47:00","value":0},{"time":"01:48:00","value":0},{"time":"01:49:00","value":0},{"time":"01:50:00","value":0},{"time":"01:51:00","value":0},{"time":"01:52:00","value":0},{"time":"01:53:00","value":0},{"time":"01:54:00","value":0},{"time":"01:55:00","value":0},{"time":"01:56:00","value":0},{"time":"01:57:00","value":0},{"time":"01:58:00","value":0},{"time":"01:59:00","value":0},{"time":"02:00:00","value":0},{"time":"02:01:00","value":0},{"time":"02:02:00","value":0},{"time":"02:03:00","value":0},{"time":"02:04:00","value":0},{"time":"02:05:00","value":0},{"time":"02:06:00","value":0},{"time":"02:07:00","value":0},{"time":"02:08:00","value":0},{"time":"02:09:00","value":0},{"time":"02:10:00","value":0},{"time":"02:11:00","value":0},{"time":"02:12:00","value":0},{"time":"02:13:00","value":0},{"time":"02:14:00","value":0},{"time":"02:15:00","value":0},{"time":"02:16:00","value":0},{"time":"02:17:00","value":0},{"time":"02:18:00","value":0},{"time":"02:19:00","value":0},{"time":"02:20:00","value":0},{"time":"02:21:00","value":0},{"time":"02:22:00","value":0},{"time":"02:23:00","value":0},{"time":"02:24:00","value":0},{"time":"02:25:00","value":0},{"time":"02:26:00","value":0},{"time":"02:27:00","value":0},{"time":"02:28:00","value":0},{"time":"02:29:00","value":0},{"time":"02:30:00","value":0},{"time":"02:31:00","value":0},{"time":"02:32:00","value":0},{"time":"02:33:00","value":0},{"time":"02:34:00","value":0},{"time":"02:35:00","value":0},{"time":"02:36:00","value":0},{"time":"02:37:00","value":0},{"time":"02:38:00","value":0},{"time":"02:39:00","value":0},{"time":"02:40:00","value":0},{"time":"02:41:00","value":0},{"time":"02:42:00","value":0},{"time":"02:43:00","value":0},{"time":"02:44:00","value":0},{"time":"02:45:00","value":0},{"time":"02:46:00","value":0},{"time":"02:47:00","value":0},{"time":"02:48:00","value":0},{"time":"02:49:00","value":0},{"time":"02:50:00","value":0},{"time":"02:51:00","value":0},{"time":"02:52:00","value":0},{"time":"02:53:00","value":0},{"time":"02:54:00","value":0},{"time":"02:55:00","value":0},{"time":"02:56:00","value":0},{"time":"02:57:00","value":0},{"time":"02:58:00","value":0},{"time":"02:59:00","value":0},{"time":"03:00:00","value":0},{"time":"03:01:00","value":0},{"time":"03:02:00","value":0},{"time":"03:03:00","value":0},{"time":"03:04:00","value":0},{"time":"03:05:00","value":0},{"time":"03:06:00","value":0},{"time":"03:07:00","value":0},{"time":"03:08:00","value":0},{"time":"03:09:00","value":0},{"time":"03:10:00","value":0},{"time":"03:11:00","value":0},{"time":"03:12:00","value":0},{"time":"03:13:00","value":0},{"time":"03:14:00","value":0},{"time":"03:15:00","value":0},{"time":"03:16:00","value":0},{"time":"03:17:00","value":0},{"time":"03:18:00","value":0},{"time":"03:19:00","value":0},{"time":"03:20:00","value":0},{"time":"03:21:00","value":0},{"time":"03:22:00","value":0},{"time":"03:23:00","value":0},{"time":"03:24:00","value":0},{"time":"03:25:00","value":0},{"time":"03:26:00","value":0},{"time":"03:27:00","value":0},{"time":"03:28:00","value":0},{"time":"03:29:00","value":0},{"time":"03:30:00","value":0},{"time":"03:31:00","value":0},{"time":"03:32:00","value":0},{"time":"03:33:00","value":0},{"time":"03:34:00","value":0},{"time":"03:35:00","value":0},{"time":"03:36:00","value":0},{"time":"03:37:00","value":0},{"time":"03:38:00","value":0},{"time":"03:39:00","value":0},{"time":"03:40:00","value":0},{"time":"03:41:00","value":0},{"time":"03:42:00","value":0},{"time":"03:43:00","value":0},{"time":"03:44:00","value":0},{"time":"03:45:00","value":0},{"time":"03:46:00","value":0},{"time":"03:47:00","value":0},{"time":"03:48:00","value":0},{"time":"03:49:00","value":0},{"time":"03:50:00","value":0},{"time":"03:51:00","value":0},{"time":"03:52:00","value":0},{"time":"03:53:00","value":0},{"time":"03:54:00","value":0},{"time":"03:55:00","value":0},{"time":"03:56:00","value":0},{"time":"03:57:00","value":0},{"time":"03:58:00","value":0},{"time":"03:59:00","value":0},{"time":"04:00:00","value":0},{"time":"04:01:00","value":0},{"time":"04:02:00","value":0},{"time":"04:03:00","value":0},{"time":"04:04:00","value":0},{"time":"04:05:00","value":0},{"time":"04:06:00","value":0},{"time":"04:07:00","value":0},{"time":"04:08:00","value":0},{"time":"04:09:00","value":0},{"time":"04:10:00","value":0},{"time":"04:11:00","value":0},{"time":"04:12:00","value":0},{"time":"04:13:00","value":0},{"time":"04:14:00","value":0},{"time":"04:15:00","value":0},{"time":"04:16:00","value":0},{"time":"04:17:00","value":0},{"time":"04:18:00","value":0},{"time":"04:19:00","value":0},{"time":"04:20:00","value":0},{"time":"04:21:00","value":0},{"time":"04:22:00","value":0},{"time":"04:23:00","value":0},{"time":"04:24:00","value":0},{"time":"04:25:00","value":0},{"time":"04:26:00","value":0},{"time":"04:27:00","value":0},{"time":"04:28:00","value":0},{"time":"04:29:00","value":0},{"time":"04:30:00","value":0},{"time":"04:31:00","value":0},{"time":"04:32:00","value":0},{"time":"04:33:00","value":0},{"time":"04:34:00","value":0},{"time":"04:35:00","value":0},{"time":"04:36:00","value":0},{"time":"04:37:00","value":0},{"time":"04:38:00","value":0},{"time":"04:39:00","value":0},{"time":"04:40:00","value":0},{"time":"04:41:00","value":0},{"time":"04:42:00","value":0},{"time":"04:43:00","value":0},{"time":"04:44:00","value":0},{"time":"04:45:00","value":0},{"time":"04:46:00","value":0},{"time":"04:47:00","value":0},{"time":"04:48:00","value":0},{"time":"04:49:00","value":0},{"time":"04:50:00","value":0},{"time":"04:51:00","value":0},{"time":"04:52:00","value":0},{"time":"04:53:00","value":0},{"time":"04:54:00","value":0},{"time":"04:55:00","value":0},{"time":"04:56:00","value":0},{"time":"04:57:00","value":0},{"time":"04:58:00","value":0},{"time":"04:59:00","value":0},{"time":"05:00:00","value":0},{"time":"05:01:00","value":0},{"time":"05:02:00","value":0},{"time":"05:03:00","value":0},{"time":"05:04:00","value":0},{"time":"05:05:00","value":0},{"time":"05:06:00","value":0},{"time":"05:07:00","value":0},{"time":"05:08:00","value":0},{"time":"05:09:00","value":0},{"time":"05:10:00","value":0},{"time":"05:11:00","value":0},{"time":"05:12:00","value":0},{"time":"05:13:00","value":0},{"time":"05:14:00","value":0},{"time":"05:15:00","value":0},{"time":"05:16:00","value":0},{"time":"05:17:00","value":0},{"time":"05:18:00","value":0},{"time":"05:19:00","value":0},{"time":"05:20:00","value":0},{"time":"05:21:00","value":0},{"time":"05:22:00","value":0},{"time":"05:23:00","value":0},{"time":"05:24:00","value":0},{"time":"05:25:00","value":0},{"time":"05:26:00","value":0},{"time":"05:27:00","value":0},{"time":"05:28:00","value":0},{"time":"05:29:00","value":0},{"time":"05:30:00","value":0},{"time":"05:31:00","value":0},{"time":"05:32:00","value":0},{"time":"05:33:00","value":0},{"time":"05:34:00","value":0},{"time":"05:35:00","value":0},{"time":"05:36:00","value":0},{"time":"05:37:00","value":0},{"time":"05:38:00","value":0},{"time":"05:39:00","value":0},{"time":"05:40:00","value":0},{"time":"05:41:00","value":0},{"time":"05:42:00","value":0},{"time":"05:43:00","value":0},{"time":"05:44:00","value":0},{"time":"05:45:00","value":0},{"time":"05:46:00","value":0},{"time":"05:47:00","value":0},{"time":"05:48:00","value":0},{"time":"05:49:00","value":0},{"time":"05:50:00","value":0},{"time":"05:51:00","value":0},{"time":"05:52:00","value":0},{"time":"05:53:00","value":0},{"time":"05:54:00","value":0},{"time":"05:55:00","value":0},{"time":"05:56:00","value":0},{"time":"05:57:00","value":0},{"time":"05:58:00","value":0},{"time":"05:59:00","value":0},{"time":"06:00:00","value":0},{"time":"06:01:00","value":0},{"time":"06:02:00","value":0},{"time":"06:03:00","value":0},{"time":"06:04:00","value":0},{"time":"06:05:00","value":0},{"time":"06:06:00","value":0},{"time":"06:07:00","value":0},{"time":"06:08:00","value":0},{"time":"06:09:00","value":0},{"time":"06:10:00","value":0},{"time":"06:11:00","value":0},{"time":"06:12:00","value":0},{"time":"06:13:00","value":0},{"time":"06:14:00","value":0},{"time":"06:15:00","value":0},{"time":"06:16:00","value":0},{"time":"06:17:00","value":0},{"time":"06:18:00","value":0},{"time":"06:19:00","value":0},{"time":"06:20:00","value":0},{"time":"06:21:00","value":0},{"time":"06:22:00","value":0},{"time":"06:23:00","value":0},{"time":"06:24:00","value":0},{"time":"06:25:00","value":0},{"time":"06:26:00","value":0},{"time":"06:27:00","value":0},{"time":"06:28:00","value":0},{"time":"06:29:00","value":0},{"time":"06:30:00","value":0},{"time":"06:31:00","value":0},{"time":"06:32:00","value":0},{"time":"06:33:00","value":0},{"time":"06:34:00","value":0},{"time":"06:35:00","value":0},{"time":"06:36:00","value":0},{"time":"06:37:00","value":0},{"time":"06:38:00","value":0},{"time":"06:39:00","value":0},{"time":"06:40:00","value":0},{"time":"06:41:00","value":0},{"time":"06:42:00","value":0},{"time":"06:43:00","value":0},{"time":"06:44:00","value":0},{"time":"06:45:00","value":0},{"time":"06:46:00","value":0},{"time":"06:47:00","value":0},{"time":"06:48:00","value":0},{"time":"06:49:00","value":0},{"time":"06:50:00","value":0},{"time":"06:51:00","value":0},{"time":"06:52:00","value":0},{"time":"06:53:00","value":0},{"time":"06:54:00","value":0},{"time":"06:55:00","value":0},{"time":"06:56:00","value":0},{"time":"06:57:00","value":0},{"time":"06:58:00","value":0},{"time":"06:59:00","value":0},{"time":"07:00:00","value":0},{"time":"07:01:00","value":0},{"time":"07:02:00","value":0},{"time":"07:03:00","value":0},{"time":"07:04:00","value":0},{"time":"07:05:00","value":0},{"time":"07:06:00","value":0},{"time":"07:07:00","value":0},{"time":"07:08:00","value":0},{"time":"07:09:00","value":0},{"time":"07:10:00","value":0},{"time":"07:11:00","value":0},{"time":"07:12:00","value":0},{"time":"07:13:00","value":0},{"time":"07:14:00","value":0},{"time":"07:15:00","value":0},{"time":"07:16:00","value":0},{"time":"07:17:00","value":0},{"time":"07:18:00","value":0},{"time":"07:19:00","value":0},{"time":"07:20:00","value":0},{"time":"07:21:00","value":0},{"time":"07:22:00","value":0},{"time":"07:23:00","value":0},{"time":"07:24:00","value":0},{"time":"07:25:00","value":0},{"time":"07:26:00","value":0},{"time":"07:27:00","value":0},{"time":"07:28:00","value":0},{"time":"07:29:00","value":0},{"time":"07:30:00","value":0},{"time":"07:31:00","value":0},{"time":"07:32:00","value":0},{"time":"07:33:00","value":0},{"time":"07:34:00","value":0},{"time":"07:35:00","value":0},{"time":"07:36:00","value":0},{"time":"07:37:00","value":0},{"time":"07:38:00","value":0},{"time":"07:39:00","value":0},{"time":"07:40:00","value":0},{"time":"07:41:00","value":0},{"time":"07:42:00","value":0},{"time":"07:43:00","value":0},{"time":"07:44:00","value":0},{"time":"07:45:00","value":0},{"time":"07:46:00","value":0},{"time":"07:47:00","value":0},{"time":"07:48:00","value":0},{"time":"07:49:00","value":0},{"time":"07:50:00","value":0},{"time":"07:51:00","value":0},{"time":"07:52:00","value":0},{"time":"07:53:00","value":0},{"time":"07:54:00","value":0},{"time":"07:55:00","value":0},{"time":"07:56:00","value":0},{"time":"07:57:00","value":0},{"time":"07:58:00","value":0},{"time":"07:59:00","value":0},{"time":"08:00:00","value":0},{"time":"08:01:00","value":0},{"time":"08:02:00","value":0},{"time":"08:03:00","value":0},{"time":"08:04:00","value":0},{"time":"08:05:00","value":0},{"time":"08:06:00","value":0},{"time":"08:07:00","value":0},{"time":"08:08:00","value":0},{"time":"08:09:00","value":0},{"time":"08:10:00","value":0},{"time":"08:11:00","value":0},{"time":"08:12:00","value":0},{"time":"08:13:00","value":0},{"time":"08:14:00","value":0},{"time":"08:15:00","value":0},{"time":"08:16:00","value":0},{"time":"08:17:00","value":0},{"time":"08:18:00","value":0},{"time":"08:19:00","value":0},{"time":"08:20:00","value":0},{"time":"08:21:00","value":0},{"time":"08:22:00","value":0},{"time":"08:23:00","value":0},{"time":"08:24:00","value":0},{"time":"08:25:00","value":0},{"time":"08:26:00","value":0},{"time":"08:27:00","value":0},{"time":"08:28:00","value":0},{"time":"08:29:00","value":0},{"time":"08:30:00","value":0},{"time":"08:31:00","value":0},{"time":"08:32:00","value":0},{"time":"08:33:00","value":0},{"time":"08:34:00","value":0},{"time":"08:35:00","value":0},{"time":"08:36:00","value":0},{"time":"08:37:00","value":0},{"time":"08:38:00","value":0},{"time":"08:39:00","value":0},{"time":"08:40:00","value":0},{"time":"08:41:00","value":0},{"time":"08:42:00","value":0},{"time":"08:43:00","value":0},{"time":"08:44:00","value":0},{"time":"08:45:00","value":0},{"time":"08:46:00","value":0},{"time":"08:47:00","value":0},{"time":"08:48:00","value":0},{"time":"08:49:00","value":0},{"time":"08:50:00","value":0},{"time":"08:51:00","value":0},{"time":"08:52:00","value":0},{"time":"08:53:00","value":0},{"time":"08:54:00","value":0},{"time":"08:55:00","value":0},{"time":"08:56:00","value":0},{"time":"08:57:00","value":0},{"time":"08:58:00","value":0},{"time":"08:59:00","value":0},<message truncated>
                        //
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
