using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using UnityEngine;
using ResponseObjects;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.IO;
using Assets.Scripts.networking;
using System.Net.Security;
using System.Net.Cache;
using System.Collections.Specialized;

namespace Assets.Scripts.networking
{
    class DatabaseController
    {
        private static string BASE_URL = "http://www.cs.drexel.edu/~jgm55/fitbit/";
        private static string UPDATE_URL = BASE_URL + "updateUser.php";
        private static string GET_FRIENDS = BASE_URL + "fetchUsers.php";
        private static string CLEAR_RECORD = BASE_URL + "clearRecord.php";
        private static string GET_RECORD = BASE_URL + "getRecord.php";

        /**
         * Sends player stats to the server for storing
         * GET to update TODO should be POST
         * */
        public static void updatePlayer(PlayerStats stats){
            Debug.Log("Updating player");
            if (stats == null)
            {
                Debug.Log("Player is null returning");
                return;
            }
            Thread oThread = new Thread(new ThreadStart(() =>
            {
                Debug.Log("Starting thread");
                Thread.Sleep(2);
                //Serialize data to string
                string serializedStats = serializeDataToString(stats);
                
                //Add info to postData
                var queryParam = "?id=" + stats.id;//.Substring(1, 6);
                queryParam += "&stats=" + WWW.EscapeURL(serializedStats);
                var request = (HttpWebRequest)WebRequest.Create(UPDATE_URL + queryParam);
                setUpHeaders(request);
                Debug.Log("Calling " + UPDATE_URL + queryParam);
                ServicePointManager.ServerCertificateValidationCallback +=
                    new RemoteCertificateValidationCallback(
                        (sender, certificate, chain, policyErrors) => { return true; });
                HttpWebResponse response;
                try
                {
                    response = (HttpWebResponse)request.GetResponse();
                }
                catch (Exception e)
                {
                    Debug.Log("Exception in updatePlayer(): "+e);
                    return;
                }
                using (response)
                {
                    //TODO do better error catching
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        Debug.Log("There's been a problem trying to access the database:" +
                                    Environment.NewLine +
                                    response.StatusDescription);
                    }
                    else
                    {
                        Debug.Log("Updated Successfully: "+Utilities.getStringFromResponse(response));
                    }
                }
            }));
            oThread.Start();
        }

        public static void clearRecord(string id)
        {
            Debug.Log("Clearing record for user: " + id);
            Thread oThread = new Thread(new ThreadStart(() =>
            {
                Thread.Sleep(2);
                HttpWebResponse response;

                try
                {
                    var queryParam = "?id=" + WWW.EscapeURL(id);
                    
                    var request = (HttpWebRequest)WebRequest.Create(CLEAR_RECORD + queryParam);
                    setUpHeaders(request);

                    ServicePointManager.ServerCertificateValidationCallback +=
                        new RemoteCertificateValidationCallback(
                            (sender, certificate, chain, policyErrors) => { return true; });
                    response = (HttpWebResponse)request.GetResponse();

                    using (response)
                    {
                        //TODO do better error catching
                        if (response.StatusCode != HttpStatusCode.OK)
                        {
                            Debug.Log("There's been a problem trying to access the database:" +
                                        Environment.NewLine +
                                        response.StatusDescription);
                        }
                        else
                        {
                            string line = Utilities.getStringFromResponse(response);
                            Debug.Log(line);
                        }
                    }
                }
                catch (Exception e)
                {
                    Debug.Log("Exception in clearRecord(): " + e);
                    return;
                }
            }));
            oThread.Start();
        }

        /**
         * Gets the main player model from the database
         * Sets it in the PlayerManager
         * */
        public static void getMainPlayer(string id)
        {
            Debug.Log("Getting record for user: " + id);
            Thread oThread = new Thread(new ThreadStart(() =>
            {
                Thread.Sleep(2);
                HttpWebResponse response;

                try
                {
                    var queryParam = "?id=" + WWW.EscapeURL(id);

                    var request = (HttpWebRequest)WebRequest.Create(GET_RECORD + queryParam);
                    setUpHeaders(request);
                    Debug.Log("URL: " + GET_RECORD + queryParam);
                    ServicePointManager.ServerCertificateValidationCallback +=
                        new RemoteCertificateValidationCallback(
                            (sender, certificate, chain, policyErrors) => { return true; });
                    response = (HttpWebResponse)request.GetResponse();

                    using (response)
                    {
                        //TODO do better error catching
                        if (response.StatusCode != HttpStatusCode.OK)
                        {
                            Debug.Log("There's been a problem trying to access the database:" +
                                        Environment.NewLine +
                                        response.StatusDescription);
                        }
                        else
                        {
                            string line = Utilities.getStringFromResponse(response);
                            JSONObject obj = new JSONObject(line);
                            PlayerStats playerStats = new PlayerStats(obj);
                            Debug.Log("ADDING PLAYER: " + playerStats);
                            //Add directly to the scene
                            PlayerManager.mainPlayer = playerStats;
                        }
                    }
                }
                catch (Exception e)
                {
                    Debug.Log("Exception in getRecord(): " + e);
                    return;
                }
            }));
            oThread.Start();
        }

        /**
         * Updates the FriendsLst in the background. 
         * Takes a list of the Friend Ids from fitbit
         * Sets the friends in PLayerManager
         * */
        public static void updateFriendsList(List<string> friendIds)
        {
            if (friendIds.Capacity == 0)
            {
                Debug.Log("You have no friends :(");
                return;
            }
            Debug.Log("Getting Friend Stats");
            List<PlayerStats> friendsList = new List<PlayerStats>(0);
            Thread oThread = new Thread(new ThreadStart(() =>
            {
                Thread.Sleep(2);
                HttpWebResponse response;

                try
                {
                    //StreamWriter dataStream = new StreamWriter(request.GetRequestStream());
                    var queryParam = "?a=a";
                    foreach (string friendId in friendIds)
                    {
                        //dataStream.Write("friendId[]="+ friendId);
                        queryParam += "&friendId[]=" + WWW.EscapeURL(friendId);
                    }
                    string url = GET_FRIENDS + WWW.EscapeURL(queryParam);
                    var request = (HttpWebRequest)WebRequest.Create(url);
                    setUpHeaders(request);
                    Debug.Log("URL: " + url);
                    ServicePointManager.ServerCertificateValidationCallback +=
                        new RemoteCertificateValidationCallback(
                            (sender, certificate, chain, policyErrors) => { return true; });
                    response = (HttpWebResponse)request.GetResponse();
                    
                    using (response)
                    {
                        //TODO do better error catching
                        if (response.StatusCode != HttpStatusCode.OK)
                        {
                            Debug.Log("There's been a problem trying to access the database:" +
                                        Environment.NewLine +
                                        response.StatusDescription);
                        }
                        else
                        {
                            string line = Utilities.getStringFromResponse(response);
                            JSONObject lineObj = new JSONObject(line);

                            lineObj.GetField("friends", delegate(JSONObject idList)
                            {
                                foreach (JSONObject obj in idList.list)
                                {
                                    PlayerStats playerStats = new PlayerStats(obj);
                                    if (playerStats.id != "")
                                    {
                                        friendsList.Add(playerStats);
                                        Debug.Log("ADDING FRIEND: " + playerStats);
                                    }
                                }
                            });
                        }
                        PlayerManager.fitBitFriends = friendsList;
                    }
                }
                catch (Exception e)
                {
                    Debug.Log("Exception in updateFriendsList(): " + e);
                    return;
                }
            }));
            oThread.Start();
        }

        private static string serializeDataToString(JSONable objectToSerialize){
            return objectToSerialize.getJSON().Print();
        }

        public static void clearCache(){

        }

        /**
         * Sets up GET headers for the calls in this function
         * */
        private static void setUpHeaders(HttpWebRequest request)
        {
            request.Method = "GET";
            request.Accept = "*/*";
            //request.ContentType = "application/x-www-form-urlencoded";
            request.AllowAutoRedirect = true;
            request.MaximumAutomaticRedirections = 3;
            request.CachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.NoCacheNoStore);
        }
    }
}
