using A.Contracts.Update_Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Contracts.Models
{
    public abstract class Student
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

    public class StudentModel : Student, IBaseModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get ; set; }
        public string Username { get; set; }
    }
}
