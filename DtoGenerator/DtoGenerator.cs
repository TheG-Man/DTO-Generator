using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace DtoGenerator
{
    public class DtoGenerator: IDtoGenerator
    {
        private readonly DtoGeneratorConfig _dtoGeneratorConfig = new DtoGeneratorConfig();
        private static readonly RandomGenerator _randomGenerator = RandomGenerator.GetInstance();
        private readonly SupportedTypes _supportedTypes = new SupportedTypes();
        private readonly List<Type> _objectTypesToCreate;

        public DtoGenerator()
        {
            _objectTypesToCreate = new List<Type>();
        }

        public DtoGenerator(DtoGeneratorConfig config)
        {
            _objectTypesToCreate = new List<Type>();
            _dtoGeneratorConfig = config;
        }

        public T Create<T>()
        {
            Type objectType = typeof(T);
            object dto = null;

            if (_supportedTypes.Has(objectType))
            {
                dto = CreateValueTypeObject(objectType, null);
            }
            else
            {
                if (!objectType.IsAbstract && !objectType.IsInterface && !objectType.IsValueType)
                {
                    _objectTypesToCreate.Add(objectType);
                    IOrderedEnumerable<ConstructorInfo> ctors = objectType.GetConstructors().OrderByDescending(x => x.GetParameters().Length);

                    if (ctors.Count() > 0 && ctors.First().GetParameters().Length > 0)
                    {
                        dto = CreateObjectViaConstructor(objectType, ctors.First());
                    }
                    else
                    {
                        dto = System.Activator.CreateInstance(objectType);
                        InitializeObjectProperties(dto);
                        InitializeObjectFields(dto);
                    }
                }
                else
                {
                    return default(T);
                }
            }

            _objectTypesToCreate.Remove(objectType);

            return (T)dto;
        }

        private object CreateObjectViaConstructor(Type dtoType, ConstructorInfo ctor)
        {
            object[] constructorArgs = GetConstructorArguments(dtoType, ctor);
            
            return ctor.Invoke(constructorArgs);
        }

        public void InitializeObjectProperties(object dto)
        {
            foreach (PropertyInfo property in dto.GetType().GetProperties())
            {
                if (property.SetMethod.IsPublic)
                {
                    property.SetValue(dto, GetObject(dto.GetType(), property.PropertyType, property.Name));
                }
            }
        }

        public void InitializeObjectFields(object dto)
        {
            foreach (FieldInfo field in dto.GetType().GetFields())
            {
                if (field.IsPublic)
                {
                    field.SetValue(dto, GetObject(dto.GetType(), field.FieldType, field.Name));
                }
            }
        }

        public object[] GetConstructorArguments(Type dtoType, ConstructorInfo ctor)
        { 
            object[] args = new object[ctor.GetParameters().Length];

            for (int i = 0; i < ctor.GetParameters().Length; ++i)
            {
                ParameterInfo parameter = ctor.GetParameters()[i];
                args[i] = GetObject(dtoType, parameter.ParameterType, parameter.Name);                               
            }

            return args;
        }

        private object GetObject(Type dtoType, Type objectType, string name)
        {
            //TODO: rename the function (something like GetObjectToInitialize)
            object obj = new object();

            if (_supportedTypes.Has(objectType))
            {
                Type generatorType;
                _dtoGeneratorConfig.Has(dtoType, name, out generatorType);
                obj = CreateValueTypeObject(objectType, generatorType);
            }
            else
            {
                if (!_objectTypesToCreate.Contains(objectType))
                {
                    obj = CreateDtoTypeObject(objectType);
                }
                else
                {
                    obj = null;
                }
            }

            return obj;
        }

        private object CreateValueTypeObject(Type objectType, Type generatorType)
        {
            Type openType = typeof(RandomGenerator);
            MethodInfo method = openType.GetMethod("Generate");
            MethodInfo genericMethod = method.MakeGenericMethod(new Type[] { objectType });

            return genericMethod.Invoke(_randomGenerator, new object[] { generatorType });
        }

        private object CreateDtoTypeObject(Type type)
        {
            Type openType = typeof(DtoGenerator);
            MethodInfo method = openType.GetMethod("Create");
            MethodInfo genericMethod = method.MakeGenericMethod(new Type[] { type });

            return genericMethod.Invoke(this, null);
        }
    }
}
