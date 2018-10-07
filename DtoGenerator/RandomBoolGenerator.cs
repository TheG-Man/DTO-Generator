using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DtoGenerator
{
    public class RandomBoolGenerator: IRandomValueGenerator
    {
        public string Type
        {
            get { return "Boolean"; }
        }

        public object Generate<T>()
        {
            return (object)true;
        }
    }
}
