using System;
using System.Net;
using System.Net.Http;
using Polly;

namespace TvMaze.Scraper.PolicyProvider
{
    public static class RetryPolicyProvider
    {
        private const int RETRY_COUNT = 5;

        private const float INITIAL_DELAY_IN_SECONDS = 1.5f;

        private const int JITTER_IN_MILLISECONDS = 500;

        private static readonly Random _jitterer = new Random();

        public static IAsyncPolicy<HttpResponseMessage> Get()
        {
            var policy = Policy
                .HandleResult<HttpResponseMessage>(ShouldRetryRequest)
                .WaitAndRetryAsync(RETRY_COUNT, CalculateDelay);

            return policy;
        }

        private static bool ShouldRetryRequest(HttpResponseMessage response)
        {
            // NOTE: we might consider 500 as well
            return response.StatusCode == HttpStatusCode.TooManyRequests;
        }

        private static TimeSpan CalculateDelay(int retryAttempt)
        {
            TimeSpan exponentialBackOff = TimeSpan.FromSeconds(INITIAL_DELAY_IN_SECONDS * Math.Pow(2, retryAttempt - 1));
            TimeSpan jitter = TimeSpan.FromMilliseconds(_jitterer.Next(0, JITTER_IN_MILLISECONDS));

            return exponentialBackOff + jitter;
        }
    }
}
