using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Collections.Generic;

namespace Mongo
{

    /*
     * https://www.tutorialspoint.com/mongodb/mongodb_overview.htm
     * http://mongodb.github.io/mongo-csharp-driver/2.2/getting_started/quick_tour/
     * http://mongodb.github.io/mongo-csharp-driver/2.2/reference/bson/mapping/polymorphism/
     * https://docs.mongodb.com/v3.2/tutorial/enable-authentication/
     * https://docs.mongodb.com/getting-started/csharp/indexes/
     * https://docs.mongodb.com/manual/tutorial/perform-two-phase-commits/
     * http://www.json-generator.com/
     */

    class Program
    {
        static void Main(string[] args)
        {
            ClassMapper.Initialize();

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

            //var filter = Builders<Person>.Filter.Eq("UID", person1.UID);
            //var r = collectionPerson.Find(filter).First();
            
            var r = collectionPerson.Find(item => item.UID == person1.UID).First();



            var q = collectionPerson.AsQueryable().Where(item => item.FirstName.StartsWith(person1.FirstName[0].ToString())).OrderBy(item => item.FirstName);
            foreach (var item in q)
            {
                System.Console.WriteLine(item.FullName);
            }
            
        }
    }
}
