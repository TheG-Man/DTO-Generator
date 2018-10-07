using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DtoGenerator
{
    public class SupportedTypes
    {
        private readonly Dictionary<Type, string> _types = new Dictionary<Type, string>
        {
            { typeof(sbyte), "Byte" },
            { typeof(byte), "Byte" },
            { typeof(char), "Byte" },
            { typeof(short), "Byte" },
            { typeof(ushort), "Byte" },
            { typeof(int), "Byte" },
            { typeof(uint), "Byte" },
            { typeof(long), "Byte" },
            { typeof(float), "Double" },
            { typeof(double), "Double" },
            { typeof(decimal), "Double" },
            { typeof(bool), "Boolean" },
            { typeof(string), "String" },
            { typeof(DateTime), "DateTime" },
            { typeof(List<sbyte>), "List" },
            { typeof(List<byte>), "List" },
            { typeof(List<char>), "List" },
            { typeof(List<short>), "List" },
            { typeof(List<ushort>), "List" },
            { typeof(List<int>), "List" },
            { typeof(List<uint>), "List" },
            { typeof(List<long>), "List" },
            { typeof(List<float>), "List" },
            { typeof(List<double>), "List" },
            { typeof(List<decimal>), "List" },
            { typeof(List<bool>), "List" },
            { typeof(List<string>), "List" },
            { typeof(List<DateTime>), "List" }

        };

        public bool Has(Type type)
        {
            return _types.TryGetValue(type, out string value);
        }

        public bool Has(Type type, out string value)
        {
            return _types.TryGetValue(type, out value);
        }
    }
}
