using System;
namespace ParkingManagementSystem
{
    public interface IVehicle
    {
        string RegistrationNumber { get; set; }
        string Color { get; set; }
        string Type { get; }
    }

    public abstract class Vehicle : IVehicle
    {
        public string RegistrationNumber { get; set; }
        public string Color { get; set; }
        public abstract string Type { get; }

        public Vehicle(string registrationNumber, string color)
        {
            RegistrationNumber = registrationNumber;
            Color = color;
        }
    }

    public class Car : Vehicle
    {
        public bool ElectricCar { get; set; }
        public override string Type { get { return "Bil"; } }

        public Car(string registrationNumber, string color, bool electricCar) : base(registrationNumber, color)
        {
            ElectricCar = electricCar;
        }
    }

    public class Bus : Vehicle
    {
        public int NumberOfPassenger { get; set; }
        public override string Type { get { return "Buss"; } }

        public Bus(string registrationNumber, string color, int numberOfPassenger) : base(registrationNumber, color)
        {
            NumberOfPassenger = numberOfPassenger;
        }
    }

    public class Mc : Vehicle
    {
        public string Brand { get; set; }
        public override string Type { get { return "Mc"; } }

        public Mc(string registrationNumber, string color, string brand) : base(registrationNumber, color)
        {
            Brand = brand;
        }
    }

}