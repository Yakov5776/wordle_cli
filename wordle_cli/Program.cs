namespace wordle_cli
{ 
    class Program
    {
        static void Main()
        {
            Console.Title = "wordle_cli | AP Demo by Yakov";
            NiceConsole.WriteColoredContent("Welcome to Wordle! Made by Yakov.\n", ConsoleColor.Cyan);
            int option = NiceConsole.WriteChoiceMenu("What would you like to do?", new string[] { "Play", "View stats", "Change the word dictionary", "Reset all" }, ConsoleColor.Red, ConsoleColor.Yellow);
        
            switch (option)
            {
                case 1: // Play
                    Play();
                    break;
            }
        }

        static void Play()
        {

        }
    }
}