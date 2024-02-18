using MongoDB.Bson.Serialization.Attributes;

namespace A.Contracts.Update_Models
{
    public class UpdateAdminModel
    {
        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("gender")]
        public string Gender { get; set; }

        [BsonElement("blood_group")]
        public string Blood_group { get; set; }
    }
}
