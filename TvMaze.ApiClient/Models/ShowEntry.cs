namespace TvMaze.ApiClient
{
    public class ShowEntry
    {
        public int Id { get; }

        public string Name { get; }

        public ShowEntry(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
