using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using DtoGenerator;

namespace DtoGeneratorProgram
{
    public class Vehicle
    {
        private readonly string _owner;
        private readonly int _doors;

        public int Price { get; set; }
        public int Age { get; }
        public VehiclePart Engine { get; set; }

        public Vehicle(int price)
        {
            Price = price;
        }

        public Vehicle(int price, int age, string owner, int doors, VehiclePart engine)
        {
            Price = price;
            Age = age;
            Engine = engine;

            _owner = owner;
            _doors = doors;
        }
    }

    public class VehiclePart
    {
        private readonly string _color;

        public int Price { get; set; }
        public string Name { get; set; }
        public double Volume { get; set; }
        public Vehicle Car { get; set; }

        public VehiclePart(int price)
        {
            Price = price;
        }

        public VehiclePart(int price, string name, double volume, string color, Vehicle car)
        {
            Price = price;
            Name = name;
            Volume = volume;
            Car = car;

            _color = color;
        }

        public VehiclePart(int price, string name)
        {
            Price = price;
            Name = name;
        }
    }

    public class Mix
    {
        private readonly VehiclePart _vehiclePart;
        public Vehicle Car { get; set; } 

        public Mix(Vehicle car, VehiclePart vehiclePart)
        {
            Car = car;
            _vehiclePart = vehiclePart;
        }
    }

    public class ObjWithoutCtor
    {
        public int Value { get; set; }
        public List<DateTime> Prices { get; set; }
    }

    class Program
    {
        private static readonly RandomGenerator _randomValueGenerator = RandomGenerator.GetInstance();
        private static readonly SupportedTypes _valueTypes = new SupportedTypes();

        static void Main(string[] args)
        {
            try
            {
                //var obj = _dtoGenerator.Create<ObjWithoutCtor>();

                //foreach (var i in obj.Prices)
                  //  Console.WriteLine(i);
                DtoGeneratorCreateTest();
            }
            catch (PluginLoadException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (TypeNotSupportedException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (RandomValueGeneratorNotFoundException e)
            {
                Console.WriteLine(e.Message);
            }
            Console.ReadKey();
        }

        public static void DtoGeneratorCreateTest()
        {
            var dtoGeneratorConfig = new DtoGeneratorConfig();
            dtoGeneratorConfig.Add<Vehicle, int, AgeGenerator>(car => car.Age);
            dtoGeneratorConfig.Add<Vehicle, int, AgeGenerator>(car => car.Price);
            dtoGeneratorConfig.Add<VehiclePart, int, AgeGenerator>(vp => vp.Price);

            var dtoGenerator = new DtoGenerator.DtoGenerator(dtoGeneratorConfig);

            var myObject = dtoGenerator.Create<Mix>();

            Console.WriteLine("{0}: [{1}]", myObject.GetType().Name, myObject);
            PrintObjectInfo(myObject, 1);
        }

        private static void PrintObjectInfo(object obj, int level)
        {
            if (obj != null)
            {
                PrintFields(obj, level);
                PrintProperties(obj, level);
            }
        }

        private static void PrintProperties(object myObject, int level)
        {
            foreach (PropertyInfo property in myObject.GetType().GetProperties())
            {
                try
                {
                    if (property.CanRead && property.GetGetMethod().GetParameters().Length == 0)
                    {
                        object propertyValue = property.GetValue(myObject);
                        Console.WriteLine(new string(' ', level * 3) + "[Property] {0} {1}: {2}", property.PropertyType.Name, property.Name, propertyValue == null ? "null" : propertyValue);
                    }

                    if (!_valueTypes.Has(property.PropertyType))
                        if (property.GetValue(myObject) != null && property.GetValue(myObject).GetType() != myObject.GetType())
                            PrintObjectInfo(property.GetValue(myObject), ++level);
                }
                catch (Exception)
                {

                }
            }
        }

        private static void PrintFields(object myObject, int level)
        {
            foreach (FieldInfo field in myObject.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public))
            {
                try
                {
                    if (field.IsInitOnly)
                    {
                        object fieldValue = field.GetValue(myObject);
                        Console.WriteLine(new string(' ', level * 3) + "[Field] {0} {1}: {2}", field.FieldType.Name, field.Name, fieldValue == null ? "null" : fieldValue);

                        if (!_valueTypes.Has(field.FieldType))
                            if (field.GetValue(myObject) != null && field.GetValue(myObject).GetType() != myObject.GetType())
                                PrintObjectInfo(field.GetValue(myObject), ++level);
                    }
                }
                catch (Exception)
                {
                    
                }
            }
        }
    }
}
