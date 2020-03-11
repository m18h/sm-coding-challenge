using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using sm_coding_challenge.Models;

namespace sm_coding_challenge.Services.DataProvider
{
    // Refactored to use HTTP client factory, to reduce socket exhaustion errors, properly manage number of clients running, and properly dispose of them
    public class DataProviderImpl : IDataProvider
    {
        public static TimeSpan Timeout = TimeSpan.FromSeconds(30);
        // Readonly HTTP client from the client factory
        private readonly HttpClient _client;
        private IMemoryCache _cache;

        // Added constructor to initialize HTTP client
        public DataProviderImpl(HttpClient client, IMemoryCache cache)
        {
            _client = client;
            _cache = cache;
        }

        /// <summary>
        /// This method is the primary way to fetch data. 
        /// It first checks to see if there's a cached copy of data, and then checks the age of said data. If the data is still usuable, it uses
        /// </summary>
        /// <returns>DataResponseModel object of all the data</returns>
        private async Task<DataResponseModel> FetchDataAsync()
        {
            // Check cache to see if a timestamp has been set
            if (_cache.TryGetValue(CacheKeys.FetchTimestamp, out DateTime dataTimestamp))
            {
                // If timestamp has been set, check to make sure it hasn't exceeded 7 days (since data is loaded once a week)
                var timeDifference = dataTimestamp - DateTime.Now;
                if (timeDifference.Days < 7)
                {
                    // If data age is still valid, check if any data is actually saved in cache
                    if (_cache.TryGetValue(CacheKeys.Data, out DataResponseModel cachedData))
                    {
                        // If cached data is not empty, return the data
                        if (cachedData != null)
                            return cachedData;
                    }
                }
            }

            // Fetch new data from endpoint
            _client.Timeout = Timeout;
            // Refactored GET request from synchronous call to asynchronous call
            var response = await _client.GetAsync("https://gist.githubusercontent.com/RichardD012/a81e0d1730555bc0d8856d1be980c803/raw/3fe73fafadf7e5b699f056e55396282ff45a124b/basic.json");
            // Refactored response read from synchronous call to asynchronous call 
            var stringData = await response.Content.ReadAsStringAsync();
            var dataResponse = JsonConvert.DeserializeObject<DataResponseModel>(stringData, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });

            // Save data to cache
            _cache.Set(CacheKeys.Data, dataResponse);
            _cache.Set(CacheKeys.FetchTimestamp, DateTime.Now);

            // Return data retrieved from endpoint
            return dataResponse;
        }

        // Made method async through rename, and return type
        public async Task<PlayerModel> GetPlayerByIdAsync(string id)
        {
            // Fetch data
            var data = await FetchDataAsync();
            
            foreach(var player in data.Rushing)
            {
                if(player.Id.Equals(id))
                {
                    return player;
                }
            }
            foreach(var player in data.Passing)
            {
                if(player.Id.Equals(id))
                {
                    return player;
                }
            }
            foreach(var player in data.Receiving)
            {
                if(player.Id.Equals(id))
                {
                    return player;
                }
            }
            foreach(var player in data.Receiving)
            {
                if(player.Id.Equals(id))
                {
                    return player;
                }
            }
            foreach(var player in data.Kicking)
            {
                if(player.Id.Equals(id))
                {
                    return player;
                }
            }
            
            return null;
        }

        // Implement method to get "latest" players
        public async Task<PlayerDataModel> GetLatestPlayersAsync(string[] ids)
        {
            // Fetch data
            var data = await FetchDataAsync();

            // search for player id in datasets
            var rushingPlayers = data.Rushing.Where(i => ids.Contains(i.Id));
            var passingPlayers = data.Passing.Where(i => ids.Contains(i.Id));
            var receivingPlayers = data.Receiving.Where(i => ids.Contains(i.Id));
            var kickingPlayers = data.Kicking.Where(i => ids.Contains(i.Id));

            var dataResponse = new PlayerDataModel
            {
                Rushing = rushingPlayers?.ToList(),
                Passing = passingPlayers?.ToList(),
                Receiving = receivingPlayers?.ToList(),
                Kicking = kickingPlayers?.ToList()
            };

            return dataResponse;
        }
    }
}
