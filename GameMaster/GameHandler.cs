using System;
using System.IO;
using System.Threading;
using MemoriaJatek.ConsoleMaster;
using MemoriaJatek.CardMaster;

namespace MemoriaJatek.GameMaster
{
    class GameHandler
    {
        private int xSize;
        private int ySize;
        private int sleepTime;
        private int hiddenCards;
        private Card[,] gameMatrix;
        private Random random = new Random();
        public ConsoleHandler consoleHandler = new ConsoleHandler();
        public bool IsGameActive { get; private set; }
        public int Turn { get; private set; }

        public GameHandler()
        {
            Init();
        }

        /**
         * <summary>This function sets up the game.</summary>
         */
        private void Init()
        {
            hiddenCards = 0;
            Turn = 0;
            int[] sizes = consoleHandler.MapMenu();
            xSize = (sizes != null) ? sizes[0] : 6;
            ySize = (sizes != null) ? sizes[1] : 6;
            sleepTime = consoleHandler.DifficultyMenu();
            string[] words = GetWordsForGameMap(ReadWordList());
            gameMatrix = MakeGameMap(new Card[xSize,ySize], words);
        }

        /**
         * <summary>This function handles outputs to the console.</summary>
         */
        public void Start()
        {
            IsGameActive = true;
            if (gameMatrix != null)
            {
                while (IsGameActive)
                {
                    GameTurn();
                    IsGameActive = hiddenCards != (xSize * ySize);
                }

                Stop();
            }
        }

        /**
         * <summary>This function stops the game from running by force if needed and also handles output when the game ends.</summary>
         */
        public void Stop()
        {
            Console.Clear();
            consoleHandler.Output($"A játéknak vége! Ennyi próbálkozás alatt teljesítetted: {Turn}", ConsoleHandler.OutputType.Success);
            consoleHandler.Output("5 másodperc múlva kilépünk a menübe...", ConsoleHandler.OutputType.Warning);
            Thread.Sleep(5000);
            if (consoleHandler.RestartMenu(Turn))
            {
                Init();
                Start();
            }
            else Environment.Exit(1);
        }

        /**
         * <summary>This function reads in the wordlist that is included (wordlist.txt).</summary>
         */
        private string[] ReadWordList()
        {
            while (!File.Exists(Path.GetFullPath(".")+"/wordlist.txt"))
            {
                consoleHandler.Output("A wordlist.txt nem a megfelelő helyen van!", ConsoleHandler.OutputType.Faliure, true);
                consoleHandler.Output("Újrapróbálkozás 5 másodperc múlva...", ConsoleHandler.OutputType.Warning);
                Thread.Sleep(5000);
            }

            return File.ReadAllLines(Path.GetFullPath(".")+"/wordlist.txt");
        }

        /**
         * <summary>This function prepares/selects the words for the game.</summary>
         */
        private string[] GetWordsForGameMap(string[] wordlist)
        {
            string[] words = new string[xSize*ySize];
            for (int i = 0; i < (xSize*ySize); i += 2)
            {
                words[i] = wordlist[random.Next(0, wordlist.Length)];
                words[i + 1] = words[i];
            }

            return Shuffle(words);
        }

        /**
         * <summary>This function shuffles the words array to make the game even harder.</summary>
         */
        private string[] Shuffle(string[] words)
        {
            for (int i = 0; i < words.Length; i++)
            {
                int randomIndex = random.Next(0, words.Length);
                string temporary = words[randomIndex];
                words[randomIndex] = words[i];
                words[i] = temporary;
            }

            return words;
        }

        /**
         * <summary>This function makes the map for the game it loads up the card matrix.</summary>
         */
        private Card[,] MakeGameMap(Card[,] matrix, string[] words)
        {
            int number = 1;
            for (int i = 0; i < xSize; i++)
            {
                for (int j = 0; j < ySize; j++)
                {
                    matrix[i, j] = new Card(words[number - 1], number++);
                }
            }

            return matrix;
        }

        /**
         * <summary>This function handles a game turn.</summary>
         */
        private void GameTurn()
        {
            Turn++;
            consoleHandler.OutputGameMap(gameMatrix, Turn);
            Card[] selectedCards = SelectCards();
            while (selectedCards[0] == null || selectedCards[1] == null)
            {
                selectedCards = SelectCards();
            }

            consoleHandler.Output($"{selectedCards[0].CardNumber}: {selectedCards[0]}", ConsoleHandler.OutputType.Warning);
            consoleHandler.Output($"{selectedCards[1].CardNumber}: {selectedCards[1]}", ConsoleHandler.OutputType.Warning);

            if (selectedCards[0].CardValue == selectedCards[1].CardValue)
            {
                hiddenCards += 2;
                selectedCards[0].IsHidden = true;
                selectedCards[1].IsHidden = true;
                consoleHandler.Output("Talált!", ConsoleHandler.OutputType.Success);
            }
            else consoleHandler.Output("Nem talált.", ConsoleHandler.OutputType.Faliure);

            Thread.Sleep(sleepTime);
        }

        /**
         * <summary>This function will select two cards based on the user input.</summary>
         */
        private Card[] SelectCards()
        {
            string answer = "";
            string[] cardNumbers = null;
            while (cardNumbers == null)
            {
                answer = consoleHandler.InputAsking("Melyik kártyák legyenek? [szám, szám]");
                if (answer != "")
                {
                    cardNumbers = answer.Split(",");
                    if (cardNumbers.Length == 2)
                    {
                        cardNumbers[0] = cardNumbers[0].Trim();
                        cardNumbers[1] = cardNumbers[1].Trim();
                        if (cardNumbers[0] == "" || cardNumbers[1] == "") cardNumbers = null;
                        else if (int.Parse(cardNumbers[0]) < 0 || int.Parse(cardNumbers[1]) < 0) cardNumbers = null;
                    }
                    else cardNumbers = null;
                }
            }

            Card cardOne = null;
            Card cardTwo = null;
            for (int i = 0; i < xSize; i++)
            {
                for (int j = 0; j < ySize; j++)
                {
                    if (gameMatrix[i, j].CardNumber == int.Parse(cardNumbers[0]) && !gameMatrix[i, j].IsHidden) cardOne = gameMatrix[i, j];
                    else if (gameMatrix[i, j].CardNumber == int.Parse(cardNumbers[1]) && !gameMatrix[i, j].IsHidden) cardTwo = gameMatrix[i, j];
                }
            }

            return new Card[] { cardOne, cardTwo };
        }
    }
}