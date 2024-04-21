using System;
using System.Collections.Generic;
namespace wordle_cli
{
    internal class NiceConsole
    {
        public static void WriteColoredContent(string content, ConsoleColor color, bool newLine = true)
        {
            Console.ForegroundColor = color;

            if (newLine)
                Console.WriteLine(content);
            else
                Console.Write(content);

            Console.ForegroundColor = ConsoleColor.White;
        }

        public static int WriteChoiceMenu(string Prompt, string[] Options, ConsoleColor color, ConsoleColor color2)
        {
            int index = 1;
            string menu = string.Empty;

            // Generate the menu string
            foreach (string Option in Options)
            {
                menu += $"{index}. {Option}\n";
                index++;
            }

            WriteColoredContent(Prompt, color);
            WriteColoredContent(menu, color2);

            int choice;
            bool isValidChoice = false;

            do
            {
                Console.ForegroundColor = ConsoleColor.White;

                // Read and validate choice
                var isNumeric = int.TryParse(Console.ReadLine(), out choice);

                if (isNumeric && choice > 0 && choice <= Options.Length)
                {
                    isValidChoice = true;
                }
                else
                {
                    // If choice is invalid, clear the current line and re-prompt for input
                    Console.SetCursorPosition(0, Console.CursorTop - 1);
                    int currentLineCursor = Console.CursorTop;
                    Console.Write(new string(' ', Console.WindowWidth));
                    Console.SetCursorPosition(0, currentLineCursor);
                }
            } while (!isValidChoice);

            return choice;
        }
    }
}
