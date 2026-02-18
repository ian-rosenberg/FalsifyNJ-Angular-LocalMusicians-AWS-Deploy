using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Falsify_Site.Server.Models
{
    public class Venue
    {
        public Venue(string name, string address)
        {
            Name = name;
            Address = address;
        }

        public Venue()
        {
        }

        public Venue(Venue old){
            Name = old.Name;
            Address = old.Address;
        }

        // MongoDB specific 
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public string Name { get; set; } = null!;

        public string Address { get; set; } = null!;

    }
}