using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace BusinessLogic.Models
{
    public class Stock
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonIgnoreIfDefault]
        public string? Id { get; set; }

        public string? ProdID { get; set; }

        public int Quantity { get; set; }

        public DateTime OperationDate { get; set; }

        [BsonElement("Timestamp")]
        public long Timestamp { get; set; }
    }
}
