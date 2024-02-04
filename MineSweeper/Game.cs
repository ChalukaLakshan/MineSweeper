using MineSweeper.Common;
using MineSweeper.Constant;

namespace MineSweeper
{
    public static class Game
    {
        #region [Private Variables]

        private static int gridSize;
        private static int totalMines;

        private static char[,]? grid;
        private static bool[,]? revealed;

        #endregion

        #region [Public Methods]

        /// <summary>
        /// Main method to Play the Game
        /// </summary>
        public static void Play()
        {
            Console.WriteLine("Welcome to Minesweeper!");

            gridSize = ValidateUserInput("Enter the size of the grid (e.g., 4 for a 4x4 grid):", 2, 10, GameConstatnt.Grid) ?? 0;

            if (gridSize == 0)
            {
                Console.WriteLine("Incorect input. Pleasse try again !");
                return;
            }

            totalMines = ValidateUserInput($"Enter the number of mines to place on the grid (maximum is 35% of the total squares):", 1, gridSize, GameConstatnt.Mines) ?? 0;

            if (totalMines == 0)
            {
                Console.WriteLine("Incorect input. Pleasse try again !");
                return;
            }

            Initialize();
            AddMines();

            while (true)
            {
                PrintGrid();

                Console.Write("Select a square to reveal (e.g., A1):");

                string userInput = Console.ReadLine();

                int row, col;


                if (ParseUserInput(userInput ?? string.Empty, out row, out col))
                {
                    if (IsMine(row, col))
                    {
                        Console.WriteLine("Game Over! You hit a mine.");
                        break;
                    }

                    UncoverSquare(row, col);

                    if (CheckWin())
                    {
                        Console.WriteLine("Congratulations! You won!");
                        break;
                    }
                }
                else
                {
                    Console.WriteLine("Incorect input.");
                }
            }
        }

        /// <summary>
        /// Validate User input based on the Geme requirement
        /// </summary>
        /// <param name="prompt"></param>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static int? ValidateUserInput(string prompt, int minValue, int maxValue, string type)
        {
            int? input = null;
            bool validInput = false;

            Console.Write($"{prompt} ");

            while (!validInput)
            {
                var userInput = ValidateInput.Parseinput(Console.ReadLine());

                if (userInput.HasValue)
                {

                    if (!ValidateInput.IsValidMinValue(userInput.Value, minValue))
                    {
                        Console.WriteLine(type == GameConstatnt.Grid ? $"Minimum size of grid is 2." : $"There must be at least 1 mine.");
                        Console.Write($"{prompt} ");
                    }
                    else if (!ValidateInput.IsValidMaxValue(userInput.Value, maxValue))
                    {
                        Console.WriteLine(type == GameConstatnt.Grid ? $"Maximum size of grid is 10." : $"Maximum number is 35% of total sqaures.");
                        Console.Write($"{prompt} ");
                    }
                    else
                    {
                        validInput = true;
                    }

                    input = userInput;
                }
                else
                {
                    Console.WriteLine($"Incorect input.");
                    Console.Write($"{prompt} ");
                }
            }
            return input;
        }

        /// <summary>
        /// initialize the Grid with defaults values
        /// </summary>
        public static void Initialize()
        {
            grid = new char[gridSize, gridSize];
            revealed = new bool[gridSize, gridSize];

            for (int i = 0; i < gridSize; i++)
            {
                for (int j = 0; j < gridSize; j++)
                {
                    grid[i, j] = ' ';
                    revealed[i, j] = false;
                }
            }
        }

        /// <summary>
        /// Print the Grid in console based on the size
        /// </summary>
        public static void PrintGrid()
        {
            try
            {
                Console.WriteLine("Here is your minefield:");

                // Print column numbers
                Console.Write("  ");
                for (int i = 0; i < gridSize; i++)
                {
                    Console.Write($"{i + 1} ");
                }
                Console.WriteLine();

                Random random = new Random();
                // Print rows
                for (int i = 0; i < gridSize; i++)
                {
                    Console.Write($"{(char)('A' + i)} ");

                    for (int j = 0; j < gridSize; j++)
                    {
                        if (revealed[i, j])
                        {


                            Console.Write($"{grid?[i, j]} ");
                        }
                        else
                        {
                            Console.Write("_ ");
                        }
                    }
                    Console.WriteLine();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }            
        }

        /// <summary>
        /// Randomely mark the mines within the grid
        /// </summary>
        public static void AddMines()
        {
            try
            {
                Random random = new Random();

                for (int i = 0; i < totalMines; i++)
                {
                    int row, col;
                    do
                    {
                        row = random.Next(0, gridSize);
                        col = random.Next(0, gridSize);
                    } while (grid?[row, col] == '*');

                    grid[row, col] = '*';
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }           
        }

        /// <summary>
        /// This method is used to identify whether a recent reveal is mine or not
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public static bool IsMine(int row, int col)
        {
            return grid?[row, col] == '*';
        }

        /// <summary>
        /// This method is used to assing the values for reveal ones and also checked the nearby mines
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        public static void UncoverSquare(int row, int col)
        {
            if (row < 0 || row >= gridSize || col < 0 || col >= gridSize || revealed[row, col])
            {
                return;
            }

            revealed[row, col] = true;

            if (grid[row, col] == ' ')
            {
                grid[row, col] = char.Parse(new Random().Next(0, 3).ToString());
            }


            int minesNearby = CountMinesNearby(row, col);

            if (minesNearby == 0)
            {
                for (int i = row - 1; i <= row + 1; i++)
                {
                    for (int j = col - 1; j <= col + 1; j++)
                    {
                        UncoverSquare(i, j);
                    }
                }
            }
        }

        /// <summary>
        /// This method is used to check nearby mines of recently revel
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public static int CountMinesNearby(int row, int col)
        {
            int count = 0;

            try
            {
                for (int i = row - 1; i <= row + 1; i++)
                {
                    for (int j = col - 1; j <= col + 1; j++)
                    {
                        if (i >= 0 && i < gridSize && j >= 0 && j < gridSize && grid[i, j] == '*')
                        {
                            count++;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }           

            return count;
        }

        /// <summary>
        /// This method is used to evaluate correctly and reveal all values
        /// </summary>
        /// <returns></returns>
        public static bool CheckWin()
        {
            try
            {
                for (int i = 0; i < gridSize; i++)
                {
                    for (int j = 0; j < gridSize; j++)
                    {
                        if (!revealed[i, j] && grid[i, j] != '*')
                        {
                            return false;
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }


        /// <summary>
        /// This method is used to Validate the User input and convert into the values of raw and column 
        /// </summary>
        /// <param name="userInput"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public static bool ParseUserInput(string userInput, out int row, out int col)
        {
            row = -1;
            col = -1;
            try
            {
                if (userInput.Length < 2)
                    return false;

                row = userInput[0] - 'A';
                if (row < 0 || row >= gridSize)
                    return false;

                if (!int.TryParse(userInput.Substring(1), out col))
                    return false;

                if (col < 1 || col > gridSize)
                    return false;

                col--;

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        #endregion
    }
}
