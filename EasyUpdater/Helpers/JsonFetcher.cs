using System;
using System.Net;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace EasyUpdater.Helpers
{
    public static class JsonFetcher
    {
        public static async Task<T> FetchAsync<T>(string url)
        { 
            using var wc = new WebClient();
            var urlContent = await wc.DownloadStringTaskAsync(new Uri(url));
            return JsonConvert.DeserializeObject<T>(urlContent);
        }

        public static T Fetch<T>(string url)
        {
            return FetchAsync<T>(url).Result;
        }
    }
}