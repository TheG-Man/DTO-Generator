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
        private readonly string _owner;
        private readonly int _doors;

        public int Price { get; set; }
        public int Age { get; set; }
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
    }

    [TestFixture]
    public class DtoGeneratorTests
    {
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void DtoGeneratorCreateObjViaCtorTest()
        {
            var dtoGenerator = new DtoGenerator.DtoGenerator();
            Vehicle vehicle = dtoGenerator.Create<Vehicle>();

            Assert.AreEqual(typeof(Vehicle), vehicle.GetType());
        }

        [Test]
        public void DtoGeneratorCreateSeveralDtoTest()
        {
            var dtoGenerator = new DtoGenerator.DtoGenerator();
            Mix mix = dtoGenerator.Create<Mix>();

            Assert.AreEqual(typeof(Mix), mix.GetType());
            Assert.AreEqual(typeof(Vehicle), mix.Car.GetType());
            Assert.AreEqual(typeof(VehiclePart), mix.Car.Engine.GetType());
        }

        [Test]
        public void DtoGeneratorCreateRecursionTest()
        {
            var dtoGenerator = new DtoGenerator.DtoGenerator();
            Vehicle vehicle = dtoGenerator.Create<Vehicle>();

            Assert.AreEqual(null, vehicle.Engine.Car);
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
