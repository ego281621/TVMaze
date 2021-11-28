using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TvMaze.ApiClient;
using TvMaze.ApiClient.Models;
using TvMaze.Data;
using TvMaze.Data.Entities;
using TvMaze.Scraper.Model;

namespace TvMaze.Scraper
{
    public class TvMazeScraper
    {
        private readonly TvMazeApiClient _apiClient;
        private readonly TvMazeRepository _tvMazeRepository;
        private readonly ILogger<TvMazeScraper> _logger;

        public TvMazeScraper(
            TvMazeApiClient apiClient,
            TvMazeRepository tvMazeRepository,
            ILogger<TvMazeScraper> logger
        )
        {
            _apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
            _tvMazeRepository = tvMazeRepository ?? throw new ArgumentNullException(nameof(tvMazeRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task RunAsync()
        {
            const int PAGE_LIMIT = 2;

            for (int page = 0; page < PAGE_LIMIT; page++)
            {
                CallResult result = await BatchInsert(page);

                switch (result.IngestionResult)
                {
                    case IngestionResult.Success:
                        break;

                    case IngestionResult.NothingToProcess:
                        _logger.LogInformation("Finished processing all batches");
                        return;

                    case IngestionResult.Failure:
                        break;
                }
            }

            _logger.LogError("Stopped ingestion as error count exceeded the limit");
        }

        private async Task<CallResult> BatchInsert(int page)
        {
            try
            {
                List<ShowEntry> showsBatch = await _apiClient.GetShowsAsync(page);
                _logger.LogInformation("Received page {0} from the API, got {1} show(s)", page, showsBatch.Count);

                foreach (ShowEntry showEntry in showsBatch)
                {
                    int showId = showEntry.Id;

                    try
                    {
                        List<CastEntry> cast = await _apiClient.GetCastAsync(showId);
                        _logger.LogInformation("Retrieved the cast for show {0}", showId);

                        await SaveShow(showEntry, cast);
                    }
                    catch (Exception exception)
                    {
                        _logger.LogError(exception, "Error when retrieving cast for show {0}", showId);
                    }
                }

                return new CallResult(Guid.Empty, true, "Show and Cast successfully created.", IngestionResult.Success);
            }
            catch (NotFoundException ne)
            {
                _logger.LogInformation("Shows for page not found {0}", page);
                return new CallResult(Guid.Empty, false, ne.Message, IngestionResult.NothingToProcess);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Error when getting page {0} from the API", page);
                return new CallResult(Guid.Empty, false, exception.Message, IngestionResult.Failure);
            }
        }

        private async Task SaveShow(ShowEntry showHeader, List<CastEntry> cast)
        {
            try
            {
                List<Cast> castDbos = cast.Select(it => new Cast
                {
                    TvMazePersonId = it.Person.Id,
                    Name = it.Person.Name,
                }).ToList();

                var showDbo = new Show
                {
                    TvMazeShowId = showHeader.Id,
                    Name = showHeader.Name,
                    Cast = castDbos
                };

                await _tvMazeRepository.PutShowAndCast(showDbo);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Cannot save show {0} into the database", showHeader.Id);
            }
        }
    }
}
