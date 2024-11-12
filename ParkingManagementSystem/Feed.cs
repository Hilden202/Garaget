using System;
using System.Collections.Generic;

namespace ParkingManagementSystem
{
    public class FeedMessage
    {
        public string Message { get; set; }
        public ConsoleColor Color { get; set; }

        public FeedMessage(string message, ConsoleColor color)
        {
            Message = message;
            Color = color;
        }
    }
    public class Feed
    {
        private List<FeedMessage> messages = new List<FeedMessage>();

        public void AddMessage(string message, ConsoleColor color = ConsoleColor.White)
        {
            if (messages.Count >= 10) // 10 meddelanden
            {
                messages.RemoveAt(messages.Count - 1); // Ta bort det äldsta meddelandet
            }
            messages.Insert(0, new FeedMessage(message, color));
        }

        public void DisplayMessages()
        {
            foreach (FeedMessage feedMessage in messages)
            {
                Console.ForegroundColor = feedMessage.Color;
                Console.WriteLine(feedMessage.Message);
            }
            Console.ResetColor();
        }

        public void DisplayRecentFeeds()
        {
            int startX = 65;
            int startY = 0;

            // Rensa tidigare feed (anpassa rensningen för upp till 15 meddelanden)
            for (int i = 0; i < 17; i++)
            {
                Console.SetCursorPosition(startX, startY + i);
                Console.WriteLine("            ");
            }

            Console.SetCursorPosition(startX, startY); // Flytta tillbaka markören till toppen av feeden
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Senaste Nytt");
            Console.ResetColor();
            Console.SetCursorPosition(startX, startY + 1);
            Console.Write(new string('=', 44));

            for (int i = 0; i < messages.Count; i++)
            {
                Console.SetCursorPosition(startX, startY + i + 2);
                Console.ForegroundColor = messages[i].Color;
                Console.WriteLine(messages[i].Message);
                Console.ResetColor();
            }
        }
    }
}
