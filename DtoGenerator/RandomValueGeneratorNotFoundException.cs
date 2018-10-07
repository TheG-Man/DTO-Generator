using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DtoGenerator
{
    public class RandomValueGeneratorNotFoundException : Exception
    {
        public RandomValueGeneratorNotFoundException()
        {

        }

        public RandomValueGeneratorNotFoundException(string message)
            : base(message)
        {

        }
    }
}
