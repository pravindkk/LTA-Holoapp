using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Net.Http;
using System;
using UnityEngine.Serialization;
using Newtonsoft.Json;


namespace Microsoft.MixedReality.OpenXR.BasicSample
{
    
    public class Graph : MonoBehaviour
    {
        [Serializable]
        public class AzureADToken
        {
            public string token_type;
            public int expires_in;
            public int ext_expires_in;
            public string access_token;
        }
        [Serializable]
        public class Rootobject
        {
            [JsonProperty("@odata.context")]
            public string odatacontext;
            [JsonProperty("@microsoft.graph.downloadUrl")]
            public string microsoftgraphdownloadUrl;
        }






        // Start is called before the first frame update
        void Start()
        {
            Auth();
        }

        async void Auth()
        {
            var clientId = "fc474e3f-1a2c-4358-9ef3-e8df4ea90376";
            var secret = "Lw68Q~uQnbY6hn5t646JxfgxAkELI4Z2a~vyqcqx";
            var grant_type = "client_credentials";
            var redirect_uri = "https://localhost";
            var scope = "https://graph.microsoft.com/.default";
            var requestUrl = "https://login.microsoftonline.com/32355549-a4c1-4d91-9b75-d3b39523f335/oauth2/v2.0/token";

            var httpClient = new HttpClient();
            var dict = new Dictionary<string, string>
            {
                { "grant_type", grant_type },
                { "client_id", clientId },
                { "client_secret", secret },
                { "redirect_uri", redirect_uri },
                { "scope", scope }
            };

            var requestBody = new FormUrlEncodedContent(dict);
            var response = await httpClient.PostAsync(requestUrl, requestBody);

            response.EnsureSuccessStatusCode();
            Debug.Log(response.EnsureSuccessStatusCode());

            var responseContent = await response.Content.ReadAsStringAsync();
            Debug.Log(responseContent);
            AzureADToken aadToken = JsonUtility.FromJson<AzureADToken>(responseContent);
            string toReturn = aadToken.access_token;
            Debug.Log(toReturn);
            GetURL(toReturn);



        }

        async void GetURL(string accessToken)
        {
            string url = "https://graph.microsoft.com/v1.0/users/ltaholo@ltaholotestoutlook.onmicrosoft.com/drive/root:/checklists.zip:?select=content.downloadUrl";
            var client = new HttpClient();
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            var responseMessage = await client.GetAsync(url);
            responseMessage.EnsureSuccessStatusCode();
            Debug.Log(responseMessage.EnsureSuccessStatusCode());

            var responseContent = await responseMessage.Content.ReadAsStringAsync();
            Debug.Log(responseContent);
            Rootobject aadToken = JsonConvert.DeserializeObject<Rootobject>(responseContent);
            string toReturn = aadToken.microsoftgraphdownloadUrl;
            Debug.Log(toReturn);
        }


    }
}
