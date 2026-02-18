using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Falsify_Site.Server.Models
{
    public class ContactModel
    {
        public required string Name { get; set; }
        public required string Email { get; set; }

        public string? Phone { get; set; }

        public required string Message { get; set; }
    }
}
