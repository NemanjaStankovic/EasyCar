using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace EasyCar.Models
{
    [BsonIgnoreExtraElements]
    public class Car
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string Marka { get; set; }
        public string Model { get; set; }
        public int GodProizvodnje{ get; set; }
        public int PredjenihKM { get; set; }
        public int Cena { get; set; }
        public byte[]? Photo { get; set; }
        public string Prodavac{ get; set; }

        public Car()
        {
            Prodavac = "";
        }

    }
}
