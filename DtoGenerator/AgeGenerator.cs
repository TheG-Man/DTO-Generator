using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DtoGenerator
{
    public class AgeGenerator: IRandomValueGenerator
    {
        private readonly Random _random;

        public string Type
        {
            get { return "Byte"; }
        }

        public AgeGenerator()
        {
            _random = new Random();
        }

        public object Generate<T>()
        {
            byte b = (byte)_random.Next(1, 47);

            return b;
        }
    }
}
