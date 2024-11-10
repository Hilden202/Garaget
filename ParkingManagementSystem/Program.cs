using System;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace ParkingManagementSystem;

class Program
{
    static void Main(string[] args)
    {
        int numberOfParkingSpaces = 15;
        Garage garage = new Garage(numberOfParkingSpaces);
        Feed feed = new Feed();

        //Clock clock = new Clock();
        //clock.Start();

        while (true)
        {
            Console.SetCursorPosition(0, 0);
            Console.Write(new string(' ', Console.WindowWidth * (Console.WindowHeight - 1))); //Istället för Console.Clear

            int usedRows = garage.ShowParkingSpaces();
            int menuRow = usedRows + 3;

            Console.SetCursorPosition(0, 0);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("[Q] Avsluta");
            Console.ResetColor();

            Console.SetCursorPosition(0, menuRow);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("[1] Checka in ett fordon:");
            Console.ResetColor();

            Console.SetCursorPosition(35, menuRow);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("[2] Checka ut ett fordon:");
            Console.ResetColor();

            feed.DisplayRecentFeeds();
            Console.SetCursorPosition(0, menuRow + 1);

            ConsoleKeyInfo key = Console.ReadKey(true);

            switch (key.KeyChar)

            {
                case '1':
                    VehicleManager.CheckinVehicle(garage, feed);
                    break;

                case '2':
                    VehicleManager.CheckoutVehicle(garage, feed);
                    break;

                case 'Q':
                case 'q':
                    return;
            }
        }

    }
}
