using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.NetworkInformation;

using Newtonsoft.Json;        

namespace icreate_test2.Utils
{
    static class RequestSender
    {
        // asynchronous function to validate token, to 
        // update token if necessary or to return false
        // if token is invalid
        public static async Task<String> GetResponseString(string field, Dictionary<string, string> dataPairs)
        {
            HttpClient client = new HttpClient();

            // http get request to validate token
            HttpResponseMessage response = await client.GetAsync(Utils.LAPI.GenerateGetURL(field, dataPairs));

            // make sure the http reponse is successful
            response.EnsureSuccessStatusCode();

            // convert http response to string
            string responseString = await response.Content.ReadAsStringAsync();

            response.Dispose();
            client.Dispose();

            return responseString;
        }
    }
}
