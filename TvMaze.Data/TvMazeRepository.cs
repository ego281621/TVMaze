using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TvMaze.Data.Entities;

namespace TvMaze.Data
{
    public class TvMazeRepository
    {
        private readonly IConfiguration _configuration;

        public TvMazeRepository(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task PutShowAndCast(Show show)
        {
            var connection = new SqlConnection(_configuration.GetConnectionString("TvMaze"));
            var command = new SqlCommand("spBulkInsertShowAndCasts", connection)
            {
                CommandType = CommandType.StoredProcedure,
                CommandTimeout = 600
            };

            command.Parameters.AddWithValue("@TvMazeShowId", show.TvMazeShowId);
            command.Parameters.AddWithValue("@Name", show.Name);

            var castDBParam = show.Cast.Select(x => new CastDBParam
            {
                TvMazePersonId = x.TvMazePersonId,
                Name = x.Name
            }).ToList();

            CastCollection castTable = new CastCollection();
            castTable.AddRange(castDBParam);

            SqlParameter sqlParam = command.Parameters.AddWithValue("@CastPersons", castTable);
            sqlParam.SqlDbType = SqlDbType.Structured;

            try
            {
                connection.Open();
                await command.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                connection.Dispose();
                command.Dispose();
            }
        }

        public async Task<List<Show>> GetShowsWithCast(int skip, int take, int showId = 0)
        {
			var shows = new List<Show>();

			var connection = new SqlConnection(_configuration.GetConnectionString("TvMaze"));
			var command = new SqlCommand("spGetShow", connection)
			{
				CommandType = CommandType.StoredProcedure
			};

			command.Parameters.AddWithValue("@Skip", skip);
			command.Parameters.AddWithValue("@Take", take);

			if(showId > 0)
            {
				command.Parameters.AddWithValue("@ShowId", showId);
			}

			try
			{
				connection.Open();
				SqlDataReader dataReader;
				using (dataReader = command.ExecuteReader())
				{
					while (dataReader.Read())
					{
						var show = new Show
						{
							Id = Convert.ToInt32(dataReader["Id"]),
							TvMazeShowId = Convert.ToInt32(dataReader["TvMazeShowId"]),
                            Name = dataReader["Name"].ToString(),
						};

						show.Cast = await GetCastByShowId(show.Id);
						shows.Add(show);
					}
				}
			}
			catch (Exception ex)
			{
                throw ex;
			}
			finally
			{
				connection.Dispose();
				command.Dispose();
			}

			return shows;
		}
		private async Task<List<Cast>> GetCastByShowId(int showId)
		{
			var casts = new List<Cast>();

			var connection = new SqlConnection(_configuration.GetConnectionString("TvMaze"));
			var command = new SqlCommand("spGetCastByShowId", connection)
			{
				CommandType = CommandType.StoredProcedure
			};

			command.Parameters.AddWithValue("@ShowId", showId);

			try
			{
				connection.Open();
				SqlDataReader dataReader;
				using (dataReader = command.ExecuteReader())
				{
					while (dataReader.Read())
					{
						var cast = new Cast
						{
							Id = Convert.ToInt32(dataReader["Id"]),
							ShowId = Convert.ToInt32(dataReader["ShowId"]),
							TvMazePersonId = Convert.ToInt32(dataReader["TvMazePersonId"]),
							Name = dataReader["Name"].ToString(),
						};

						casts.Add(cast);
					}
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
				connection.Dispose();
				command.Dispose();
			}

			return casts;
		}

	}
}
