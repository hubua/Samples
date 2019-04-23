using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDB
{
    public class ProductCharacteristic
    {
        public string Title { get; set; }
        public string Hint { get; set; }
        public string Value { get; set; }
        public int Order { get; set; }
    }

    public class Product
    {
        public ObjectId Id { get; set; }
        public string SKU { get; set; }
        public string Category { get; set; }
        public string Name { get; set; }

        public int Price { get; set; }
        public int ItemsInStock { get; set; }

        public IEnumerable<ProductCharacteristic> Characteristics { get; set; }
    }


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
    
    public class Customer
    {
        public Guid Id { get; set; }
        public Guid UID { get; set; }
        public int SeqId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [BsonIgnore]
        public string FullName { get { return $"{FirstName} {LastName}"; } }
        public string Email { get; set; }
        public string Address { get; set; }
        [BsonIgnoreIfNull]
        public IList<PhoneNumber> PhoneNumbers { get; set; }
        public BsonDocument Metadata { get; set; }
    }

    public class Employee : Customer
    {
        public string Company { get; set; }
    }

    public class Artist : Customer
    {
        public string Art { get; set; }
    }


    public class Basket
    {

    }

    public static class ClassMapper
    {
        public static void Initialize()
        {
            BsonClassMap.RegisterClassMap<Customer>(cm =>
            {
                cm.AutoMap();
                cm.MapIdMember(c => c.Id).SetIdGenerator(CombGuidGenerator.Instance);
                cm.MapExtraElementsMember(c => c.Metadata);
                cm.SetIsRootClass(true);
            });

            BsonClassMap.RegisterClassMap<Employee>();
            BsonClassMap.RegisterClassMap<Artist>();
        }
    }

    public static class TimestampExtension
    {
        public static DateTime TimestampToUTC(this int timestamp)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0).AddSeconds(timestamp);
        }
    }

}
