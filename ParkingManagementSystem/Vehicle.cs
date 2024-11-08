using System;
namespace ParkingManagementSystem
{
    public class Vehicle
    {
        public string RegistrationNumber { get; set; }
        public string Color { get; set; }
        public virtual string Type { get { return "Okänt fordon"; } }
        //public DateTime StartTime { get; set; } // todo
        //public double PricePerMinute { get; set; } = 1.5;

        public Vehicle(string registrationNumber, string color/*, double pricePerMinute*/)
        {
            RegistrationNumber = registrationNumber;
            Color = color;
            //StartTime = DateTime.Now;
            //PricePerMinute = pricePerMinute;
        }

        //public void Checkout()
        //{
        //    //double totalMinutes = (DateTime.Now - StartTime).TotalMinutes; // <-- Beräkna total parkeringstid
        //    //double totalPrice = totalMinutes * PricePerMinute;             // <-- Räkna ut total kostnad
        //    //Console.WriteLine($"Total parkeringstid: {totalMinutes:F1} minuter");
        //    //Console.WriteLine($"Pris för parkering: {totalPrice:F2} kr");
        //}

    }

    class Car : Vehicle
    {
        public bool ElectricCar { get; set; }
        public override string Type { get { return "Bil"; } }

        public Car(string registrationNumber, string color, bool electricCar/*, double pricePerMinuetes*/) : base(registrationNumber, color/*,pricePerMinute*/)
        {
            ElectricCar = electricCar;
        }

    }

    class Bus : Vehicle
    {
        public int NumberOfPassenger { get; set; }
        public override string Type { get { return "Buss"; } }

        public Bus(string registrationNumber, string color, int numberOfPassenger) : base(registrationNumber, color) //todo
        {
            NumberOfPassenger = numberOfPassenger;
        }

    }

    class Mc : Vehicle
    {
        public string Brand { get; set; }
        public override string Type { get { return "Mc"; } }

        public Mc(string registrationNumber, string color, string brand) : base(registrationNumber, color)
        {
            Brand = brand;
        }
    }
}

