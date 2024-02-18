using MongoDB.Bson.Serialization.Attributes;

namespace A.Contracts.Update_Models
{
    public class UpdateTeacherModel
    {
        [BsonElement("name")]
        public string Name { get; set; }
        [BsonElement("university_name")]
        public string University { get; set; }

        [BsonElement("teacher_id")]
        public string Teacher_id { get; set; }

        [BsonElement("department")]
        public string Department { get; set; }

        [BsonElement("gender")]
        public string Gender { get; set; }

        [BsonElement("blood_group")]
        public string Blood_group { get; set; }
    }
}
