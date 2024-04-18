using System.Text;

namespace wordle_cli
{
    class Program
    {
        DictionaryManager dictionaryManager = new DictionaryManager();

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
                case 3: // Change dictionary
                    ChangeDict();
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
                            curPos = Math.Max(curPos - 1, 0);
                            break;
                        case ConsoleKey.RightArrow:
                            curPos = Math.Min(curPos + 1, letters);
                            break;
                        case ConsoleKey.Backspace:
                            if (curPos > 0)
                            {

                                Console.CursorLeft--;
                                curPos--;
                                letters--;
                                NiceConsole.WriteColoredContent('_'.ToString(), ConsoleColor.Cyan, false);
                            }
                            break;
                        case ConsoleKey.Enter:
                            if (letters != totalLetters) break;
                            submitted = true;
                            currentTries++;
                            Console.Write('\n');
                            break;
                        default:
                            if (char.IsLetter(key.KeyChar))
                            {
                                if (curPos >= letters && letters == totalLetters) break;
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

        enum LetterStatus
        {
            White,
            Green,
            Yellow
        }

        static LetterStatus[] Try(string guess, string answer)
        {
            char[] guessC = guess.ToCharArray();
            char[] answerC = answer.ToCharArray();

            var result = new LetterStatus[guessC.Length];

            for (int i = 0; i < guessC.Length; i++)
            {
                if (guessC[i] == answerC[i])
                {
                    result[i] = LetterStatus.Green;
                    guessC[i] = '*';
                    answerC[i] = '*';
                }
            }

            for (int i = 0; i < guessC.Length; i++)
            {
                if (guessC[i] != '*' && answerC.Contains(guessC[i]))
                {
                    result[i] = LetterStatus.Yellow;
                    var index = Array.IndexOf(answerC, guessC[i]);
                    guessC[i] = '*';
                    answerC[index] = '*';
                }
            }

            for (int i = 0; i < result.Length; i++)
            {
                if (result[i] == 0)
                    result[i] = LetterStatus.White;
            }

            return result;
        }

        static void ChangeDict()
        {

        }
    }
}