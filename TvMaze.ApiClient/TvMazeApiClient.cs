using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TvMaze.ApiClient.Models;

namespace TvMaze.ApiClient
{
    public class TvMazeApiClient
    {
        private const string BASE_URL = "http://api.tvmaze.com";

        private readonly HttpClient _httpClient;

        public TvMazeApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));

            _httpClient.BaseAddress = new Uri(BASE_URL);
        }

        public Task<List<ShowEntry>> GetShowsAsync(int page)
        {
            return GetJsonAsync<List<ShowEntry>>($"/shows?page={page}");
        }

        public Task<List<CastEntry>> GetCastAsync(int showId)
        {
            return GetJsonAsync<List<CastEntry>>($"/shows/{showId}/cast");
        }

        private async Task<T> GetJsonAsync<T>(string url)
        {
            HttpResponseMessage response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                switch (response.StatusCode)
                {
                    case HttpStatusCode.NotFound:
                        throw new NotFoundException();
                    default:
                        throw new ApiErrorException(response.StatusCode);
                }
            }

            string content = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(content);
        }
    }
}
