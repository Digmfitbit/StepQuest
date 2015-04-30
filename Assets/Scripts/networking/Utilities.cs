using System;
using System.Collections.Generic;
using System.Net;

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
                while (!reader.EndOfStream)
                {
                    line = reader.ReadLine();
                }
            }
            return line;
        }
    }
}
