using System;

namespace TvMaze.Data.Entities
{
    public class Cast
    {
        public int Id { get; set; }

        public int TvMazePersonId { get; set; }

        public int ShowId { get; set; }

        public string Name { get; set; }
    }
}
