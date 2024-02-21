// See https://aka.ms/new-console-template for more information
using wordle_cli;

Console.Title = "wordle_cli | AP Demo by Yakov";
NiceConsole.WriteColoredContent("Welcome to Wordle! Made by Yakov.\n",ConsoleColor.Cyan);
NiceConsole.WriteChoiceMenu("What would you like to do?", new string[] { "Play", "View stats", "Change the word dictionary", "Reset all" }, ConsoleColor.Red, ConsoleColor.Yellow);