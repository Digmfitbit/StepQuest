﻿using System;
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

namespace Assets.Scripts.networking
{
    class DatabaseController
    {
        private static string BASE_URL = "http://www.cs.drexel.edu/~jgm55/fitbit/";
        private static string UPDATE_URL = BASE_URL + "updateUser.php";
        private static string GET_FRIENDS = BASE_URL + "getFriends.php";

        /**
         * Sends player stats to the server for storing
         * POST to update
         * */
        public static void updatePlayer(FriendModel player, playerStats stats){
            Debug.Log("Updating player");
            Thread oThread = new Thread(new ThreadStart(() =>
            {
                Debug.Log("Starting thread");
                //Serialize data to string
                string serializedStats = serializeDataToString(stats);
                Debug.Log("stats: " + serializedStats);
                
                //Add info to postData
                Debug.Log("encodedId: " + player.encodedId);
                var queryParam = "?id=" + player.encodedId.Substring(1,6);
                queryParam += "&stats=" + WWW.EscapeURL(serializedStats);

                var request = (HttpWebRequest)WebRequest.Create(UPDATE_URL + queryParam);
                setUpHeaders(request);
                
                Debug.Log(UPDATE_URL + queryParam);

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
                        Debug.Log("Updated Successfully: "+Utilities.getStringFromResponse(response));
                    }
                }
            }));
            oThread.Start();
        }
        /**
         * Gets the game data for the given friend ids
         * */
        public static List<playerStats> getFriends(List<string> friendIds)
        {
            return null;
        }

        private static string serializeDataToString(JSONable objectToSerialize){
            return objectToSerialize.getJSON().Print();
            /*IFormatter formatter = new BinaryFormatter();
            Stream stream = new MemoryStream();
            formatter.Serialize(stream, objectToSerialize);
            StreamReader sr = new StreamReader(stream);
            string serializedString = sr.ReadToEnd();
            return serializedString;*/
        }

        /**
         * Sets up GET headers for the calls in this function
         * */
        private static void setUpHeaders(HttpWebRequest request)
        {
            request.Method = "GET";
            request.ContentType = "application/x-www-form-urlencoded";
        }
    }
}