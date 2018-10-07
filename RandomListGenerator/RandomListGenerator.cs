using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using DtoGenerator;
using RandomDateTimeGenerator;

namespace RandomIEnumerableGenerator
{
    public class RandomListGenerator: IRandomValueGenerator
    {
        private readonly SupportedTypes _supportedTypes = new SupportedTypes();

        public string Type
        {
            get { return "List"; }
        }

        public RandomListGenerator()
        {
            
        }

        public object Generate<T>()
        {
            T enumObj;
            if (!typeof(T).IsInterface && !typeof(T).IsAbstract)
            {
                enumObj = (T)Activator.CreateInstance(typeof(T));
                Type genericTypeArgument = typeof(T).GetGenericArguments().First();
                
                for (int i = 0; i < 10; ++i)
                {
                    object argument = new object();
                    string typeName;

                    if (_supportedTypes.Has(genericTypeArgument, out typeName))
                    {
                        switch (typeName)
                        {
                            case "Byte":
                                IRandomValueGenerator byteGenerator = new RandomByteGenerator();
                                argument = byteGenerator.Generate<T>();
                                break;
                            case "Double":
                                IRandomValueGenerator doubleGenerator = new RandomDoubleGenerator();
                                argument = doubleGenerator.Generate<T>();
                                break;
                            case "Boolean":
                                IRandomValueGenerator boolGenerator = new RandomBoolGenerator();
                                argument = boolGenerator.Generate<T>();
                                break;
                            case "String":
                                IRandomValueGenerator stringGenerator = new RandomStringGenerator();
                                argument = stringGenerator.Generate<T>();
                                break;
                            case "DateTime":
                                IRandomValueGenerator dateTimeGenerator = new RandomDateTimeGenerator.RandomDateTimeGenerator();
                                argument = dateTimeGenerator.Generate<T>();
                                break;
                        }

                        MethodInfo method = typeof(T).GetMethod("Add");
                        method.Invoke(enumObj, new object[] { argument });
                    }
                    else
                    {
                        throw new Exception();
                    }
                }

                return enumObj;
            }

            return null;
        }
    }
}
