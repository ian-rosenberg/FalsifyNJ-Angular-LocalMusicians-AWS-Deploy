using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Falsify_Site.Server.Models
{
    public class LiveShows
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public string DateId { get; set; } = null!;

        public int DoorTime { get; set; } = 0;
        public int ShowTime { get; set; } = 0;
        
        public string Description { get; set; } = null!;

        public string PosterLink { get; set; } = null!;


        [BsonRepresentation(BsonType.ObjectId)]
        public string? VenueId { get; set; }

        public Venue Venue { get; set; } = null!;   

        public string? TicketsLink { get; set; } = null;
    }
}
