using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Collections.Generic;

namespace Mongo
{
    
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
