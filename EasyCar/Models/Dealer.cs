using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System.Text.Json.Serialization;
namespace EasyCar.Models
{
    public class Dealer
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string ime { get; set; }
        public string adresa { get; set; }
        public string email { get; set; }
        public string telefon { get; set; }
        public List<string> cars{ get; set; }

        public Dealer()
        {
            cars = new List<string>();
        }
    }
}
