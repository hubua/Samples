using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDB
{
    public static class Generator
    {
        private static Random _random = new Random((int)DateTime.Now.Ticks);
        private static int _seqId = 0;

        private static List<Product> products = new List<Product>();
        private static List<ProductCharacteristic> characteristics = new List<ProductCharacteristic>();

        private static List<Customer> persons = new List<Customer>();

        static Generator()
        {
            var productDataCSV = System.IO.File.ReadAllLines("product_data.csv");
            var productdata = productDataCSV.Skip(1).Select(item => item.Split(',')).Take(30);

            var personDataCSV = System.IO.File.ReadAllLines("person_data.csv");
            var persondata = personDataCSV.Skip(1).Select(item => item.Split(',')).Take(10);

            int order = 0;
            foreach (var row in productdata)
            {
                order++;
                var product = new Product
                {
                    //ID = row[0],
                    SKU = row[1],
                    Name = row[2],
                    Category = row[2][0].ToString(),
                    Price = _random.Next(100, 1000),
                    ItemsInStock = 10,
                };
                var characteristic = new ProductCharacteristic
                {
                    Title = row[3],
                    Value = row[4],
                    Order = order,
                };

                products.Add(product);
                characteristics.Add(characteristic);
            }

            foreach (var product in products)
            {
                product.Characteristics = Enumerable.Range(1, _random.Next(3, 10)).Select(item => characteristics[_random.Next(characteristics.Count)]);
            }



            foreach (var row in persondata)
            {
                switch (_random.Next(3))
                {
                    case 1:
                        persons.Add(CreatePerson<Employee>(row));
                        break;
                    case 2:
                        persons.Add(CreatePerson<Artist>(row));
                        break;
                    default:
                        persons.Add(CreatePerson<Customer>(row));
                        break;
                }
            }
        }

        private static Customer CreatePerson<T>(string[] row) where T : Customer, new()
        {
            var person = new T()
            {
                UID = Guid.NewGuid(),
                SeqId = _seqId++,
                FirstName = row[0],
                LastName = row[1],
                Email = row[2],
                Address = row[3],
                Metadata = new BsonDocument("hobby", (new[] { "Fishing", "Hunting", "Farming" })[_random.Next(3)]),

                PhoneNumbers = new List<PhoneNumber>()
            };

            if (typeof(T) == typeof(Employee))
            {
                (person as Employee).Company = (new[] { "Intel", "Google", "IBM" })[_random.Next(3)];
            }

            if (typeof(T) == typeof(Artist))
            {
                (person as Artist).Art = (new[] { "Painting", "Music", "Singing" })[_random.Next(3)];
            }

            for (int i = 0; i < _random.Next(1, 3); i++)
            {
                person.PhoneNumbers.Add(new PhoneNumber() { Number = $"1 111 {_random.Next(999).ToString("000")}", PhoneType = (PhoneType)_random.Next(3) });
            }

            return person;
        }

        public static IEnumerable<Product> GetProducts()
        {
            return products.AsReadOnly();
        }

        public static IEnumerable<Customer> GetPersons()
        {
            return persons.AsReadOnly();
        }
    }
}
