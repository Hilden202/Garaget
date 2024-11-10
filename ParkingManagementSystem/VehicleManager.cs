using System;
using System.Collections.Generic;

namespace ParkingManagementSystem
{
    public class VehicleManager
    {
        private static List<string> registeredVehicles = new List<string>();

        public static void CheckinVehicle(Garage garage, Feed feed)
        {
            Random random = new Random();

            Console.Write("\nAnge typ av fordon: ");
            string vehicleType = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(vehicleType))
            {
                string[] vehicleTypes = { "car", "bus", "mc" };
                vehicleType = vehicleTypes[random.Next(vehicleTypes.Length)];
                Console.WriteLine("Slumpmässigt fordonstyp: " + vehicleType);
            }

            if (vehicleType.ToLower() == "car" || vehicleType.ToLower() == "bil")
            {
                string regNumber = RequestRegistrationNumber("Bilen");

                string vehicleColor = RequestVehicleColor("Bilen");

                bool isElectric = RequestIfElectric();

                Car car = new Car(regNumber, vehicleColor, isElectric);

                garage.AddVehicle(car);

                CheckinVehicleToList(regNumber);

                feed.AddMessage($"Incheckad: Bil: {regNumber}", ConsoleColor.Green); // Lägg till meddelande i feed
            }

            else if (vehicleType.ToLower() == "bus" || vehicleType.ToLower() == "buss")
            {
                string regNumber = RequestRegistrationNumber("bussen");

                string vehicleColor = RequestVehicleColor("Bussen");

                int passengerCount = RequestNumberOfPassengers();

                Bus bus = new Bus(regNumber, vehicleColor, passengerCount);

                garage.AddVehicle(bus);

                CheckinVehicleToList(regNumber);

                feed.AddMessage($"Incheckad: Buss: {regNumber}", ConsoleColor.Green);
            }

            else if (vehicleType.ToLower() == "mc" || vehicleType.ToLower() == "motorcyckel" || vehicleType.ToLower() == "motorbike" || vehicleType.ToLower() == "motorcycle")
            {
                string regNumber = RequestRegistrationNumber("Motocyckeln");

                string vehicleColor = RequestVehicleColor("motorcyckeln");

                string brandChoice = RequestMotorcycleBrand();

                Mc mc = new Mc(regNumber, vehicleColor, brandChoice);

                garage.AddVehicle(mc);

                CheckinVehicleToList(regNumber);

                feed.AddMessage($"Incheckad: Mc: {regNumber}", ConsoleColor.Green);
            }
        }

        public static void CheckoutVehicle(Garage garage, Feed feed)
        {
            Console.Write("\nAnge registreringsnummer på det fordon du vill checka ut: ");
            string regNumber = Console.ReadLine()?.ToUpper(); //todo ?

            if (string.IsNullOrWhiteSpace(regNumber))
            {
                Console.WriteLine();
                IVehicle vehicle = CheckoutRandomVehicleFromList(garage);

                if (vehicle != null)
                {
                    feed.AddMessage($"Utcheckad: {vehicle.Type}: {vehicle.RegistrationNumber}", ConsoleColor.Red);
                }
                else
                {
                    Console.WriteLine("Inga fordon tillgängliga för slumpmässig utcheckning.");
                }
                Console.ReadKey();
                return;
            }

            if (regNumber.Length != 6)
            {
                Console.WriteLine("Ogiltigt registreringsnummer, ange 6 tecken.");
                Console.ReadKey();
                return;
            }

            if (registeredVehicles.Contains(regNumber))
            {
                IVehicle vehicle = garage.GetVehicleByRegNumber(regNumber);

                if (vehicle != null)
                {
                    feed.AddMessage($"Utcheckad: {vehicle.Type}: {vehicle.RegistrationNumber}", ConsoleColor.Red);
                    bool removed = garage.RemoveVehicle(regNumber);

                    if (removed)
                    {
                        registeredVehicles.Remove(regNumber);
                        Console.WriteLine($"Fordon med registreringsnummer {regNumber} har checkats ut.");
                    }
                    else
                    {
                        Console.WriteLine($"Fordon med registreringsnummer {regNumber} kunde inte tas bort från garaget.");
                    }
                }
                else
                {
                    Console.WriteLine($"Fordon med registreringsnummer {regNumber} hittades inte.");
                }
            }
            else
            {
                Console.WriteLine($"Fordon med registreringsnummer {regNumber} är inte incheckat.");
            }
            Console.ReadKey();
        }

        public static string RequestRegistrationNumber(string vehicleType)
        {
            Random random = new Random();

            while (true)
            {
                Console.Write("\nAnge registreringsnummer på " + vehicleType + ":");
                string regNumber = Console.ReadLine().ToUpper();

                if (string.IsNullOrWhiteSpace(regNumber))
                {
                    regNumber = GenerateRandomRegistrationNumber(random);
                    Console.WriteLine("Slumpmässigt registreringsnummer: " + regNumber);
                }

                if (regNumber.Length == 6)
                {
                    return regNumber.ToUpper();
                }
                else
                {
                    Console.WriteLine("Ogiltigt registreringsnummer, ange 6 tecken.");
                }
            }
        }

        private static string GenerateRandomRegistrationNumber(Random random)
        {
            // Skapa en sträng för att lagra registreringsnumret
            char[] regNumber = new char[6];

            // Lägg till tre slumpmässiga bokstäver
            for (int i = 0; i < 3; i++)
            {
                regNumber[i] = (char)('A' + random.Next(26)); // Värden mellan A och Z
            }
            // Lägg till tre slumpmässiga siffror
            for (int i = 3; i < 6; i++)
            {
                regNumber[i] = (char)('0' + random.Next(10)); // Värden mellan 0 och 9
            }

            return new string(regNumber);
        }

        public static string RequestVehicleColor(string vehicleType)
        {
            Random random = new Random();
            string[] colors = { "RÖD", "BLÅ", "GRÖN", "GUL", "SVART" };

            while (true)
            {
                Console.Write("\nAnge färg på " + vehicleType + ": ");
                string vehicleColor = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(vehicleColor))
                {
                    vehicleColor = colors[random.Next(colors.Length)];
                    Console.WriteLine("Slumpmässigt färg valt: " + vehicleColor);
                    return vehicleColor;
                }

                if (Array.Exists(colors, color => color.Equals(vehicleColor.ToUpper())))
                {
                    Console.WriteLine("Färg vald: " + vehicleColor);
                    return vehicleColor.ToUpperInvariant();
                }
                else
                {
                    Console.WriteLine("Ogiltig färg. Ange en giltig färg.");
                }
            }
        }

        public static bool RequestIfElectric()
        {
            Random random = new Random();
            bool isElectricRoll = random.Next(2) == 1;

            while (true)
            {
                Console.Write("\nÄr bilen en Elbil? ja/nej: ");
                string isElectric = Console.ReadLine().ToLower();
                if (string.IsNullOrWhiteSpace(isElectric))
                {
                    Console.WriteLine("Slumpmässigt roll: " + (isElectricRoll ? "Ja" : "Nej"));
                    Thread.Sleep(1500);
                    return isElectricRoll;
                }
                if (isElectric == "ja")
                {
                    return true;
                }
                else if (isElectric == "nej")
                {
                    return false;
                }
                else
                {
                    Console.WriteLine("Ogiltigt svar. Vänligen ange 'ja' eller 'nej'.");
                }
            }
        }

        public static int RequestNumberOfPassengers()
        {
            Random random = new Random();
            int randomPassengerCount = random.Next(20, 60);

            while (true)
            {
                Console.Write("\nHur många passagerare rymmer bussen? ");
                string passengerCount = Console.ReadLine();
                int passengerCountInt;

                if (string.IsNullOrWhiteSpace(passengerCount))
                {
                    Console.WriteLine("Slumpmässigt antal passagerare: " + randomPassengerCount);
                    Thread.Sleep(1500);
                    return randomPassengerCount;
                }
                if (int.TryParse(passengerCount, out passengerCountInt))
                {
                    return passengerCountInt;
                }
                else
                {
                    Console.WriteLine("Ogiltig inmatning. Ange ett heltal.");

                }
            }
        }

        public static string RequestMotorcycleBrand()
        {
            Random random = new Random();
            string[] acceptedBrands = { "Honda", "Yamaha", "Kawasaki", "Ducati", "Suzuki", "BMW", "Harley-Davidson", "KTM", "Triumph", };
            string randomBrand = acceptedBrands[random.Next(acceptedBrands.Length)];
            while (true)
            {
                Console.Write("\nAnge märket på motorcykeln: ");
                string brandChoice = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(brandChoice))
                {
                    Console.WriteLine("Slumpmässigt märke: " + randomBrand);
                    Thread.Sleep(1500);
                    return randomBrand;
                }
                if (Array.Exists(acceptedBrands, brand => brand.Equals(brandChoice, StringComparison.OrdinalIgnoreCase)))
                {
                    return brandChoice;
                }
                else
                {
                    Console.WriteLine("Ogiltig inmatning. Ange ett giltigt märkesnamn.");
                }
            }
        }

        public static void CheckinVehicleToList(string regNumber)
        {
            if (!registeredVehicles.Contains(regNumber))
            {
                registeredVehicles.Add(regNumber);
                Console.WriteLine($"Fordon med registreringsnummer {regNumber} har checkats in.");
            }
            else
            {
                Console.WriteLine($"Fordon med registreringsnummer {regNumber} är redan incheckat.");
            }
        }

        public static IVehicle CheckoutRandomVehicleFromList(Garage garage)
        {
            if (registeredVehicles.Count > 0)
            {
                Random random = new Random();
                string randomRegNumber = registeredVehicles[random.Next(registeredVehicles.Count)];

                IVehicle vehicle = garage.GetVehicleByRegNumber(randomRegNumber);

                if (vehicle != null && garage.RemoveVehicle(randomRegNumber))
                {
                    registeredVehicles.Remove(randomRegNumber);

                    // Beräkna parkeringstiden i minuter och kostnaden
                    //double parkedMinutes = (DateTime.Now - vehicle.StartTime).TotalMinutes;
                    //double totalCost = Math.Round(parkedMinutes * 1.5, 2); // Runda av till två decimaler

                    Console.WriteLine($"Slumpmässigt fordon med registreringsnummer {randomRegNumber} har checkats ut.");
                    //Console.WriteLine($"Parkeringstid: {parkedMinutes:F2} minuter, Totalkostnad: {totalCost} SEK");

                    return vehicle;  // Returnera det slumpmässiga fordonet
                }
                else
                {
                    Console.WriteLine("Kunde inte ta bort det slumpmässigt valda fordonet från garaget.");
                    return null;
                }
            }
            else
            {
                Console.WriteLine("Det finns inga fordon att checka ut.");
                return null;
            }
        }
    }
}
