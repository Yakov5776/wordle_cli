using System.Text;

namespace wordle_cli
{ 
    class Program
    {
        static void Main()
        {
            Console.Title = "wordle_cli | AP Demo by Yakov";
            NiceConsole.WriteColoredContent("Welcome to Wordle! Made by Yakov.\n", ConsoleColor.Cyan);
            int option = NiceConsole.WriteChoiceMenu("What would you like to do?", new string[] { "Play", "View stats", "Change the word dictionary", "Reset all", "Quit" }, ConsoleColor.Red, ConsoleColor.Yellow);
        
            switch (option)
            {
                case 1: // Play
                    Play();
                    break;
                case 2: // View stats
                    break;
                case 4: // Quit
                    return;
            }
        }

        static void Play()
        {
            Console.Clear();
            NiceConsole.WriteColoredContent("Guess the word:\n", ConsoleColor.Cyan);

            int maxTries = 5;
            int currentTries = 0;
            int totalLetters = 5;
            StringBuilder guessedWord = new StringBuilder(new string('_', totalLetters));

            while (maxTries > currentTries)
            {
                NiceConsole.WriteColoredContent(guessedWord.ToString(), ConsoleColor.Cyan, false);
                Console.SetCursorPosition(0, Console.CursorTop);

                bool submitted = false;
                int letters = 0, curPos = 0;
                while (!submitted)
                {
                    Console.SetCursorPosition(curPos, Console.CursorTop);
                    ConsoleKeyInfo key = Console.ReadKey(true);
                    switch (key.Key)
                    {
                        case ConsoleKey.LeftArrow:
                            //Console.CursorLeft = Math.Max(Console.CursorLeft - 1, 0);
                            curPos = (curPos - 1).Clamp(0, letters);
                            break;
                        case ConsoleKey.RightArrow:
                            curPos = (curPos + 1).Clamp(0, letters);
                            break;
                        case ConsoleKey.Backspace:
                            if (curPos > 0)
                            {

                            Console.CursorLeft--;
                            curPos--;
                            NiceConsole.WriteColoredContent('_'.ToString(), ConsoleColor.Cyan, false);
                            }
                            break;
                        case ConsoleKey.Enter:
                            submitted = true;
                            break;
                        default:
                            if (char.IsLetter(key.KeyChar)) {
                                if (curPos == letters && letters == totalLetters) break;
                                Console.SetCursorPosition(curPos, Console.CursorTop);
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.Write(key.Key);
                                letters++;
                                curPos++;
                            }
                            //Console.CursorLeft+=2;
                            break;
                    }
                }   
            }
        }
    }

    public static class InputExtensions
{
    public static int Clamp(
        this int value, int Min, int Max)
    {
        if (value < Min) { return Min; }
        if (value > Max) { return Max; }
        return value;
    }
}
}