

using MongoDB.Bson.Serialization.Attributes;

namespace A.Contracts.Models
{
    public class AdminModel : BaseModel
    {
        [BsonElement("name")]
        public string Name { get; set; }
        
        [BsonElement("gender")]
        public string Gender { get; set; }

        [BsonElement("blood_group")]
        public string Blood_group { get; set; }
    }
}
