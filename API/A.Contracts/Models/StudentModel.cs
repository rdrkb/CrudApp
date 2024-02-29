using MongoDB.Bson.Serialization.Attributes;

namespace Contracts.Models
{
    public class StudentModel : BaseModel
    {
        [BsonElement("name")]
        public string Name { get; set; }
        [BsonElement("university_name")]
        public string University { get; set; }

        [BsonElement("student_id")]
        public string Student_id { get; set; }

        [BsonElement("department")]
        public string Department { get; set; }

        [BsonElement("gender")]
        public string Gender { get; set; }

        [BsonElement("year_of_graduation")]
        public string Year_of_graduation { get; set; }

        [BsonElement("blood_group")]
        public string Blood_group { get; set; }
    }
}
