using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.Models
{
   public enum Direction
   {
      up = 0,
      down = 1,
      left = 2,
      right = 3

   }
   public class GameGridModel
   {
      private uint _rows;
      private uint _columns;
      private uint _numberOfFoods;
      //private Food _food;
      private List<Snake> _snakes;
      private List<List<GridElement>> _gridElements;
      bool _gameOver = false;

      public event PropertyChangedEventHandler PropertyChanged;

      public GameGridModel(uint rows, uint columns, uint numberOfFoods, uint numberOfSnakes)//, Food food, List<Snake> snakes)
      {
         SetupGameGridModel(rows, columns, numberOfFoods, numberOfSnakes);

      }

      public GridElement GetGridElement(int row, int col)
      {
         return _gridElements[row][col];
      }

      public List<List<GridElement>> GridElements
      {
         get
         {
            return _gridElements;
         }
      }

      public string DebugMessage { get; set; }

      public uint Rows { get => _rows; private set => _rows = value; }
      public uint Columns { get => _columns; private set => _columns = value; }
      public uint NumberOfFoods { get => _numberOfFoods; private set => _numberOfFoods = value; }

      public bool GameOver
      {
         get
         {
            return _gameOver;
         }
         set
         {
            _gameOver = value;
            OnPropertyChanged("GameOver");
         }
      }

      public void SetupGameGridModel(uint rows, uint columns, uint numberOfFoods, uint numberOfSnakes)//, Food food, List<Snake> snakes)
      {
         Rows = rows;
         Columns = columns;
         NumberOfFoods = numberOfFoods;
         //_food = food;
         //_snakes = snakes;
         //_gridElements = new GridElement[_rows, _columns];
         _gridElements = new List<List<GridElement>>();
         for (int i = 0; i < Rows; i++)
         {
            _gridElements.Add(new List<GridElement>());

            for (uint j = 0; j < Columns; j++)
            {
               _gridElements[i].Add(new GridElement((uint)i, j, GridElementType.Free));
            }
         }

         // Generate Random Food spots
         for (int i = 0; i < numberOfFoods; i++)
         {
            var possibilities = _gridElements.SelectMany(p => p).Where(t => t.GridElementType == GridElementType.Free).ToList();
            if (possibilities != null && possibilities.Count() > 0) // TODO: handle error condition
            {
               Random rnd = new Random(i + DateTime.Now.Second);
               possibilities[rnd.Next(0, possibilities.Count() - 1)].GridElementType = GridElementType.Food;
            }
         }

         // Generate Random Snake(s)
         _snakes = new List<Snake>();
         for (int i = 0; i < numberOfSnakes; i++)
         {
            var possibilities = _gridElements.SelectMany(p => p).Where(t => t.GridElementType != GridElementType.Snake).ToList();
            if (possibilities != null && possibilities.Count() > 0) // TODO: handle error condition
            {
               Random rnd = new Random(i + DateTime.Now.Second);
               var snake = possibilities[rnd.Next(0, possibilities.Count() - 1)];
               snake.GridElementType = GridElementType.Snake;
               _snakes.Add(new Snake(snake));
            }
         }

         OnPropertyChanged("GridElements");
         GameOver = false;
      }

      /// <summary>
      /// Todo: Refactor!
      /// </summary>
      /// <param name="direction"></param>
      /// <param name="message"></param>
      /// <returns></returns>
      public bool MoveSnake(Direction direction, out string message)
      {
         message = "";
         Snake snake = _snakes[0];  // TODO, change so we can move any snake
         var head = snake.GetHead();
         var tail = snake.GetTail();
         int newRow = 0;
         int newCol = 0;
         switch (direction)
         {
            case Direction.up:
               {
                  newRow = (int)head.Row - 1;
                  newCol = (int)head.Col;
                  break;
               }
            case Direction.down:
               {
                  newRow = (int)head.Row + 1;
                  newCol = (int)head.Col;
                  break;
               }
            case Direction.left:
               {
                  newRow = (int)head.Row;
                  newCol = (int)head.Col - 1;
                  break;
               }
            case Direction.right:
               {
                  newRow = (int)head.Row;
                  newCol = (int)head.Col + 1;
                  break;
               }
            default:
               {
                  // TODO log.  Error in logic here!
                  throw new ArgumentException();
                  break;
               }
         };
         if (HitBoundary(newRow, newCol))
         {
            message = "Hit Boundary";
            DebugMessage = "Game Over: " + message;
            OnPropertyChanged("DebugMessage");
            GameOver = true;
            return false;
         }
         else if (newRow == tail.Row && newCol == tail.Col)
         {
            message = "You hit your tail";
            DebugMessage = "Game Over: " + message;
            OnPropertyChanged("DebugMessage");
            GameOver = true;
            return false;
            
         }
         // TODO check for hitting other snakes.  Or just other snake tails?
         // TOD check if food all eaten
         else
         {
            if (GetGridElement(newRow, newCol).GridElementType != GridElementType.Food)
            {
               snake.DeleteTail();
            }
            snake.AddHead(GetGridElement(newRow, newCol));
         }

         OnPropertyChanged("GridElements");
         return true;
      }


      public void PrintSnakeInfo()
      {
         foreach (Snake snake in _snakes)
         {
            DebugMessage = snake.GetSnakeInfo();
            OnPropertyChanged("DebugMessage");
         }
      }

      private bool HitBoundary(int newRow, int newCol)
      {
         if (newRow < 0 || newRow >= _rows || newCol < 0 || newCol >= _columns)
         {
            return true;
         }
         return false;
      }

      protected virtual void OnPropertyChanged(string propertyName)
      {

         PropertyChangedEventHandler handler = this.PropertyChanged;
         if (handler != null)
         {
            var e = new PropertyChangedEventArgs(propertyName);
            handler(this, e);
         }
      }
   }
}
