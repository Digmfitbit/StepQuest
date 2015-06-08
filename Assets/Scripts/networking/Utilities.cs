using System;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

using System.IO;

namespace Assets.Scripts.networking
{
    class Utilities
    {
        public static string getStringFromResponse(HttpWebResponse response)
        {
            string line = "";
            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                line = reader.ReadToEnd();
            }
            return line;
        }

        /**
         * Helper to convert A string to a DateTime. Returns Datetime of string
         * */
        public static DateTime ConvertToDateTime(string value)
        {
            value = value.Trim(new char[] { '\"' });
            
            DateTime convertedDate = new DateTime();
            try
            {
                convertedDate = Convert.ToDateTime(value);
                //Debug.Log(value + " converts to "+convertedDate+" "+convertedDate.Kind.ToString()+" time.");
            }
            catch (FormatException)
            {
                Debug.Log(value+" is not in the proper format.");
            }
            return convertedDate;
        }

    }
}
