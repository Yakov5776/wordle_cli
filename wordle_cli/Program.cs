using System.Text;

namespace wordle_cli
{
    class Program
    {
        static Difficulty selectedDifficulty = Difficulty.Easy;
        static int victories = 0;
        static int losses = 0;
        static int streak = 0;
        static void Main()
        {
            Console.Title = "wordle_cli | AP Demo";
            Console.Clear();
            NiceConsole.WriteColoredContent("Welcome to Wordle! Made by Yakov.\n", ConsoleColor.Cyan) ;

            NiceConsole.WriteColoredContent("Difficulty: ", ConsoleColor.Gray, false);
            NiceConsole.WriteColoredContent(selectedDifficulty.ToString(), GetDifficultyColor(selectedDifficulty));

            int menuChoice = NiceConsole.WriteChoiceMenu("What would you like to do?", new string[] { "Play", "View stats", "Change difficulty", "Reset all", "Quit" }, ConsoleColor.Red, ConsoleColor.Yellow);

            switch (menuChoice)
            {
                case 1:
                    Play();
                    break;
                case 2:
                    ViewStats();
                    break;
                case 3:
                    ChangeDifficulty();
                    break;
                case 4:
                    selectedDifficulty = Difficulty.Easy;
                    break;
                case 5:
                    Environment.Exit(0);
                    return;
            }

            Main();
        }

        static void Play()
        {
            Console.Clear();
            NiceConsole.WriteColoredContent("Guess the word:\n", ConsoleColor.Cyan);

            int maxTries = 6;
            int currentTries = 0;
            bool wonGame = false;
            List<string> validWords = wordle_cli.Properties.Resources.wordlist.Split('\n').ToList();
            (int min, int max) = GetDifficultyRange(selectedDifficulty);
            string randomWord = validWords[new Random().Next(min, max)];
            int totalLetters = randomWord.Length;

            while (currentTries < maxTries && !wonGame)
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
                            if (guess.Length == totalLetters && validWords.Contains(guess.ToString()))
                            {
                                submitted = true;
                                currentTries++;
                                LetterStatus[] guessCorrectness = ProcessGuess(guess.ToString(), randomWord);
                                PrintProcessedGuess(guessCorrectness, guess);
                                wonGame = guessCorrectness.All(guess => guess == LetterStatus.CorrectLocation);
                            }
                            break;
                        default:
                            if (char.IsLetter(key.KeyChar) && guess.Length < totalLetters)
                            {
                                Console.SetCursorPosition(curPos, Console.CursorTop);
                                Console.Write(key.Key);
                                guess.Append(key.KeyChar);
                                curPos++;
                            }
                            break;
                    }
                }
            }

            Console.Write('\n');
            if (wonGame)
            {
                victories++;
                streak++;
                NiceConsole.WriteColoredContent("Congratulations! ", ConsoleColor.Green, false);
            }
            else
            {
                losses++;
                streak = 0;
                NiceConsole.WriteColoredContent("Sorry! ", ConsoleColor.Red, false);
            }
            NiceConsole.WriteColoredContent("The word was: " + randomWord.ToUpper(), ConsoleColor.Yellow);
            NiceConsole.WriteColoredContent("Press enter to return to the Main Menu...", ConsoleColor.White);
            Console.ReadLine();
            Main();
        }

        enum LetterStatus
        {
            NotInWord,
            CorrectLocation,
            IncorrectLocation
        }

        static LetterStatus[] ProcessGuess(string guess, string answer)
        {
            char[] guessChar = guess.ToCharArray();
            char[] answerChar = answer.ToCharArray();

            LetterStatus[] processedGuess = new LetterStatus[guessChar.Length];

            for (int i = 0; i < guessChar.Length; i++)
            {
                if (guessChar[i] == answerChar[i])
                {
                    processedGuess[i] = LetterStatus.CorrectLocation;
                    guessChar[i] = '*';
                    answerChar[i] = '*';
                }
            }

            for (int i = 0; i < guessChar.Length; i++)
            {
                if (guessChar[i] != '*' && answerChar.Contains(guessChar[i]))
                {
                    processedGuess[i] = LetterStatus.IncorrectLocation;
                    int index = Array.IndexOf(answerChar, guessChar[i]);
                    guessChar[i] = '*';
                    answerChar[index] = '*';
                }
            }

            return processedGuess;
        }

        static void ChangeDifficulty()
        {
            Console.Clear();
            NiceConsole.WriteColoredContent("Current difficulty is ", ConsoleColor.Gray, false);
            NiceConsole.WriteColoredContent(selectedDifficulty.ToString(), GetDifficultyColor(selectedDifficulty));
            int menuChoice = NiceConsole.WriteChoiceMenu("Choose your difficulty:\n", new string[] { "Easy", "Medium", "Hard" }, ConsoleColor.Cyan, ConsoleColor.Yellow);
            switch (menuChoice)
            {
                case 1:
                    selectedDifficulty = Difficulty.Easy;
                    break;
                case 2:
                    selectedDifficulty = Difficulty.Medium;
                    break;
                case 3:
                    selectedDifficulty = Difficulty.Hard;
                    break;
            }
        }

        static void PrintProcessedGuess(LetterStatus[] guessCorrectness, StringBuilder guess)
        {
            Console.SetCursorPosition(0, Console.CursorTop);
            for (int i = 0; i < guessCorrectness.Length; i++)
            {
                if (guessCorrectness[i] == LetterStatus.CorrectLocation) Console.BackgroundColor = ConsoleColor.Green;
                else if (guessCorrectness[i] == LetterStatus.IncorrectLocation) Console.BackgroundColor = ConsoleColor.DarkYellow;
                Console.Write(Char.ToUpper(guess[i]));
                Console.BackgroundColor = ConsoleColor.Black;
            }
            Console.Write('\n');
        }

        enum Difficulty
        {
            Easy,
            Medium,
            Hard
        }

        static ConsoleColor GetDifficultyColor(Difficulty difficulty)
        {
            switch(difficulty)
            {
                case Difficulty.Easy:
                    return ConsoleColor.Green;
                case Difficulty.Medium:
                    return ConsoleColor.DarkYellow;
                case Difficulty.Hard:
                    return ConsoleColor.DarkMagenta;
                default:
                    return ConsoleColor.White;
            }
        }

        static (int, int) GetDifficultyRange(Difficulty difficulty)
        {
            switch (difficulty)
            {
                case Difficulty.Easy:
                    return (0, 2000);
                case Difficulty.Medium:
                    return (2000, 3000);
                case Difficulty.Hard:
                    return (3000, 4000);
                default:
                    return (0,0);
            }
        }

        static void ViewStats()
        {
            Console.Clear();
            int totalGames = victories + losses;
            int winRatio = totalGames > 0 ? (victories * 100) / totalGames : 0;

            NiceConsole.WriteColoredContent("Total Games: ", ConsoleColor.Yellow, false);
            NiceConsole.WriteColoredContent(totalGames.ToString(), ConsoleColor.Cyan);
            NiceConsole.WriteColoredContent("Streak: ", ConsoleColor.Yellow, false);
            NiceConsole.WriteColoredContent(streak.ToString(), ConsoleColor.Cyan);
            NiceConsole.WriteColoredContent("Total Wins: ", ConsoleColor.Yellow, false);
            NiceConsole.WriteColoredContent(victories.ToString(), ConsoleColor.Green);
            NiceConsole.WriteColoredContent("Total Losses: ", ConsoleColor.Yellow, false);
            NiceConsole.WriteColoredContent(losses.ToString(), ConsoleColor.Red);
            NiceConsole.WriteColoredContent("Winning Rate: ", ConsoleColor.Yellow, false);
            NiceConsole.WriteColoredContent(winRatio.ToString() + "%", winRatio > 50 ? ConsoleColor.Green : ConsoleColor.Red );

            Console.ReadKey();
        }

    }
}