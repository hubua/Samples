using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Mongo
{
    
    class Program
    {
        static void Main(string[] args)
        {
            const bool RECREATE_DB = true;

            ClassMapper.Initialize();

            var persons = Generator.GeneratePersons(0);
            if (RECREATE_DB)
            {
                persons = Generator.GeneratePersons(10000000);
            }

            var client = new MongoClient("mongodb://localhost:27017");
            var database = client.GetDatabase("sample");
            var collectionRestraurants = database.GetCollection<BsonDocument>("restaraunts");

            if (RECREATE_DB)
            {
                database.DropCollection("Persons");
            }

            var personCollection = database.GetCollection<Person>("Persons");

            if (RECREATE_DB)
            {
                //var keys = Builders<Person>.IndexKeys.Ascending("_t");
                //personCollection.Indexes.CreateOne(keys);

                personCollection.InsertMany(persons);
            }
            /*var c = personCollection.Count(new BsonDocument("Art", "Singing")); // 2
            Console.WriteLine(c);

            c = personCollection.Count(x => x.UID == person1.UID); // 1

            var filter = new BsonDocument();
            var all = personCollection.Find(filter).ToList();

            var artists = personCollection.AsQueryable().OfType<Person>();

            var r = personCollection.Find(item => item.UID == person1.UID).First();

            */

            var sw = Stopwatch.StartNew();
            // agregate search
            var resultAgregate = personCollection.AsQueryable().OfType<Artist>().ToList();
            Console.WriteLine(sw.ElapsedMilliseconds);


            // filter search
            sw = Stopwatch.StartNew();
            var t = typeof(Artist);
            const string DISCRIMINATOR_FIELD = "_t";
            var resultFilter = personCollection.Find(new BsonDocument(DISCRIMINATOR_FIELD, t.Name)).ToList();
            Console.WriteLine(sw.ElapsedMilliseconds);

            Console.WriteLine($"{resultAgregate.Count} {resultFilter.Count}");


            //var q = personCollection.AsQueryable().Where(item => item.FirstName.StartsWith(person1.FirstName[0].ToString())).OrderBy(item => item.FirstName);
            //foreach (var item in q)
            //{
                
            //}
            
        }
    }

    /*
     * 
     * 
     * public async Task<IEnumerable<TEntity>> GetAll()
        {
            var collection = _mongoDbContext.GetCollection<TEntity>();

            // command agregate basememoryevents $match _t MovieME
            await collection.AsQueryable().OfType<TEntity>().ToListAsync();

            // query find basememoryevens filter _t MovieME
            var t = typeof(TEntity);
            const string DISCRIMINATOR_FIELD = "_t";
            await collection.Find(new BsonDocument(DISCRIMINATOR_FIELD, t.Name)).ToListAsync();
            //return entities;
}



internal class MongoDbContext
{
    private static readonly IMongoClient _client;
    private static readonly IMongoDatabase _database;

    static MongoDbContext()
    {
        ClassMapper.Initialize();

        _client = new MongoClient("mongodb://localhost:27017");
        _database = _client.GetDatabase("MSS");

        // TESTS INIT CODE
        if (!_database.ListCollections().ToList().Exists(item => item["name"].AsString == "omdbmovies"))
        {
            InitMemoryEvents.Seed();
        }

    }

    public IMongoCollection<TEntity> GetCollection<TEntity>()
    {
        var basetype = (typeof(TEntity).BaseType == typeof(object)) ? typeof(TEntity) : typeof(TEntity).BaseType;
        string collectionName = String.Concat(basetype.Name.ToLower(), "s");
        return _database.GetCollection<TEntity>(collectionName);
    }

}
     */
}
