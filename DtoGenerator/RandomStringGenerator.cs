using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DtoGenerator
{
    public class RandomStringGenerator : IRandomValueGenerator
    {
        private readonly Random _random;

        public string Type
        {
            get { return "String"; }
        }

        public RandomStringGenerator()
        {
            _random = new Random();
        }

        public object Generate<T>()
        {
            byte[] bytes = new byte[25];
            
            _random.NextBytes(bytes);

            for (int i = 0; i < bytes.Length; ++i)
                bytes[i] = (byte)(48 + (byte)(bytes[i] / 3.23));

            return System.Text.Encoding.ASCII.GetString(bytes);
        }
    }
}
