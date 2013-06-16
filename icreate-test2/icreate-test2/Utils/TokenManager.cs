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

        // to check if the current token is still valid
        // returns true if token is valid
        // returns false if token has expired
        public static bool IsTokenValid()
        {
            return false;
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
        
        // to store token in application data settings
        public static void StoreToken()
        {
            Windows.Storage.ApplicationDataContainer roamingSettings = Windows.Storage.ApplicationData.Current.RoamingSettings;

            roamingSettings.Values[DataStructure.ConstName.Token] = _token.TokenContent;
        }
    }
}
