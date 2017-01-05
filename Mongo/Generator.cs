using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mongo
{
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

        public static Person GeneratePerson<T>(string spec) where T : Person, new()
        {
            var row = _sampledata.ToList()[_random.Next(_sampledata.Count())];

            var result = new T()
            {
                UID = Guid.NewGuid(),
                FirstName = row[0],
                LastName = row[1],
                Email = row[2],
                Address = row[3],
                Metadata = new BsonDocument("hobby", spec)
            };

            if (typeof(T) == typeof(Employee))
            {
                (result as Employee).Company = spec;
            }

            if (typeof(T) == typeof(Artist))
            {
                (result as Artist).Art = spec;
            }

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
