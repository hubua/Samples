using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;

namespace Mongo
{

    /*
     * https://www.tutorialspoint.com/mongodb/mongodb_overview.htm
     * https://docs.mongodb.com/getting-started/csharp/indexes/
     * http://mongodb.github.io/mongo-csharp-driver/2.2/getting_started/quick_tour/
     * http://www.json-generator.com/
     */

    class Program
    {
        static void Main(string[] args)
        {
            var client = new MongoClient("mongodb://localhost:27017");
            var database = client.GetDatabase("sample");
            var collection = database.GetCollection<BsonDocument>("restaraunts");

            var persons = new List<Person>()
            {
                Generator.GeneratePerson(),
                Generator.GeneratePerson(),
                Generator.GeneratePerson(),
            };
            var person1 = persons[0];

            var collectionPerson = database.GetCollection<Person>("Persons");
            collectionPerson.InsertMany(persons);

            var filter = Builders<Person>.Filter.Eq("UID", person1.UID);
            var r = collectionPerson.Find(filter).First();

            
        }
    }
}
