using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mongo
{
    public class Person
    {
        public Guid UID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [BsonIgnore]
        public string FullName { get { return $"{FirstName} {LastName}"; } }
        public string Email { get; set; }
        public string Address { get; set; }
        [BsonIgnoreIfNull]
        public IList<PhoneNumber> PhoneNumbers { get; set; }
        // [BsonExtraElements]
        public BsonDocument Metadata { get; set; }
    }

    public class PhoneNumber
    {
        public string Number { get; set; }
        public PhoneType PhoneType { get; set; }
    }

    public enum PhoneType
    {
        Home,
        Work,
        Mobile
    }

    public static class ClassMapper
    {
        public static void Initialize()
        {
            BsonClassMap.RegisterClassMap<Person>(cm =>
            {
                cm.AutoMap();
                cm.MapExtraElementsMember(c => c.Metadata);
            });
        }
    }

    public static class Generator
    {
        private static IEnumerable<string[]> _sampledata;
        private static Random _random;

        static Generator()
        {
            var samplefile = System.IO.File.ReadAllLines("sample_data.csv");
            _sampledata = samplefile.Skip(1).Select(item => item.Split(','));
            _random = new Random((int)DateTime.Now.Ticks);
        }

        public static Person GeneratePerson()
        {
            var row = _sampledata.ToList()[_random.Next(_sampledata.Count())];

            var result = new Person()
            {
                UID = Guid.NewGuid(),
                FirstName = row[0],
                LastName = row[1],
                Email = row[2],
                Address = row[3],
                Metadata = new BsonDocument("rel", "metatada")
            };

            for (int i = 0; i < _random.Next(3); i++)
            {
                if (result.PhoneNumbers == null)
                {
                    result.PhoneNumbers = new List<PhoneNumber>();
                }
                result.PhoneNumbers.Add(new PhoneNumber() { Number = $"1 111 {_random.Next(999).ToString("000")}", PhoneType = (PhoneType)_random.Next(2) });
            }
            
            return result;    
        }
    }

}
