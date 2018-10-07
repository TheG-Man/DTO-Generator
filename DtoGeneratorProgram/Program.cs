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
        public int Price { get; set; }
        public int Age { get; set; }
        public Shoes Boots { get; set; }

        public Vehicle(int price)
        {
            Price = price;
        }

        public Vehicle(int price, int age, Shoes boots)
        {
            Price = price;
            Age = age;
            Boots = boots;
        }
    }

    public class Shoes
    {
        public int Price { get; set; }
        public string Name { get; set; }
        public double Size { get; set; }
        public Vehicle Car { get; set; }

        public Shoes(int price)
        {
            Price = price;
        }

        public Shoes(int price, string name, double size, Vehicle car)
        {
            Price = price;
            Name = name;
            Size = size;
            Car = car;
        }

        public Shoes(int price, string name)
        {
            Price = price;
            Name = name;
        }
    }

    public class Mix
    {
        public Vehicle Car { get; set; }

        public Mix(Vehicle car)
        {
            Car = car;
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
            dtoGeneratorConfig.Add<Vehicle, int, AgeGenerator>( car => car.Age );
            var dtoGenerator = new DtoGenerator.DtoGenerator(dtoGeneratorConfig);

            var myObject = dtoGenerator.Create<Vehicle>();

            if (myObject != null)
            {
                Console.WriteLine("{0}: [{1}]", myObject.GetType().Name, myObject);
                PrintProperties(myObject);
            }
        }

        private static void PrintProperties(object myObject)
        { 
            Console.WriteLine("Properties:");

            foreach (PropertyInfo property in myObject.GetType().GetProperties())
            {
                try
                {
                    if (property.CanRead && property.GetGetMethod().GetParameters().Length == 0)
                        Console.WriteLine("\t{0}: {1}", property.Name, property.GetValue(myObject));
                
                    if (!_valueTypes.Has(property.PropertyType))
                        if (property.GetValue(myObject) != null && property.GetValue(myObject).GetType() != myObject.GetType())
                            PrintProperties(property.GetValue(myObject));
                    }
                catch (Exception)
                {

                }
            }
        }
    }
}
