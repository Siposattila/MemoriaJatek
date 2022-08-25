using System;
using MemoriaJatek.CardMaster;

namespace MemoriaJatek.ConsoleMaster
{
    class ConsoleHandler
    {
        /**
         * <summary>This enum defines console output types. Faliure, Success, Warning and Default.</summary>
         */
        public enum OutputType : int
        {
            Faliure = ConsoleColor.Red,
            Success = ConsoleColor.Green,
            Warning = ConsoleColor.Yellow,
            Default = ConsoleColor.White
        }
        public string consolePrefix = "[¬]";

        /**
         * <summary>This function handles input asking from the user.</summary>
         * <returns>Returns a user input as a string.</returns>
         */
        public string InputAsking(string message)
        {
            Console.Write($"{consolePrefix}\t{message}: ");
            string answer = Console.ReadLine();
            Console.WriteLine();

            return answer;
        }

        /**
         * <summary>This function handles outputs to the console.</summary>
         */
        public void Output(string message, OutputType type, bool clear = false)
        {
            if (clear) Console.Clear();
            Console.ForegroundColor = (ConsoleColor) type;
            Console.WriteLine($"{consolePrefix}\t{message}", Console.ForegroundColor);
            Console.ForegroundColor = (ConsoleColor) OutputType.Default;
        }

        /**
         * <summary>This function handles outputs to the console.</summary>
         */
        public void OutputGameMap(Card[,] matrix, int turnCount)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"Eddig próbálkozások: {turnCount}\n");
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                Console.Write("\t\t\t");
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    string format = "{0}";
                    if (matrix[i, j].CardNumber.ToString().Length > 1)
                        format += " ";
                    else format += "  ";

                    if (matrix[i, j].IsHidden)
                        Console.Write(String.Format(format, " "));
                    else Console.Write(String.Format(format, matrix[i, j].CardNumber));
                }
                Console.WriteLine();
            }
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.White;
        }

        /**
         * <summary>This function outputs the map menu to the console.</summary>
         */
        public int[] MapMenu()
        {
            Console.Clear();
            
            string[] menuItems = new string[]
            {
                "2x2 (könnyű)",
                "4x4 (normál)",
                "6x6 (közepes)",
                "8x8 (nehéz)"
            };

            Console.CursorVisible = false;
            int indexMapMenu = 0;
            string selectedMenuItem = "";
            while (true)
            {
                Console.WriteLine("Válasszon játék terület nagyságot!\n");
                selectedMenuItem = DrawMenu(menuItems, ref indexMapMenu);
                switch (selectedMenuItem)
                {
                    case "2x2 (könnyű)":
                        return new int[] { 2, 2 };
                    case "4x4 (normál)":
                        return new int[] { 4, 4 };
                    case "6x6 (közepes)":
                        return new int[] { 6, 6 };
                    case "8x8 (nehéz)":
                        return new int[] { 8, 8 };
                    default:
                        break;
                }
            }
        }

        /**
         * <summary>This function outputs the map menu to the console.</summary>
         */
        public int DifficultyMenu()
        {
            Console.Clear();

            string[] menuItems = new string[]
            {
                "6 másodperces kártyafordítás (könnyű)",
                "5 másodperces kártyafordítás (normál)",
                "3 másodperces kártyafordítás (közepes)",
                "1 másodperces kártyafordítás (nehéz)",
            };

            Console.CursorVisible = false;
            int indexDiffucultyMenu = 0;
            string selectedMenuItem = "";
            while (true)
            {
                Console.WriteLine("Válasszon kártyafordítási időt!\n");
                selectedMenuItem = DrawMenu(menuItems, ref indexDiffucultyMenu);
                switch (selectedMenuItem)
                {
                    case "6 másodperces kártyafordítás (könnyű)":
                        return 6000;
                    case "5 másodperces kártyafordítás (normál)":
                        return 5000;
                    case "3 másodperces kártyafordítás (közepes)":
                        return 3000;
                    case "1 másodperces kártyafordítás (nehéz)":
                        return 1000;
                    default:
                        break;
                }
            }
        }

        /**
         * <summary>This function outputs the restart menu to the console.</summary>
         */
        public bool RestartMenu(int turnCount)
        {
            Console.Clear();

            string[] menuItems = new string[]
            {
                "Új játék",
                "Kilépés"
            };

            Console.CursorVisible = false;
            int indexRestartMenu = 0;
            string selectedMenuItem = "";
            while (true)
            {
                Console.WriteLine($"Az előző játékot ennyi próbálkozás alatt teljesítetted: {turnCount}\n");
                selectedMenuItem = DrawMenu(menuItems, ref indexRestartMenu);
                switch (selectedMenuItem)
                {
                    case "Új játék":
                        return true;
                    case "Kilépés":
                        return false;
                    default:
                        break;
                }
            }
        }

        /**
         * <summary>This function draws an interactive menu.</summary>
         */
        private string DrawMenu(string[] items, ref int menuIndex)
        {
            for (int i = 0; i < items.Length; i++)
            {
                if (i == menuIndex)
                {
                    Console.BackgroundColor = ConsoleColor.Gray;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.WriteLine($"{consolePrefix} {items[i]}");
                }
                else Console.WriteLine($"{consolePrefix} {items[i]}");
                Console.ResetColor();
            }

            ConsoleKeyInfo consoleKey = Console.ReadKey();
            switch (consoleKey.Key)
            {
                case ConsoleKey.DownArrow:
                    if (menuIndex != items.Length - 1) menuIndex++;
                    break;
                case ConsoleKey.UpArrow:
                    if (menuIndex > 0) menuIndex--;
                    break;
                case ConsoleKey.LeftArrow:
                    Console.Clear();
                    break;
                case ConsoleKey.RightArrow:
                    Console.Clear();
                break;
                case ConsoleKey.Enter:
                    Console.CursorVisible = true;
                    return items[menuIndex];
                default:
                    return "";
            }

            Console.Clear();
            return "";
        }
    }
}