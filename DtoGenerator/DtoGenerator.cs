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
        private static volatile DtoGenerator _instance;
        private static readonly object _syncRoot = new object();
        private static readonly RandomGenerator _randomGenerator = RandomGenerator.GetInstance();
        private readonly ValueTypes _valueTypes = new ValueTypes();
        private readonly List<Type> _objectTypesToCreate;

        private DtoGenerator()
        {
            _objectTypesToCreate = new List<Type>();
        }

        public static DtoGenerator GetInstance()
        {
            if (_instance == null)
            {
                lock (_syncRoot)
                {
                    if (_instance == null)
                    {
                        _instance = new DtoGenerator();
                    }
                }
            }

            return _instance;
        }

        public T Create<T>()
        {
            Type objectType = typeof(T);
            object dto = null;

            if (_valueTypes.Has(objectType))
            {
                dto = CreateValueTypeObject(objectType);
            }
            else
            {
                _objectTypesToCreate.Add(objectType);
                IOrderedEnumerable<ConstructorInfo> ctors = objectType.GetConstructors().OrderByDescending(x => x.GetParameters().Length);

                if (ctors.Count() > 0 && ctors.First().GetParameters().Length > 0)
                {
                    dto = CreateObjectViaConstructor(ctors.First());
                }
                else
                {
                    dto = System.Activator.CreateInstance(objectType);
                    InitializeObjectProperties(dto);
                    InitializeObjectFields(dto);
                }
            }

            _objectTypesToCreate.Remove(objectType);

            return (T)dto;
        }

        private object CreateObjectViaConstructor(ConstructorInfo ctor)
        {
            object[] constructorArgs = GetConstructorArguments(ctor);
            
            return ctor.Invoke(constructorArgs);
        }

        public void InitializeObjectProperties(object dto)
        {
            foreach (PropertyInfo property in dto.GetType().GetProperties())
            {
                if (property.SetMethod.IsPublic)
                {
                    property.SetValue(dto, GetObject(property.PropertyType));
                }
            }
        }

        public void InitializeObjectFields(object dto)
        {
            foreach (FieldInfo field in dto.GetType().GetFields())
            {
                if (field.IsPublic)
                {
                    field.SetValue(dto, GetObject(field.FieldType));
                }
            }
        }

        public object[] GetConstructorArguments(ConstructorInfo ctor)
        { 
            object[] args = new object[ctor.GetParameters().Length];

            for (int i = 0; i < ctor.GetParameters().Length; ++i)
            {
                ParameterInfo parameter = ctor.GetParameters()[i];
                args[i] = GetObject(parameter.ParameterType);                               
            }

            return args;
        }

        private object GetObject(Type type)
        {
            //TODO: rename the function (something like GetObjectToInitialize)
            object obj = new object();

            if (_valueTypes.Has(type))
            {
                obj = CreateValueTypeObject(type);
            }
            else
            {
                //Add handler which can handle undefined types.......
                if (!_objectTypesToCreate.Contains(type))
                {
                    obj = CreateDtoTypeObject(type);
                }
                else
                {
                    obj = null;
                }
            }

            return obj;
        }

        private object CreateValueTypeObject(Type type)
        {
            Type openType = typeof(RandomGenerator);
            MethodInfo method = openType.GetMethod("Generate");
            MethodInfo genericMethod = method.MakeGenericMethod(new Type[] { type });

            return genericMethod.Invoke(_randomGenerator, null);
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
