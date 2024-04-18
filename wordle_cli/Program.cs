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
                    Environment.Exit(0);
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
            List<string> validWords = wordle_cli.Properties.Resources.wordlist.Split('\n').ToList();
            string randomWord = validWords[new Random().Next(0, 2000)];

            while (maxTries > currentTries)
            {
                NiceConsole.WriteColoredContent(new StringBuilder(new string('_', totalLetters)).ToString(), ConsoleColor.Cyan, false);
                Console.SetCursorPosition(0, Console.CursorTop);

                bool submitted = false;
                int curPos = 0;
                StringBuilder guess = new StringBuilder();
                while (!submitted)
                {
                    Console.SetCursorPosition(curPos, Console.CursorTop);
                    ConsoleKeyInfo key = Console.ReadKey(true);
                    switch (key.Key)
                    {
                        case ConsoleKey.Backspace:
                            if (curPos > 0)
                            {
                                Console.CursorLeft--;
                                curPos--;
                                guess.Length--;
                                NiceConsole.WriteColoredContent('_'.ToString(), ConsoleColor.Cyan, false);
                            }
                            break;
                        case ConsoleKey.Enter:
                            if (guess.Length != totalLetters) break;
                            if (!validWords.Contains(guess.ToString())) break;
                            submitted = true;
                            currentTries++;
                            Console.SetCursorPosition(0, Console.CursorTop);
                            var guesses = Try(guess.ToString(), randomWord);
                            for (int i = 0; i < guesses.Length; i++)
                            {
                                if (guesses[i] == LetterStatus.White) Console.ForegroundColor = ConsoleColor.White;
                                else if (guesses[i] == LetterStatus.Green) Console.ForegroundColor = ConsoleColor.Green;
                                else if (guesses[i] == LetterStatus.Yellow) Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.Write(Char.ToUpper(guess[i]));
                            }

                            Console.Write('\n');
                            break;
                        default:
                            if (char.IsLetter(key.KeyChar))
                            {
                                if (curPos >= guess.Length && guess.Length == totalLetters) break;
                                Console.SetCursorPosition(curPos, Console.CursorTop);
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.Write(key.Key);
                                guess.Append(key.KeyChar);
                                curPos++;
                            }
                            //Console.CursorLeft+=2;
                            break;
                    }
                }
            }

            Main();
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