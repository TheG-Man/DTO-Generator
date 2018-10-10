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
        object Create(Type type);
    }

    public static class DtoGeneratorExtentions
    {
        public static T Create<T>(this IDtoGenerator dtoGenerator)
        {
            return (T)dtoGenerator.Create(typeof(T));
        }
    } 
}
