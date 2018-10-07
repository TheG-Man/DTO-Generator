using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DtoGenerator
{
    public class TypeNotSupportedException : Exception
    {
        public TypeNotSupportedException()
        {

        }

        public TypeNotSupportedException(string message)
            :base(message)
        {

        }
    }
}
