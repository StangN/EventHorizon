using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace WebApplication1.Models
{
    public class Entry
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public ObjectId Id { get; set; }
        public string Stock { get; set; }

        public double Value { get; set; }

        public string Headline { get; set; }

        public double ValueAfterHour { get; set; }
    }
}
