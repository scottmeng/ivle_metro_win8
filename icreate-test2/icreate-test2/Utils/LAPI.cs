// LAPI.cs
// Store domain URL, token and API key
// for National University of Singapore
// IVLE services
// Created by Kaizhi Meng on 10 June 2013
// Last edited on 10 June 2013

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace icreate_test2.Utils
{
    class LAPI
    {
        private static String key = "lAY3TAAcAGYcokEEqKNCt";
        private static String domain = "https://ivle.nus.edu.sg/";

        // get the request url with API item and parameters
        public static string GenerateURL(string item, Dictionary<String,String> parameters)
        {
            string url = domain + "api/Lapi.svc/" + item + "?APIKey=" + key + "&Token=" + Utils.TokenManager.GetTokenValue();

            foreach (KeyValuePair<String, String> parameter in parameters)
            {
                url += "&" + parameter.Key+ "=" + parameter.Value;
            }

            return url;
        }

        // return the url for token achievement
        public static string GeneratePostString(string userid, string password, string domain)
        {
            string url = "APIKey=" + key + "&UserID=" + userid + "&Password=" + password + "&Domain=" + domain;
            
            return url;
        }

        // generate the url for downloading files
        public static string GenerateDownloadURL(string id)
        {
            string url = domain + "api/downloadfile.ashx?APIKey=" + key + "&AuthToken=" + Utils.TokenManager.GetTokenValue() + "&ID=" + id + "&target=workbin";

            return url;
        }
    }
}
