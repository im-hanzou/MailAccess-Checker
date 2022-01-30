using System;
using System.Collections.Generic;
using System.Drawing;
using Console = Colorful.Console;

namespace MailAccess_Checker.Utils
{
    public class Logger
    {
                public static void Info(string message)
        {
            Console.Write("[ ! ] ", Color.HotPink);
            Console.Write("Information: ", Color.Gray);
            Console.WriteLine($"'{message}'", Color.HotPink);
        }


        public static void Hit(string credentials)
        {
            Console.Write("[ \u2713 ] ", Color.HotPink);
            Console.Write("Hit: ", Color.Gray);
            Console.WriteLine($"'{credentials}'", Color.HotPink);
        }
        
        public static void Error(ErrorType errorType, string details = "No specifications")
        {
            switch (errorType)
            {
                case ErrorType.CHECK_FAILURE:
                    Console.Write("[ X ] ", Color.IndianRed);
                    Console.Write($"Error: ", Color.Gray);
                    Console.WriteLine($"Check failure ({details})", Color.IndianRed);
                    break;
                case ErrorType.TOO_MANY_INSTANCES:
                    Console.Write("[ X ] ", Color.IndianRed);
                    Console.Write($"Error: ", Color.Gray);
                    Console.WriteLine($"App is already running", Color.IndianRed);
                    break;
                case ErrorType.UNKNOWN:
                    Console.Write("[ X ] ", Color.IndianRed);
                    Console.Write($"Error: ", Color.Gray);
                    Console.WriteLine($"Unexpected error happened ({details})", Color.IndianRed);
                    break;
                case ErrorType.AUTH_ERROR:
                    Console.Write("[ X ] ", Color.IndianRed);
                    Console.Write($"Error: ", Color.Gray);
                    Console.WriteLine($"Authentication Error ({details})", Color.IndianRed);
                    break;



            }
        }

        public static void Warning(string message)
         {
                Console.Write("[ ! ] ", Color.PaleGoldenrod);
                Console.Write("Warning: ", Color.Gray);
                Console.WriteLine($"{message}", Color.PaleGoldenrod);
        }

        public static void Question(string message)
        {
            Console.WriteLine("[ ? ] " + message, Color.Pink);
            Console.Write("> ", Color.IndianRed);
            Console.Write("", Color.White);
        }

        public static void Debug(string message)
        {
            if (Program.debug)
                Console.WriteLine("[ Debug ] " + message, Color.LightBlue);
        }

        public enum ErrorType
        {
            CHECK_FAILURE,
            TOO_MANY_INSTANCES,
            AUTH_ERROR,
            UNKNOWN
        }
    }
}