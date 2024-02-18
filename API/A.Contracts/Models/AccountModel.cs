
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace A.Contracts.Models
{
    public class AccountModel : BaseModel
    {
        [BsonRequired]
        public byte[] PasswordHash { get; set; }

        [BsonRequired]
        public byte[] PasswordSalt { get; set; }

        [BsonRequired]
        public string Role { get; set; }
    }
}
