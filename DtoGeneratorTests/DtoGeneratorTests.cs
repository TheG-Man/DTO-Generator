using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using NUnit.Framework;
using DtoGenerator;

namespace DtoGeneratorTests
{
    public class Vehicle
    {
        public int Price { get; set; }
        public int Age { get; set; }

        public Vehicle(int price)
        {
            Price = price;
        }

        public Vehicle(int price, int age)
        {
            Price = price;
            Age = age;
        }
    }

    public class Shoes
    {
        public int Price { get; set; }
        public string Name { get; set; }
        public double Size { get; set; }
        public Trainers Sneakers { get; set; }

        public Shoes(int price)
        {
            Price = price;
        }

        public Shoes(int price, string name, double size, Trainers sneakers)
        {
            Price = price;
            Name = name;
            Size = size;
            Sneakers = sneakers;
        }

        public Shoes(int price, string name)
        {
            Price = price;
            Name = name;
        }
    }

    public class Trainers
    {
        public int Price { get; set; }
        public string Name { get; set; }
        public double Size { get; set; }
        public Shoes Boots { get; set; }

        public Trainers(int price, string name, double size, Shoes boots)
        {
            Price = price;
            Name = name;
            Size = size;
            Boots = boots;
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
    }

    [TestFixture]
    public class DtoGeneratorTests
    {
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void GetConstructorArgumentsTest()
        {
            var dtoGenerator = new DtoGenerator.DtoGenerator();
            Type t = typeof(Vehicle);

            object[] args = dtoGenerator.GetConstructorArguments(t, t.GetConstructors()[0]);

            Assert.AreEqual(1, args.Length);
            Assert.AreEqual(typeof(int), args[0].GetType());
        }

        [Test]
        public void DtoGeneratorCreateTest()
        {
            var dtoGenerator = new DtoGenerator.DtoGenerator();
            Shoes myObject = dtoGenerator.Create<Shoes>();

            Assert.AreEqual(typeof(Shoes), myObject.GetType());
        }

        [Test]
        public void DtoGeneratorCreateSeveralDtoTest()
        {
            var dtoGenerator = new DtoGenerator.DtoGenerator();
            Mix mix = dtoGenerator.Create<Mix>();

            Assert.AreEqual(typeof(Mix), mix.GetType());
            Assert.AreEqual(typeof(Vehicle), mix.Car.GetType());
        }

        [Test]
        public void DtoGeneratorCreateRecursionTest()
        {
            var dtoGenerator = new DtoGenerator.DtoGenerator();
            Shoes shoes = dtoGenerator.Create<Shoes>();

            Assert.AreEqual(null, shoes.Sneakers.Boots);
        }

        [Test]
        public void DtoGeneratorCreateObjWithoutCtorTest()
        {
            var dtoGenerator = new DtoGenerator.DtoGenerator();
            ObjWithoutCtor obj = dtoGenerator.Create<ObjWithoutCtor>();

            Assert.AreNotEqual(0, obj.Value);
        }

        [Test]
        public void DtoGeneratorConfigTest()
        {
            var dtoGeneratorConfig = new DtoGeneratorConfig();
            dtoGeneratorConfig.Add<Vehicle, int, AgeGenerator>(car => car.Age);
            var dtoGenerator = new DtoGenerator.DtoGenerator(dtoGeneratorConfig);

            var vehicle = dtoGenerator.Create<Vehicle>();

            Assert.GreaterOrEqual(vehicle.Age, 1);
            Assert.LessOrEqual(vehicle.Age, 46);
        }
    }
}
