using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DtoGenerator;

namespace RandomDateTimeGenerator
{
    public class RandomDateTimeGenerator: IRandomValueGenerator
    {
        private readonly Random _random;

        public string Type
        {
            get { return "DateTime"; }
        }

        public RandomDateTimeGenerator()
        {
            _random = new Random();
        }

        public object Generate<T>()
        {
            DateTime init = new DateTime(1945, 6, 2);
            int range = (DateTime.Today - init).Days;

            return init.AddDays(_random.Next(range));
        }
    }
}
