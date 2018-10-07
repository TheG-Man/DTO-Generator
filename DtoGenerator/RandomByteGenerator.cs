using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DtoGenerator
{
    public class RandomByteGenerator : IRandomValueGenerator
    {
        private readonly Random _random;

        public string Type
        {
            get { return "Byte"; }
        }

        public RandomByteGenerator()
        {
            _random = new Random();
        }

        public object Generate<T>()
        {
            byte[] b = new byte[1];

            _random.NextBytes(b);

            return b[0];
        }
    }
}
