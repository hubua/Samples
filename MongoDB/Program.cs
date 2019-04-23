using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MongoDB
{
    class Program
    {
        private static MongoClientSettings GetMongoClientSettings()
        {
            IConfigurationRoot Configuration;

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            builder.AddUserSecrets<Program>();
            Configuration = builder.Build();

            var mongo_admin_dbname = "admin";
            var mongo_username = "admin";
            var mongo_password = Configuration["mongo_pass"];

            var mongo_db_host = Configuration["DbHost"];
            var mongo_db_port = Convert.ToInt32(Configuration["DbPort"]);

            var mongoServer = new MongoServerAddress(mongo_db_host, mongo_db_port);
            var mongoCredential = MongoCredential.CreateCredential(mongo_admin_dbname, mongo_username, mongo_password);

            var mongoClientSettings = new MongoClientSettings() { Server = mongoServer, Credential = mongoCredential };
            return mongoClientSettings;
        }

        static async Task Main(string[] args)
        {
            MongoClientSettings mongoClientSettings = GetMongoClientSettings();

            var mongoClient = new MongoClient(mongoClientSettings);

            var database = mongoClient.GetDatabase("CatalogSample");

            database.DropCollection("Products");
            var productsCollection = database.GetCollection<Product>("Products");
            await productsCollection.InsertManyAsync(Generator.GetProducts());

            database.DropCollection("Customers");
            var customersCollection = database.GetCollection<Customer>("Customers");
            await customersCollection.InsertManyAsync(Generator.GetPersons());

            var personsCount = productsCollection.CountDocuments(new BsonDocument());
            var customersCount = customersCollection.CountDocuments(new BsonDocument());


            Console.WriteLine(personsCount);
            Console.WriteLine(customersCount);

            var price_field_name = nameof(Product.Price);


            var filterBuilder = Builders<Product>.Filter;
            var filter = filterBuilder.Gt(price_field_name, 800) & filterBuilder.Lte(price_field_name, 900);

            var expensiveProducts = productsCollection.Find(filter).ToEnumerable();
            var d = expensiveProducts.First().Id.Timestamp.TimestampToUTC().ToLocalTime();

            Console.WriteLine(d);
        }

    }
}
