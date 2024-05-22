using System;
using Microsoft.AspNetCore.Mvc;
//using EasyCar.Services;
using EasyCar.Models;
using MongoDB.Driver;
using System.Linq;
using Microsoft.VisualBasic;
using System.Collections.ObjectModel;
using MongoDB.Bson;

namespace EasyCar.Controllers
{
    [Controller]
    [Route("[controller]")]
    public class CarController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;
        private readonly IMongoCollection<Dealer> _dealerCollection;

        public CarController(IConfiguration configuration, IWebHostEnvironment environment)
        {
            _configuration = configuration;
            _environment = environment;
        }
        [Route("PreuzmiAuta")]
        [HttpGet]
        public JsonResult Get()
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("MongoDB"));
            var dbList = dbClient.GetDatabase("EasyCar").GetCollection<Car>("Cars").AsQueryable();
            
            return Json(dbList);
        }
        [Route("Filtriraj/{plac}/{godisteOd}/{godisteDo}/{opadajuce}/{cenaOd}/{cenaDo}/{marka}/{model}/{kriterijum}/")]
        [HttpGet]
        public JsonResult GetFiltered(string plac, int? godisteOd, int? godisteDo, int opadajuce, int? cenaOd, int? cenaDo, string marka, string model, int kriterijum)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("MongoDB"));

            var filterBuilder = Builders<Car>.Filter;
            FilterDefinition<Car> filterAutaMarka = null;
            FilterDefinition<Car> filterAutaModel= null;
            FilterDefinition<Car> filterPlac = null;
            FilterDefinition<Car> filterGodiste = null;
            FilterDefinition<Car> filterCena = null;
            SortDefinition<Car> sort = null;

            if (marka!="Sve")
                filterAutaMarka = Builders<Car>.Filter.Eq("Marka", marka);
            if (model != "Sve")
                filterAutaModel = Builders<Car>.Filter.Eq("Model", model);
            if (plac != "Svi")//a to ce bude opcija
                filterPlac = Builders<Car>.Filter.Eq("Prodavac", plac);
            if (godisteOd<godisteDo && godisteDo != 0)
                filterGodiste = Builders<Car>.Filter.And(Builders<Car>.Filter.Gte("GodProizvodnje", godisteOd),
                                Builders<Car>.Filter.Lte("GodProizvodnje", godisteDo));
            if (cenaOd<cenaDo && cenaDo != 0)
                filterCena = Builders<Car>.Filter.And(Builders<Car>.Filter.Gt("Cena", cenaOd),
                                                      Builders<Car>.Filter.Lt("Cena", cenaDo));
            if (kriterijum==1) //1 cena, 0 god
            {
                if (opadajuce == 1)
                    sort = Builders<Car>.Sort.Descending("Cena");
                else
                    sort = Builders<Car>.Sort.Ascending("Cena");
            }
            else
            {
                if (opadajuce == 1)
                    sort = Builders<Car>.Sort.Descending("GodProizvodnje");
                else
                    sort = Builders<Car>.Sort.Ascending("GodProizvodnje");
            }
            var combinedFilter = Builders<Car>.Filter.Empty;


            if (filterAutaMarka != null)
                combinedFilter &= filterAutaMarka;

            if (filterAutaModel != null)
                combinedFilter &= filterAutaModel;

            if (filterPlac != null)
                combinedFilter &= filterPlac;

            if (filterGodiste != null)
                combinedFilter &= filterGodiste;

            if (filterCena != null)
                combinedFilter &= filterCena;
            
            if (combinedFilter == null)
                combinedFilter = Builders<Car>.Filter.Empty;
            Console.WriteLine($"Combined Filter: {combinedFilter.ToJson()}");

            var kolekcijaAuta = dbClient.GetDatabase("EasyCar").GetCollection<Car>("Cars");
            List<Car> auta = kolekcijaAuta.Find(combinedFilter).Sort(sort).ToList();
            return Json(auta);
        }

        [Route("NoviAuto/{ime}")]
        [HttpPost]
        public JsonResult Post([FromBody]Car c, string ime)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("MongoDB"));
           
            var filterDealer = Builders<Dealer>.Filter.Eq("ime", ime);
            var nesto = dbClient.GetDatabase("EasyCar").GetCollection<Dealer>("Dealers");
            var user = nesto.Find(filterDealer).FirstOrDefault();
            c.Prodavac = ime;
            dbClient.GetDatabase("EasyCar").GetCollection<Car>("Cars").InsertOne(c);
            if (user != null && c.Id != null)
            {
                if (user.cars == null)
                    user.cars = new List<string>();
                user.cars.Add(c.Id); 
                //c.Prodavac = user.Id;  //ovo nece sigurno
                var update = Builders<Dealer>.Update.Set("cars", user.cars);
                dbClient.GetDatabase("EasyCar").GetCollection<Dealer>("Dealers").UpdateOne(filterDealer, update);
            }
            return new JsonResult("Added Successfully");
        }
        [Route("PromeniAuto")]
        [HttpPut]
        public JsonResult Put([FromBody]Car c)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("MongoDB"));
            var filter = Builders<Car>.Filter.Eq("Id", c.Id);                 //                J O S !
            var update = Builders<Car>.Update.Set("Marka", c.Marka)
                                                .Set("Model", c.Model)
                                                .Set("GodProizvodnje", c.GodProizvodnje)
                                                .Set("PredjenihKM", c.PredjenihKM)
                                                .Set("Cena", c.Cena)
                                                .Set("Photo", c.Photo);

            dbClient.GetDatabase("EasyCar").GetCollection<Car>("Cars").UpdateOne(filter, update);
            return new JsonResult("Update Succesfully");
        }
        [HttpDelete("ObrisiAuto/{id}")]
        public JsonResult Delete(string id)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("MongoDB"));

            var carFilter = Builders<Car>.Filter.Eq("Id", id);
            var carCollection = dbClient.GetDatabase("EasyCar").GetCollection<Car>("Cars");
            var delaerCollection = dbClient.GetDatabase("EasyCar").GetCollection<Dealer>("Dealers");

            var car = carCollection.Find(carFilter).FirstOrDefault();
            var dealerFilter = Builders<Dealer>.Filter.Eq("ime", car.Prodavac);
            var dealer = delaerCollection.Find(dealerFilter).FirstOrDefault();
            carCollection.DeleteOne(carFilter);
            dealer.cars.Remove(id);

            var update = Builders<Dealer>.Update.Set("cars", dealer.cars);
            delaerCollection.UpdateOne(dealerFilter, update);
            return new JsonResult("Deleted Succesfully");
        }
        [Route("SaveFile")]
        [HttpPost]
        public JsonResult SaveFile()
        {
            try
            {
                var httpRequest = Request.Form;
                var postedFile = httpRequest.Files[0];
                string filename = postedFile.FileName;
                var physicalPath = _environment.ContentRootPath + "/Photos/" + filename;

                using (var stream = new FileStream(physicalPath, FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                }
                return new JsonResult(filename);
            }
            catch
            {
                return new JsonResult("default.png");
            }
        }

    }
}
