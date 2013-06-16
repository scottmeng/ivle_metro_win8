// TokenManager.cs
// class that holds a private instance of 
// web request token and offers interfaces for 
// retrieving and modifying the token
// Created by Kaizhi Meng on 14th June 2013
// Last edited on 14th June 2013

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

using Newtonsoft.Json;

namespace icreate_test2.Utils
{
    static class TokenManager
    {
        private static DataStructure.Token _token;

        public static void UpdateToken(DataStructure.Token token)
        {
            _token = token;
            StoreToken();
        }

        public static string GetTokenValue()
        {
            return _token.TokenContent;
        }

        // to check if token has been stored in application data
        // if so, restore the token from stored data, else, return false
        public static bool IsTokenExisting()
        {
            Windows.Storage.ApplicationDataContainer roamingSettings = Windows.Storage.ApplicationData.Current.RoamingSettings;

            if (roamingSettings.Values.ContainsKey(DataStructure.ConstName.Token))
            {
                // load token from application data
                _token = new DataStructure.Token(roamingSettings.Values[DataStructure.ConstName.Token].ToString());
                return true;
            }
            else
            {
                return false;
            }
        }

        // asynchronous function to validate token, to 
        // update token if necessary or to return false
        // if token is invalid
        public static async Task<bool> IsTokenValid()
        {
            HttpClient client = new HttpClient();

            // http get request to validate token
            HttpResponseMessage response = await client.GetAsync(Utils.LAPI.GenerateURL("Validate", new Dictionary<string, string>()));

            // make sure the http reponse is successful
            response.EnsureSuccessStatusCode();

            // convert http response to string
            string responseString = await response.Content.ReadAsStringAsync();

            DataStructure.Token token = JsonConvert.DeserializeObject<DataStructure.Token>(responseString);

            if (token.TokenSuccess)
            {
                UpdateToken(token);
            }

            response.Dispose();
            client.Dispose();

            return token.TokenSuccess;
        }
        
        // to store token in application data settings
        public static void StoreToken()
        {
            Windows.Storage.ApplicationDataContainer roamingSettings = Windows.Storage.ApplicationData.Current.RoamingSettings;

            roamingSettings.Values[DataStructure.ConstName.Token] = _token.TokenContent;
        }
    }
}
