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
    public enum PhoneType
    {
        Home,
        Work,
        Mobile
    }

    public class PhoneNumber
    {
        public string Number { get; set; }
        public PhoneType PhoneType { get; set; }
    }
    
    // [BsonKnownTypes(typeof(Employee), typeof(Artist))]
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

    public class Employee : Person
    {
        public string Company { get; set; }
    }

    public class Artist : Person
    {
        public string Art { get; set; }
    }

    public static class ClassMapper
    {
        public static void Initialize()
        {
            BsonClassMap.RegisterClassMap<Person>(cm =>
            {
                cm.AutoMap();
                cm.MapExtraElementsMember(c => c.Metadata);
                // cm.SetIsRootClass(true);
            });

            BsonClassMap.RegisterClassMap<Employee>();
            BsonClassMap.RegisterClassMap<Artist>();
        }
    }

}
