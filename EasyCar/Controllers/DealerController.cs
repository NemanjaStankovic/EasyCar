using System;
using Microsoft.AspNetCore.Mvc;
//using EasyCar.Services;
using EasyCar.Models;
using MongoDB.Driver;
using System.Linq;


namespace EasyCar.Controllers
{
    [Controller]
    [Route("[controller]")]
    public class DealerController : Controller
    {
        private readonly IConfiguration _configuration;

        public DealerController(IConfiguration configuration) {
            _configuration = configuration;
        }

        [Route("PreuzmiDealere")]
        [HttpGet]
        public JsonResult Get()
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("MongoDB"));
            var dbList = dbClient.GetDatabase("EasyCar").GetCollection<Dealer>("Dealers").AsQueryable();

            return Json(dbList);
        }
        [Route("PreuzmiDealerImena")]
        [HttpGet]
        public JsonResult GetDI()
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("MongoDB"));
            var dbList = dbClient.GetDatabase("EasyCar").GetCollection<Dealer>("Dealers").AsQueryable();

            return Json(dbList);
        }
        [Route("DodajDealera")]
        [HttpPost]
        public JsonResult Post([FromBody]Dealer d)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("MongoDB"));
            dbClient.GetDatabase("EasyCar").GetCollection<Dealer>("Dealers").InsertOne(d);
            return new JsonResult("Added Successfully");
        }
        [Route("PromeniDealera")]
        [HttpPut]
        public JsonResult Put([FromBody]Dealer d)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("MongoDB"));
            var filter = Builders<Dealer>.Filter.Eq("Id", d.Id);
            var update = Builders<Dealer>.Update.Set("telefon", d.telefon)
                                                .Set("email", d.email)
                                                .Set("ime",d.ime)
                                                .Set("adresa",d.adresa);

            dbClient.GetDatabase("EasyCar").GetCollection<Dealer>("Dealers").UpdateOne(filter, update);
            return new JsonResult("Update Succesfully");
        }
        [Route("ObrisiDealera/{id}")]
        [HttpDelete]
        public JsonResult Delete(string id)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("MongoDB"));
            var dealerCollection = dbClient.GetDatabase("EasyCar").GetCollection<Dealer>("Dealers");

            var filter = Builders<Dealer>.Filter.Eq("Id", id);
            var dealer = dealerCollection.Find(filter).FirstOrDefault();
            foreach (string c in dealer.cars)
            {
                var filterCar = Builders<Car>.Filter.Eq("Id",c);
                dbClient.GetDatabase("EasyCar").GetCollection<Car>("Cars").DeleteOne(filterCar);
            }

            dbClient.GetDatabase("EasyCar").GetCollection<Dealer>("Dealers").DeleteOne(filter);
            return new JsonResult("Deleted Succesfully");
        }
    }
}
