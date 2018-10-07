using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;

namespace DtoGenerator
{
    public sealed class RandomGenerator
    {
        private static volatile RandomGenerator _instance;
        private static readonly object _syncRoot = new object();
        private readonly List<IRandomValueGenerator> _randomGenerators;
        private readonly SupportedTypes _supportedTypes = new SupportedTypes();

        private RandomGenerator()
        {
            _randomGenerators = new List<IRandomValueGenerator>();
            _randomGenerators.Add(new RandomByteGenerator());
            _randomGenerators.Add(new RandomDoubleGenerator());
            _randomGenerators.Add(new RandomBoolGenerator());
            _randomGenerators.Add(new RandomStringGenerator());
            _randomGenerators.Add(new AgeGenerator());

            LoadExternalRandomGenerators();
        }

        public static RandomGenerator GetInstance()
        {
            if (_instance == null)
            {
                lock (_syncRoot)
                {
                    if (_instance == null)
                    {
                        _instance = new RandomGenerator();
                    }
                }
            }

            return _instance;
        }

        public T Generate<T>(Type generatorType)
        {
            string typeName;
            object randomValue = new object();

            if (_supportedTypes.Has(typeof(T), out typeName))
            {
                IRandomValueGenerator randomValueGenerator;

                if (generatorType == null)
                    randomValueGenerator = _randomGenerators.Find( x => x.Type == typeName );
                else
                    randomValueGenerator = _randomGenerators.Find(x => x.GetType() == generatorType);

                if (randomValueGenerator != null)
                    randomValue = randomValueGenerator.Generate<T>();
                else
                    throw new RandomValueGeneratorNotFoundException("There is not generator for [" + typeof(T).Name + "]");
            }
            else
            {            
                throw new TypeNotSupportedException("The type [" + typeof(T).Name + "] is not supported.");
            }
            
            return (T)Convert.ChangeType(randomValue, typeof(T));
        }

        private void LoadExternalRandomGenerators()
        {
            foreach (string file in Directory.EnumerateFiles(Directory.GetCurrentDirectory()))
            {
                try
                {
                    Assembly assembly = Assembly.LoadFrom(file);
                    Type[] exportedTypes = assembly.GetExportedTypes();

                    Type[] randomGeneraterTypes = Array.FindAll(exportedTypes,
                        x => typeof(IRandomValueGenerator).IsAssignableFrom(x) && !x.IsAbstract);

                    foreach (Type randomGeneratorType in randomGeneraterTypes)
                    {
                        IRandomValueGenerator randomValueGenerator = (IRandomValueGenerator)Activator.CreateInstance(randomGeneratorType);
                        _randomGenerators.Add(randomValueGenerator);
                    }
                }
                catch (Exception)
                {
                    //throw new PluginLoadException(e.Message);
                }
            }
        }
    }
}
