﻿using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using StoryBuilder.Models;



namespace StoryBuilder.Services.Keys
{

    public class Doppler
    {
        [JsonPropertyName("APIKEY")]
        public string APIKEY { get; set; }

        [JsonPropertyName("LOGID")]
        public string LOGID { get; set; }

        private static HttpClient client = new HttpClient();

        /// <summary>
        /// Obtain tokens for elmah.io logging, if they exist.
        /// Based on https://docs.doppler.com/docs/asp-net-core-csharp
        /// </summary>
        /// <returns>elmah.io tokens, or empty strings</returns>
        public async Task<Doppler> FetchSecretsAsync()
        {
            var basicAuthHeaderValue = Convert.ToBase64String(Encoding.Default.GetBytes(GlobalData.DopplerToken + ":"));

            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", basicAuthHeaderValue);
            var streamTask = client.GetStreamAsync("https://api.doppler.com/v3/configs/config/secrets/download?format=json");
            var secrets = await JsonSerializer.DeserializeAsync<Doppler>(await streamTask);
            
            return secrets;
        }

    }
}
