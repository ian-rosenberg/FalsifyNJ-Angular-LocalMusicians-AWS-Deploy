namespace Falsify_Site.Server.Models
{
    public class FalsifyDatabaseSettings
    {
        public string ConnectionString { get; set; } = null!;

        public string DatabaseName { get; set; } = null!;

        public string LiveShowsCollectionName { get; set; } = null!;
        public string VenueCollectionName { get; set; } = null!;
    }
}
