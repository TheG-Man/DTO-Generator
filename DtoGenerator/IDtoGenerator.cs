using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace DtoGenerator
{
    public interface IDtoGenerator
    {
        T Create<T>();
        object[] GetConstructorArguments(ConstructorInfo ctor);
    }
}
