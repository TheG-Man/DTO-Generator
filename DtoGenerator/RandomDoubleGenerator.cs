using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DtoGenerator
{
    public class RandomDoubleGenerator : IRandomValueGenerator
    {
        private readonly Random _random;

        public string Type
        {
            get { return "Double"; }
        }

        public RandomDoubleGenerator()
        {
            _random = new Random();
        }

        public object Generate<T>()
        {
            return _random.NextDouble();
        }
    }
}
