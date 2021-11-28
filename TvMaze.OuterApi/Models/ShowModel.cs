using System.Collections.Generic;

namespace TvMaze.OuterApi.Models
{
    public class ShowModel
    {
        public int Id { get; }

        public string Name { get; }

        public List<CastModel> Cast { get; }

        public ShowModel(int id, string name, List<CastModel> cast)
        {
            Id = id;
            Name = name;
            Cast = cast;
        }
    }
}
