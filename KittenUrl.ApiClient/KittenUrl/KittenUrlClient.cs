using DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace KittenUrl.ApiClient.KittenUrl
{
    public class KittenUrlClient : IKittenUrlClient
    {
        public KittenUrlClient()
        {

        }

        public async Task<string> Get(string url)
        {
            KittenUrlDto result = new KittenUrlDto();
            string apiResponse;
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://localhost:44351/kittenurl/get" + url))
                {
                     apiResponse = await response.Content.ReadAsStringAsync();
                }
            }
            return apiResponse;
        }

        [HttpPost("CreateUrl")]
        public async Task<string> CreateUrl(string url)
        {           
            KittenUrlDto dto = new KittenUrlDto();
            string apiResponse;
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://localhost:44351/kittenurl/createurl/?url=" + url))
                {
                    apiResponse = await response.Content.ReadAsStringAsync();
                }
            }
            return apiResponse;
        }
    }
}

