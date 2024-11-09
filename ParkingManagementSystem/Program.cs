using System;
using System.Security.Cryptography;

namespace ParkingManagementSystem;

class Program
{
    static void Main(string[] args)
    {
        int numberOfParkingSpaces = 15;
        Garage garage = new Garage(numberOfParkingSpaces);
        Feed feed = new Feed();

        int usedRows = garage.ShowParkingSpaces();
        int menuRow = usedRows + 3;
        
        //// Starta klockan i en separat tråd för att undvika konflikt med annan output
        //Thread clockThread = new Thread(() =>
        //{
        //    while (true)
        //    {

        //        Console.SetCursorPosition(79, 0);
        //        Console.ForegroundColor = ConsoleColor.Yellow;
        //        Console.WriteLine("Tid: " + DateTime.Now.ToString("HH: mm: ss"));
        //        Console.ResetColor();
        //        Console.SetCursorPosition(0, menuRow + 1);
        //        Console.WriteLine("                                                        ");
        //        Thread.Sleep(1000);
        //    }
        //});

        //clockThread.IsBackground = true;
        //clockThread.Start();

        while (true)
        {
            Console.Clear();
            garage.ShowParkingSpaces();

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
