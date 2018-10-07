using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DtoGenerator
{
    public class ConfigDescription
    {
        public Type ObjectType { get; }
        public string PropertyName { get; }
        public Type RandomValueGeneratorType { get; }

        public ConfigDescription(Type objectType, string propertyName, Type randomGeneratorType)
        {
            ObjectType = objectType;
            PropertyName = propertyName;
            RandomValueGeneratorType = randomGeneratorType;
        }
    }
}
