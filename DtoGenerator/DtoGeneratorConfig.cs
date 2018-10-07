using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace DtoGenerator
{
    public class DtoGeneratorConfig
    {
        private readonly List<ConfigDescription> _configDescriptions;

        public DtoGeneratorConfig()
        {
            _configDescriptions = new List<ConfigDescription>();
        }

        public void Add<T, ParamType, G>(Expression<Func<T, ParamType>> expression)
        {
            MemberExpression memberExpression = (MemberExpression)expression.Body;
            _configDescriptions.Add(new ConfigDescription(typeof(T), memberExpression.Member.Name, typeof(G))); 
        }

        public bool Has(Type objectType, string propertyName, out Type generator)
        {
            ConfigDescription configDescription = _configDescriptions.Find(x => x.ObjectType == objectType && x.PropertyName.Equals(propertyName, StringComparison.OrdinalIgnoreCase));

            if (configDescription != null)
            {
                generator = configDescription.RandomValueGeneratorType;
            }
            else
            {
                generator = null;
            }

            return configDescription == null ? false : true;
        }
    }
}
