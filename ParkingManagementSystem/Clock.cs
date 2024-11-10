using System;
namespace ParkingManagementSystem
{
    public class Clock //Todo: Fungerar inte som jag tänkt men nästan..
    {
        public void Start()
        {
            Thread clockThread = new Thread(() =>
            {
                while (true)
                {

                    Console.SetCursorPosition(0, 19);

                    // Spara den aktuella positionen
                    int currentLeft = Console.CursorLeft;
                    int currentTop = Console.CursorTop;

                    Console.SetCursorPosition(81, 0);
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("Tid: " + DateTime.Now.ToString("HH:mm:ss"));
                    Console.ResetColor();

                    // Återställ cursorpositionen till den ursprungliga platsen
                    Console.SetCursorPosition(currentLeft, currentTop);

                    Thread.Sleep(1000);
                }
            });

            clockThread.IsBackground = true;
            clockThread.Start();
        }
    }

}