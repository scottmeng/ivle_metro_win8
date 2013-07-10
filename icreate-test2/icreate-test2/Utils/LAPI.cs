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

        // get the request url for token validation
        public static string GenerateValidateURL()
        {
            string url = domain + "api/Lapi.svc/Validate?APIKey=" + key + "&Token=" + Utils.TokenManager.GetTokenValue();

            return url;
        }

        // get the request url with API item and parameters
        public static string GenerateGetURL(string item, Dictionary<string, string> parameters)
        {
            string url = domain + "api/Lapi.svc/" + item + "?APIKey=" + key + "&AuthToken=" + Utils.TokenManager.GetTokenValue();

            foreach (KeyValuePair<String, String> parameter in parameters)
            {
                url += "&" + parameter.Key+ "=" + parameter.Value;
            }

            return url;
        }

        public static string GeneratePostURL(string item)
        {
            string url = domain + "api/Lapi.svc/" + item;

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

        public static string GenerateJSONString(Dictionary<string, string> parameters)
        {
            string jsonString = "{\"APIKey\":\"" + key + "\",\"AuthToken\":\"" + Utils.TokenManager.GetTokenValue() + "\"";

            foreach (KeyValuePair<String, String> parameter in parameters)
            {
                jsonString += ",\"" + parameter.Key + "\":\"" + parameter.Value + "\"";
            }

            jsonString += "}";

            return jsonString;
        }
    }
}
