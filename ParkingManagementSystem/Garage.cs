using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace ParkingManagementSystem
{
    public class Garage
    {
        private List<List<IVehicle>> parkingSpaces = new List<List<IVehicle>>();
        private double availableSpaces;
        private List<IVehicle> vehicles = new List<IVehicle>();

        public static double totalIncome = 0;

        public void UpdateTotalIncome()
        {
            Console.SetCursorPosition(87, 0);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Total intäkt {totalIncome} Kr");
            Console.ResetColor();
        }

        public Garage(double numberOfParkingSpaces)
        {
            availableSpaces = numberOfParkingSpaces;

            // Lägg till tomma platser
            for (int i = 0; i < numberOfParkingSpaces; i++)
            {
                parkingSpaces.Add(new List<IVehicle>());
            }
        }

        public void AddVehicle(IVehicle vehicle, Feed feed)
        {
            bool added = false;
            string parkingTime = DateTime.Now.ToString("HH:mm:ss");

            if (vehicle is Car)
            {
                if (availableSpaces >= 1)
                {
                    parkingSpaces.Find(space => space.Count == 0)?.Add(vehicle);
                    availableSpaces -= 1;
                    added = true;
                }
                else
                {
                    Console.WriteLine("Finns inte plats för bil..");
                    Thread.Sleep(1500);
                }
            }
            else if (vehicle is Mc)
            {
                List<IVehicle> halfOccupiedSpace = parkingSpaces.Find(space => space.Count == 1 && space[0] is Mc);
                if (halfOccupiedSpace != null)
                {
                    halfOccupiedSpace.Add(vehicle);
                    availableSpaces -= 0.5;
                    added = true;
                }
                else if (availableSpaces >= 0.5)
                {
                    parkingSpaces.Find(space => space.Count == 0)?.Add(vehicle);
                    availableSpaces -= 0.5;
                    added = true;
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
                    List<IVehicle> emptySpace = parkingSpaces.Find(space => space.Count == 0);
                    emptySpace?.Add(vehicle);
                    parkingSpaces[parkingSpaces.IndexOf(emptySpace) + 1].Add(vehicle);
                    availableSpaces -= 2;
                    added = true;
                }
                else
                {
                    Console.WriteLine("Finns inte plats för buss..");
                    Thread.Sleep(1500);
                }
            }
            if (added)
            {
            vehicles.Add(vehicle);
            feed.AddMessage($"Incheckad: {vehicle.Type} - {vehicle.RegistrationNumber} - Ankomst: {parkingTime}", ConsoleColor.Green);
            }
        }

        public bool RemoveVehicle(string regNumber)
        {
            bool removed = false;

            for (int i = 0; i < parkingSpaces.Count; i++)
            {
                List<IVehicle> space = parkingSpaces[i];
                IVehicle vehicleToRemove = space.FirstOrDefault(v => v.RegistrationNumber.Equals(regNumber, StringComparison.OrdinalIgnoreCase));
                if (vehicleToRemove != null)
                {
                    space.Remove(vehicleToRemove);
                    availableSpaces += GetSpaceOccupied(vehicleToRemove);
                    vehicles.Remove(vehicleToRemove);
                    removed = true;

                    if (vehicleToRemove is Bus)
                    {
                        // Ta bort från nästa plats också
                        if (i + 1 < parkingSpaces.Count)
                        {
                            List<IVehicle> nextSpace = parkingSpaces[i + 1];
                            IVehicle busInNextSpace = nextSpace.FirstOrDefault(v => v.RegistrationNumber.Equals(regNumber, StringComparison.OrdinalIgnoreCase));
                            if (busInNextSpace != null)
                            {
                                nextSpace.Remove(busInNextSpace);
                            }
                        }
                    }
                    break;
                }
            }

            return removed;
        }

        public IVehicle GetVehicleByRegNumber(string regNumber)
        {
            return vehicles.FirstOrDefault(v => v.RegistrationNumber == regNumber);
        }

        private double GetSpaceOccupied(IVehicle vehicle)
        {
            if (vehicle is Car)
                return 1.0;

            else if (vehicle is Mc)
                return 0.5;

            else if (vehicle is Bus)
                return 2.0;

            return 0.0;
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
                List<IVehicle> vehiclesOnSpace = parkingSpaces[i];
                if (vehiclesOnSpace.Count > 0)
                {
                    foreach (IVehicle vehicle in vehiclesOnSpace)
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
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine(vehicleInfo);
                        Console.ResetColor();
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
