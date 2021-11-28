using System.Collections.Generic;

namespace TvMaze.Data.Entities
{
    public class Show
    {
        public int Id { get; set; }

        public int TvMazeShowId { get; set; }

        public string Name { get; set; }

        public List<Cast> Cast { get; set; }

    }
}
