using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TvMaze.Data;
using TvMaze.OuterApi.Models;

namespace TvMaze.OuterApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShowsController : ControllerBase
    {
        private const int PAGE_SIZE = 10;

        private readonly TvMazeRepository _repository;

        public ShowsController(TvMazeRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        [HttpGet]
        public async Task<IActionResult> Get(int? page)
        {
            try
            {
                int pageNumber = page ?? 1;
                int skip = (pageNumber - 1) * PAGE_SIZE;

                var showData = await _repository.GetShowsWithCast(skip, PAGE_SIZE);

                var data = showData.
                    Select(s => new ShowModel(s.Id, s.Name,
                    s.Cast.Select(c => new CastModel(c.Id, c.Name)).ToList()))
                    .ToList();

                return Ok(data);
            } catch(Exception ex)
            {
                return BadRequest(ex);
            }

        }
        

        [HttpGet("{id?}")]
        public async Task<IActionResult> Get(int id)
        {
            int pageNumber = 1;
            int skip = (pageNumber - 1) * PAGE_SIZE;

            var showData = await _repository.GetShowsWithCast(skip, PAGE_SIZE, id);

            var data = showData.
            Select(s => new ShowModel(s.Id, s.Name,
            s.Cast.Select(c => new CastModel(c.Id, c.Name)).ToList()))
            .ToList();

            return Ok(data);
        }

    }
}
