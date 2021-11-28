using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using NUnit.Framework;
using TvMaze.ApiClient.Models;
using Shouldly;

namespace TvMaze.ApiClient.Integration.Tests
{
    [TestFixture]
    public class TvMazeApiClientTests
    {
        private TvMazeApiClient _tvMazeApiClient;

        [SetUp]
        public void SetUp()
        {
            _tvMazeApiClient = new TvMazeApiClient(new HttpClient());
        }

        [Test]
        public async Task GetShowsRetrievesFirstPage()
        {
            // arrange 
            int page = 0;

            // act
            List<ShowEntry> shows = await _tvMazeApiClient.GetShowsAsync(page);

            //assert
            shows.ShouldNotBeNull();
            shows.Count.ShouldBeGreaterThan(1);

            shows[0].Id.ShouldBe(1);
            shows[0].Name.ShouldBe("Under the Dome");

            shows[1].Id.ShouldBe(2);
            shows[1].Name.ShouldBe("Person of Interest");
        }

        [Test]
        public async Task GetShowsThrowsNotFoundExceptionForPagesFarAwayAsync()
        {
            // arrange
            int unreachablePage = 99999;

            // act
            await _tvMazeApiClient.GetShowsAsync(unreachablePage).ShouldThrowAsync<NotFoundException>();

        }

        [Test]
        public async Task GetsShowCast()
        {
            // arrange
            int showId = 1;

            // act
            List<CastEntry> cast = await _tvMazeApiClient.GetCastAsync(showId);

            //assert
            cast.ShouldNotBeNull();
            cast.Count.ShouldBeGreaterThan(1);

            cast[0].Person.ShouldNotBeNull();
            cast[0].Person.Id.ShouldBe(1);
            cast[0].Person.Name.ShouldBe("Mike Vogel");

            cast[1].Person.ShouldNotBeNull();
            cast[1].Person.Id.ShouldBe(2);
            cast[1].Person.Name.ShouldBe("Rachelle Lefevre");

        }
    }
}
