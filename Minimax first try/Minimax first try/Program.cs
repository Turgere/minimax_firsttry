using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minimax_first_try
{
    class Program
    {
        static string[] OriginalBoard = new string[] { "O", "1", "X", "X", "4", "X", "6", "O", "O" };

        static string HumanPlayer = "O";
        static string AIPlayer = "X";
        static int functionCalls = 0;

        const int FIELD_WIDTH = 3;

        static void Main(string[] args)
        {
            PrintBoard(OriginalBoard, FIELD_WIDTH);
        }

        public static void PrintBoard(string[] board, int a)
        {
            for (int i = 0; i < board.Length; i++)
            {
                if (i % a == 0)
                {
                    Console.WriteLine(board[i]);
                }
                else
                {
                    Console.Write(board[i]);
                }
            }
        }

        public static IEnumerable<string> EmptyIndexes(string[] board)
        {
            var indexes = board.Where(s => s != "O" && s != "X");

            return indexes;
        }

        public static bool Winning(string[] board, string playersMark)
        {
            if (
                (board[0] == playersMark && board[1] == playersMark && board[2] == playersMark) ||
                (board[3] == playersMark && board[4] == playersMark && board[5] == playersMark) ||
                (board[6] == playersMark && board[7] == playersMark && board[8] == playersMark) ||
                (board[0] == playersMark && board[3] == playersMark && board[6] == playersMark) ||
                (board[1] == playersMark && board[4] == playersMark && board[7] == playersMark) ||
                (board[2] == playersMark && board[5] == playersMark && board[8] == playersMark) ||
                (board[0] == playersMark && board[4] == playersMark && board[8] == playersMark) ||
                (board[2] == playersMark && board[4] == playersMark && board[6] == playersMark)
                )
            {
                return true;
            }

            return false;
        }

        public static Move Minimax(string[] newBoard, string player)
        {
            functionCalls++;

            // Получаем доступные для хода клетки
            var availSpots = EmptyIndexes(newBoard).ToArray();
            // Проверяем выигрывает ли одна из сторон при текущем поле
            // Чем меньше результат хода тем выгоднее в эту клетку сходить человеку и тем менее выгодно ходить компьютеру и наоборот.
            if (Winning(newBoard, HumanPlayer))
            {
                Move move = new Move();
                move.score = -10;
                return move;
            }
            else if (Winning(newBoard, AIPlayer))
            {
                Move move = new Move();
                move.score = 10;
            }
            // Проверяем что есть доступные ходы
            else if (availSpots.Count() == 0)
            {
                Move move = new Move();
                move.score = 0;
            }

            // Создаем массив возможных ходов с их результатами
            List<Move> moves = new List<Move>();

            // Для каждой свободной клетки посчитаем результаты ходов
            for (var i = 0; i < availSpots.Count(); i++)
            {
                // Попробуем сделать ход
                Move move = new Move();
                // Получим первую сводобную клетку - она вернется к нам индексом свободной клетки в виде строки
                string nextMoveIndexAsString = availSpots[i];
                // Преобразуем к числу
                int index = Convert.ToInt32(nextMoveIndexAsString);
                // Запишем индекс следующего хода в объект хода
                move.index = Convert.ToInt32(newBoard[index]);

                // Делаем отметку на поле что игрок совершил ход в эту клетку
                newBoard[Convert.ToInt32(availSpots[i])] = player;

                //получить очки, заработанные после вызова минимакса от противника текущего игрока
                if (player == AIPlayer)
                {
                    var result = Minimax(newBoard, HumanPlayer);
                    move.score = result.score;
                }
                else
                {
                    var result = Minimax(newBoard, AIPlayer);
                    move.score = result.score;
                }

                // очистить клетку
                newBoard[Convert.ToInt32(availSpots[i])] = "" + move.index;

                // положить объект в массив
                moves.Add(move);
            }

            Move bestMove = new Move();
            if (player == AIPlayer)
            {
                bestMove.score = -10000;
                for (var i = 0; i < moves.Count; i++)
                {
                    if (moves[i].score > bestMove.score)
                    {
                        bestMove.index = i;
                        bestMove.score = moves[i].score;
                    }
                }
            }
            else
            {
                // else loop over the moves and choose the move with the lowest score
                bestMove.score = 10000;
                for (var i = 0; i < moves.Count; i++)
                {
                    if (moves[i].score < bestMove.score)
                    {
                        bestMove.index = i;
                        bestMove.score = moves[i].score;
                    }
                }
            }

            return bestMove;
        }

    }
}
