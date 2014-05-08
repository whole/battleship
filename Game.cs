using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace battleShip
{
  public class Game
    {
        public enum CellState { Empty, EmptyHit, Gray, GrayHit };

      public enum ActionResult
      {
          Miss,
          Hit,
          Sunk
      };

        // the first cell has coordinates [0,0], the last cell has coordinates [9,9]
        public CellState[,] board = new CellState[BOARD_SIZE, BOARD_SIZE];

        #region Game parameters
        private const int SIZE_BATTLESHIP = 5;
        private const int SIZE_DESTROYER = 4;
        private const int NUMBER_BATTLESHIP = 1;
        private const int NUMBER_DESTROYER = 2;
        private const int BOARD_SIZE = 10;
        #endregion

      public Game()
      {
        EmptyBoard();
      }


      public int NumberOfShipsOnBoard()
      {
          int numHorizontal = 0, numVertical = 0;

          // count the number of horizontal ships
          for (int i = 0; i < BOARD_SIZE; ++i)
          {
              for (int j = 0; j < BOARD_SIZE-1; ++j)
              {
                  if (IsEmptyCell(i, j))
                  {
                      continue;
                  }
                  if (IsValidCell(i + 1, j) && IsEmptyCell(i + 1, j))
                  {
                      continue;
                  }

                  bool isShipAlive = board[i, j] == CellState.Gray;
                  while (IsValidCell(i, j) && IsShipCell(i, j))
                  {
                      ++j;
                      isShipAlive = isShipAlive || (board[i, j] == CellState.Gray);
                  }
                  if(isShipAlive) {
                    numHorizontal++;
                  }
              }
          }

          // count the number of vertical ships
          for (int j = 0; j < BOARD_SIZE; ++j)
          {
              for (int i = 0; i < BOARD_SIZE-1; ++i)
              {
                  if (IsEmptyCell(i, j))
                  {
                      continue;
                  }
                  if (IsValidCell(i, j + 1) && IsEmptyCell(i, j + 1))
                  {
                      continue;
                  }

                  bool isShipAlive = board[i, j] == CellState.Gray;
                  while (IsValidCell(i, j) && IsShipCell(i, j))
                  {
                      ++i;
                      isShipAlive = isShipAlive || (board[i, j] == CellState.Gray);
                  }
                  if (isShipAlive)
                  {
                      num++;
                  }
              }
          }

          return numHorizontal + numVertical;
      }


        public void EmptyBoard()
        {
            // initially empty the entire board
            for (int i = 0; i < BOARD_SIZE; i++)
            {
                for (int j = 0; j < BOARD_SIZE; j++)
                {
                    board[i, j] = CellState.Empty;
                }
            }
        }

        private static  bool IsValidCell(int xCoordinate, int yCoordinate)
        {
            return (xCoordinate > 0 && xCoordinate < BOARD_SIZE && yCoordinate > 0 && yCoordinate < BOARD_SIZE);
        }

        private bool IsShipCell(int xCoordinate, int yCoordinate)
        {
            if (!IsValidCell(xCoordinate, yCoordinate))
            {
                return false;
            }

            return (board[xCoordinate, yCoordinate] == CellState.Gray || board[xCoordinate, yCoordinate] == CellState.GrayHit);
        }

        private bool IsEmptyCell(int xCoordinate, int yCoordinate)
        {
            if (!IsValidCell(xCoordinate, yCoordinate))
            {
                return false;
            }

            return (board[xCoordinate, yCoordinate] == CellState.Empty || board[xCoordinate, yCoordinate] == CellState.EmptyHit);
        }

        private bool IsNeighborAShipCell(int xCoordinate, int yCoordinate)
        {
            return IsShipCell(xCoordinate, yCoordinate - 1) || IsShipCell(xCoordinate, yCoordinate + 1) || IsShipCell(xCoordinate - 1, yCoordinate) || IsShipCell(xCoordinate + 1, yCoordinate);
        }

        // randomly places ships of the specifized size on the board
        public void RandomlyPlaceShips(int shipCount, int shipSize)
        {
            Random rnd = new Random();

            for (int i = 0; i < shipCount; ++i)
            {
                bool isGenerated = false;
                while (!isGenerated)
                {
                    var randomX = rnd.Next(0, 9);
                    var randomY = rnd.Next(0, 9);
                    // if randomly generated cell is not empty, try another pair of coordinates in the next loop iteration
                    if (board[randomX, randomY] != CellState.Empty)
                        continue;

                    // check if we have the required number of empty cells in a row in any of the four possible directions, and check that any
                    // such cell does not have a neighboring ship cell

                    // left to right
                    if (randomX + shipSize - 1 < BOARD_SIZE && !IsNeighborAShipCell(randomX-1, randomY))
                    {
                        var canFit = true;
                        for (int j = 0; j < shipSize; ++j)
                        {
                            if (board[randomX + j, randomY] != CellState.Empty && !IsNeighborAShipCell(randomX + j, randomY))
                            {
                                canFit = false;
                                break;
                            }
                        }

                        // check for a space between neighboring ships


                        // if the ship can fit, mark all of the cells as gray
                        if (canFit)
                        {
                            for (int j = 0; j < shipSize; ++j)
                            {
                                board[randomX + j, randomY] = CellState.Gray;
                            }
                            isGenerated = true;
                        }
                    }
                    // top to bottom
                    if (randomY + shipSize - 1 < BOARD_SIZE && !IsNeighborAShipCell(randomX, randomY-1))
                    {
                        var canFit = true;
                        for (int j = 0; j < shipSize; ++j)
                        {
                            if (board[randomX, randomY + j] != CellState.Empty && !IsNeighborAShipCell(randomX, randomY + j))
                            {
                                canFit = false;
                                break;
                            }
                        }

                        // if the ship can fit, mark all of the cells as gray
                        if (canFit)
                        {
                            for (int j = 0; j < shipSize; ++j)
                            {
                                board[randomX, randomY + j] = CellState.Gray;
                            }
                            isGenerated = true;
                        }
                    }

                }
            }
        }

      public void RandomlyInitialize()
      {
          RandomlyPlaceShips(NUMBER_BATTLESHIP, SIZE_BATTLESHIP);
          RandomlyPlaceShips(NUMBER_DESTROYER, SIZE_DESTROYER);
      }

        public int ParseXCoordinate(string cell)
        {
            int xCoordinate = cell[0] - 'A';
            if (xCoordinate > BOARD_SIZE - 1)
            {
                throw new Exception("Cell format number is not correct!");
            }
            else
            {
                return xCoordinate;
            }
        }

        public int ParseYCoordinate(string cell)
      {
          int yCoordinate = Convert.ToInt32(cell.Substring(1, cell.Length - 1)) - 1;
          if (yCoordinate > BOARD_SIZE - 1)
          {
              throw new Exception("Cell format number is not correct!");
          }
          else
          {
              return yCoordinate;
          }
      }

        public ActionResult FireAMissile(int xCoordinate, int yCoordinate)
        {
            if (board[xCoordinate, yCoordinate] == CellState.Empty || board[xCoordinate, yCoordinate] == CellState.EmptyHit)
            {
                board[xCoordinate, yCoordinate] = CellState.EmptyHit;
                return ActionResult.Miss;
            }

            if (board[xCoordinate, yCoordinate] == CellState.Gray ||
                     board[xCoordinate, yCoordinate] == CellState.GrayHit)
            {
                int previousNumberOfShips = NumberOfShipsOnBoard();
                board[xCoordinate, yCoordinate] = CellState.GrayHit;
                if (previousNumberOfShips != NumberOfShipsOnBoard())
                {
                    return ActionResult.Sunk;
                }
            }
            return ActionResult.Hit;
   
        }
    }
}
