using System;
using System.Collections.Generic;

namespace ParkingManagementSystem
{
    public class Garage
    {
        private List<List<Vehicle>> parkingSpaces = new List<List<Vehicle>>();
        private double availableSpaces;

        private List<Vehicle> vehicles = new List<Vehicle>();

        public Garage(double numberOfParkingSpaces)
        {
            availableSpaces = numberOfParkingSpaces;

            // Kolla tomma platser
            for (int i = 0; i < numberOfParkingSpaces; i++)
            {
                parkingSpaces.Add(new List<Vehicle>());
            }
        }

        public void AddVehicle(Vehicle vehicle)
        {

            if (vehicle is Car)
            {
                if (availableSpaces >= 1)
                {
                    parkingSpaces.Find(space => space.Count == 0)?.Add(vehicle);
                    availableSpaces -= 1;
                }
                else
                {
                    Console.WriteLine("Finns inte plats för bil..");
                    Thread.Sleep(1500);
                }
            }
            else if (vehicle is Mc)
            {
                List<Vehicle> halfOccupiedSpace = parkingSpaces.Find(space => space.Count == 1 && space[0] is Mc);
                if (halfOccupiedSpace != null)
                {
                    halfOccupiedSpace.Add(vehicle); // Lägg till andra motorcykeln i samma plats
                    availableSpaces -= 0.5; // samma som availableSpaces = availableSpaces - 0.5;
                }
                else if (availableSpaces >= 0.5)
                {
                    parkingSpaces.Find(space => space.Count == 0)?.Add(vehicle); // Lägg till i en ny ledig plats
                    availableSpaces -= 0.5;
                }
                else
                {
                    Console.WriteLine("Finns inte plats för motorcykel..");
                    Thread.Sleep(1500);
                }
            }
            else if (vehicle is Bus)
            {
                if (availableSpaces >= 2)
                {
                    List<Vehicle> emptySpace = parkingSpaces.Find(space => space.Count == 0);
                    emptySpace?.Add(vehicle); // Buss använder två platser
                    parkingSpaces[parkingSpaces.IndexOf(emptySpace) + 1].Add(vehicle); // Placera på nästa plats också
                    availableSpaces -= 2;
                }
                else
                {
                    Console.WriteLine("Finns inte plats för buss..");
                    Thread.Sleep(1500);
                }
            }
            vehicles.Add(vehicle);
        }
        public bool RemoveVehicle(string regNumber)
        {
            bool removed = false;

            for (int i = 0; i < parkingSpaces.Count; i++)
            {
                List<Vehicle> space = parkingSpaces[i];
                Vehicle vehicleToRemove = space.FirstOrDefault(v => v.RegistrationNumber.Equals(regNumber, StringComparison.OrdinalIgnoreCase)); // lambda-funktion
                if (vehicleToRemove != null)
                {
                    space.Remove(vehicleToRemove);
                    availableSpaces += GetSpaceOccupied(vehicleToRemove);
                    vehicles.Remove(vehicleToRemove); //todo nytt
                    removed = true;

                    if (vehicleToRemove is Bus)
                    {
                        // Ta bort från nästa plats också
                        if (i + 1 < parkingSpaces.Count)
                        {
                            List<Vehicle> nextSpace = parkingSpaces[i + 1];
                            Vehicle busInNextSpace = nextSpace.FirstOrDefault(v => v.RegistrationNumber.Equals(regNumber, StringComparison.OrdinalIgnoreCase));
                            if (busInNextSpace != null)
                            {
                                nextSpace.Remove(busInNextSpace);
                                availableSpaces += GetSpaceOccupied(busInNextSpace);
                            }
                        }
                    }
                }
            }

            if (removed)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Vehicle GetVehicleByRegNumber(string regNumber)
        {
            return vehicles.FirstOrDefault(v => v.RegistrationNumber == regNumber);
        }

        private double GetSpaceOccupied(Vehicle vehicle)
        {
            if (vehicle is Car)
            {
                return 1.0;
            }
            else if (vehicle is Mc)
            {
                return 0.5;
            }
            else if (vehicle is Bus)
            {
                return 1.0;
            }
            return 0.0; // Om fordonet är av okänt typ
        }

        public int ShowParkingSpaces()
        {
            Console.SetCursorPosition(26, 0);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Antal lediga parkeringsplatser: " + availableSpaces);
            Console.ResetColor();
            Console.Write(new string('=', 60));
            Console.WriteLine('=');

            int usedRows = 0;

            for (int i = 0; i < parkingSpaces.Count; i++)
            {
                List<Vehicle> vehiclesOnSpace = parkingSpaces[i];
                if (vehiclesOnSpace.Count > 0)
                {
                    foreach (Vehicle vehicle in vehiclesOnSpace)
                    {
                        Console.Write(" Plats " + (i + 1) + ":\t");
                        string vehicleInfo = vehicle.Type + "\t" + vehicle.RegistrationNumber + "\t" + vehicle.Color + "\t";
                        if (vehicle is Car car)
                        {
                            vehicleInfo += "Eldriven: " + (car.ElectricCar ? "Ja" : "Nej");
                        }
                        else if (vehicle is Mc mc)
                        {
                            vehicleInfo += "Märke: " + mc.Brand;
                        }
                        else if (vehicle is Bus bus)
                        {
                            vehicleInfo += "Antal Passagerare: " + bus.NumberOfPassenger;
                        }
                        Console.WriteLine(vehicleInfo);
                        usedRows++;
                    }
                }
                else
                {
                    Console.Write(" Plats " + (i + 1) + ":\t");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("Ledig");
                    Console.WriteLine();
                    Console.ResetColor();
                    usedRows++;
                }
            }
            Console.Write(new string('=', 60));
            Console.WriteLine('=');

            return usedRows;
        }
    }
}
