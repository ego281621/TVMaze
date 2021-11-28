using System;
using TvMaze.Scraper.Model;

namespace TvMaze.Scraper
{
    public class CallResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public IngestionResult IngestionResult { get; set; }
        public CallResult() { }

        public CallResult(Guid id, bool success, string message, IngestionResult ingestionResult)
        {
            Success = success;
            Message = message;
            IngestionResult = ingestionResult;
        }

    }
}
