using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson;
using MongoDB.Driver;

namespace SimpleMongoDBConnection
{
    public class Program
    {
        static void Main(string[] args)
        {
            try
            {
                MongoClient client = new MongoClient("mongodb://127.0.0.1:27017");

                List<BsonDocument> dbs = client.ListDatabases().ToList();

                Console.WriteLine("Bases de datos en mongo.");
                Console.WriteLine();
                foreach(BsonDocument b in dbs)
                {
                    Console.WriteLine(b["name"]);
                    Console.WriteLine("Collecciones");
                    IMongoDatabase db = client.GetDatabase(b["name"].ToString());
                    List<BsonDocument> coll = db.ListCollections().ToList();
                    foreach (BsonDocument bc in coll)
                    {
                        Console.WriteLine(bc["name"]);
                    }
                    Console.WriteLine();
                }

                // Crear nueva base de datos con una colección para poder agregar datos
                IMongoDatabase database = client.GetDatabase("pruebasC#");
                IMongoCollection<BsonDocument> collection = database.GetCollection<BsonDocument>("autos");
                Auto a = new Auto()
                {
                    Placa = "AGS-001",
                    Marca = "Marca auto",
                    Modelo = "Marca modelo",
                    Anio = 2020
                };

                collection.InsertOne(CreateBson(a));             
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
            Console.ReadKey();
        }

        private static BsonDocument CreateBson(Auto auto)
        {
            BsonDocument element = new BsonDocument();
            element = auto.ToBsonDocument();
            Console.WriteLine("Objeto serializado a BsonDocument");
            Console.WriteLine(element);
            return element;
        }
    }

    public class Auto
    {
        public string Placa { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public int Anio { get; set; }
    }
}
