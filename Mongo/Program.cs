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

            database.DropCollection("Persons");

            var persons = new List<Person>()
            {
                Generator.GeneratePerson<Person>("Diving"),
                Generator.GeneratePerson<Person>("Diving"),
                Generator.GeneratePerson<Person>("Singing"),
                Generator.GeneratePerson<Employee>("Serios Co"),
                Generator.GeneratePerson<Artist>("Music"),
                Generator.GeneratePerson<Artist>("Music"),
                Generator.GeneratePerson<Artist>("Painting"),
            };
            var person1 = persons[0];

            var collectionPerson = database.GetCollection<Person>("Persons");
            
            collectionPerson.InsertMany(persons);

            var c = collectionPerson.Count(new BsonDocument("Art", "Music")); // 2
            c = collectionPerson.Count(x => x.UID == person1.UID); // 1

            var filter = new BsonDocument();
            var all = collectionPerson.Find(filter).ToList();

            var artists = collectionPerson.AsQueryable().OfType<Person>();

            var r = collectionPerson.Find(item => item.UID == person1.UID).First();



            var q = collectionPerson.AsQueryable().Where(item => item.FirstName.StartsWith(person1.FirstName[0].ToString())).OrderBy(item => item.FirstName);
            foreach (var item in q)
            {
                System.Console.WriteLine(item.FullName);
            }
            
        }
    }
}
