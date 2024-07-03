using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToe.utils;

namespace TicTacToe.game
{
    public class GameManager
    {
        private bool started;
        private char[ , ] board;
        private int dimension;
        private Figure currentFigureTurn;
        private Random rndm;
        private int maxNumerOfTurns;
        private int currentTurn;

        public bool Started
        {
            get { return started; }
            set { started = value; }
        }

        public GameManager(int dimension = 3)
        {
            started = false;
            this.dimension = dimension;
            board = new char[dimension, dimension];
            rndm = new Random();

            if (rndm.Next(0, 2) == 0)
            {
                currentFigureTurn = Figure.O;
            }
            else
            {
                currentFigureTurn = Figure.X;
            }

            this.maxNumerOfTurns = dimension * dimension;
        }

        public bool StartGame()
        {
            if (started) return false;

            started = true;
            this.currentTurn = 0;

            ClearBoard();
            ExecuteTurn();

            return true;
        }

        public bool StopGame()
        {
            if (!started) return false;

            ClearBoard();

            this.currentTurn = 0;
            started = false;

            return true;
        }

        public void ClearBoard()
        {
            for (int i = 0; i < dimension; i++)
            {
                for (int j = 0; j < dimension; j++)
                {
                    board[i, j] = ' ';
                }
            }
        }

        private void ExecuteTurn()
        {
            bool won = false;

            if (currentTurn >= maxNumerOfTurns)
            {
                Console.WriteLine("Se han agotado el número máximo de turnos posible. El juego queda en empate.");
                StopGame();
                return;
            }

            if (currentFigureTurn.Equals(Figure.X))
            {
                try
                {
                    int row = int.Parse(AskForFirstParameter()) - 1;
                    int column = int.Parse(AskForSecondParameter()) - 1;

                    if (!PlaceFigure(Figure.X, row, column))
                    {
                        ExecuteTurn();
                        return;
                    }

                    won = CheckWin(row, column, this.currentFigureTurn);

                    this.currentFigureTurn = Figure.O;
                }
                catch (Exception)
                {
                    Console.WriteLine("No has introducido un valor válido para la fila o la columna.");
                    return;
                }
            }
            else
            {
                List<Pair<int, int>> freeSlots = GetFreeSlots();
                Shuffle<Pair<int, int>>(freeSlots);

                int row = freeSlots.First().FirstElement;

                var column = freeSlots.First().SecondElement;

                PlaceFigure(Figure.O, row, column);

                won = CheckWin(row, column, this.currentFigureTurn);

                this.currentFigureTurn = Figure.X;
            }

            currentTurn++;

            if (!won)
            {
                ExecuteTurn();
            }
        }

        private static string AskForFirstParameter()
        {
            Console.WriteLine("Escribe el número de la fila en la que quieres poner una ficha.");

            string? firstImput = Console.ReadLine();

            if (firstImput == null)
            {
                Console.WriteLine("No has introducido un dato válido.");
                return AskForFirstParameter();
            } 
            else
            {
                return firstImput;
            }
        }

        private static string AskForSecondParameter()
        {
            Console.WriteLine("Escribe el número de la columna en la que quieres poner una ficha.");

            string? secondImput = Console.ReadLine();

            if (secondImput == null)
            {
                Console.WriteLine("No has introducido un dato válido.");
                return AskForSecondParameter();
            } 
            else
            {
                return secondImput;
            }
        }

        private bool PlaceFigure(Figure figure, int row, int column)
        {
            if (!started)
            {
                Console.WriteLine("El juego debe de estar inicado para poder poner una figura.");
                return false;
            }

            if (row > dimension || column > dimension || row < 0 || column < 0)
            {
                Console.WriteLine("La fila o la columna a modificar es mayor o menor que las dimensiones del tablero.");
                return false;
            }

            Console.WriteLine(board[row, column]);

            if (!(board[row, column] == ' '))
            {
                Console.WriteLine("La casilla introducida ya tiene una figura.");
                return false;
            }

            board[row, column] = ((char)figure);

            Console.WriteLine("Se ha posicionado una pieza en las coordenadas " + (row + 1) + ":" + (column + 1));
            Console.WriteLine("");

            DrawBoard();

            return true;
        }

        private void DrawBoard()
        {
            string boardDisplay = "";
            for (int i = 0; i < dimension; ++i)
            {
                string currentRowBoardDisplay = "";
                string currentRowDecorationDisplay = "";

                for (int j = 0; j < dimension; j++)
                {
                    if (j == 0)
                    {
                        currentRowBoardDisplay += "|| " + board[i, j] + " ||";
                    } 
                    else
                    {
                        currentRowBoardDisplay += " " + board[i, j] + " ||";
                    }
                    currentRowDecorationDisplay += "――――――";
                }

                if (i == 0)
                {
                    boardDisplay += currentRowBoardDisplay;
                }
                else
                {
                    boardDisplay += System.Environment.NewLine + currentRowDecorationDisplay;
                    boardDisplay += System.Environment.NewLine + currentRowBoardDisplay;
                }
            }

            Console.Write(boardDisplay);
            Console.WriteLine("");
        }

        private List<Pair<int, int>> GetFreeSlots()
        {
            List<Pair<int, int>> freeSlots = new List<Pair<int, int>>();

            for (int i = 0; i < dimension; i++)
            {
                for (int j = 0; j < dimension; j++)
                {
                    if (board[i, j].Equals(' ')) freeSlots.Add(new Pair<int, int>(i, j));
                }
            }

            return freeSlots;
        }

        private void Shuffle<T>(IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rndm.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        private bool CheckWin(int row, int column, Figure figure)
        {
            //check col
            for (int i = 0; i < dimension; i++)
            {
                if (board[row, i] != ((char)figure))
                    break;
                if (i == dimension - 1)
                {
                    ExecuteWin(figure);
                    return true;
                }
            }

            //check row
            for (int i = 0; i < dimension; i++)
            {
                if (board[i, column] != ((char)figure))
                    break;
                if (i == dimension - 1)
                {
                    ExecuteWin(figure);
                    return true;
                }
            }

            //check diag
            if (row == column)
            {
                //we're on a diagonal
                for (int i = 0; i < dimension; i++)
                {
                    if (board[i, i] != ((char)figure))
                        break;
                    if (i == dimension - 1)
                    {
                        ExecuteWin(figure);
                        return true;
                    }
                }
            }

            //check anti diag (thanks rampion)
            if (row + column == dimension - 1)
            {
                for (int i = 0; i < dimension; i++)
                {
                    if (board[i, (dimension - 1) - i] != ((char)figure))
                        break;
                    if (i == dimension - 1)
                    {
                        ExecuteWin(figure);
                        return true;
                    }
                }
            }

            return false;
        }

        private void ExecuteWin(Figure figure)
        {
            Console.WriteLine("Ha ganado el juego la figura " + figure.ToString());

            Thread.Sleep(1000);

            StopGame();
        }

    }
}
