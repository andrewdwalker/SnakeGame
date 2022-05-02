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
      right = 3,
      none = 4 // used for start of game


   }
   public class GameGridModel
   {
      private uint _rows;
      private uint _columns;
      private uint _numberOfFoods;
      private List<Snake> _snakes;
      private List<List<GridElement>> _gridElements;
      private bool _gameOver = false;
      private Direction _oldDirection = Direction.none;

      public event PropertyChangedEventHandler PropertyChanged;

      public GameGridModel(uint rows, uint columns, uint numberOfFoods, uint numberOfSnakes)//, Food food, List<Snake> snakes)
      {
         NumberOfSnakes = numberOfSnakes;
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

      public uint NumberOfSnakes { get; set; }
      public bool BounceMode { get; set; }
      public bool DifficultMode { get; set; }
      public bool BackwardsMode { get; set; }
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
         NumberOfSnakes = numberOfSnakes;

         _oldDirection = Direction.none;

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
               var food = possibilities[rnd.Next(0, possibilities.Count() - 1)];
               food.GridElementType = GridElementType.Food;
               
            }
         }

         // Generate Random Snake(s)
         _snakes = new List<Snake>();
         for (int i = 0; i < numberOfSnakes; i++)
         {
            System.Windows.Media.Brush suggestedColor = (i == 0) ? System.Windows.Media.Brushes.Blue : System.Windows.Media.Brushes.Yellow; // TODO Allow for different color for snake 3,4, etc.
            var possibilities = _gridElements.SelectMany(p => p).Where(t => t.GridElementType != GridElementType.Snake && t.GridElementType != GridElementType.Food).ToList();
            if (possibilities != null && possibilities.Count() > 0)
            {
               Random rnd = new Random(i + DateTime.Now.Second);
               var snake = possibilities[rnd.Next(0, possibilities.Count() - 1)];
               snake.GridElementType = GridElementType.Snake;
               
               _snakes.Add(new Snake(snake, i));
            }
            else // since the requirements demand that every square could have food, we need this else statement
            {
               possibilities = _gridElements.SelectMany(p => p).Where(t => t.GridElementType != GridElementType.Snake).ToList();
               if (possibilities != null && possibilities.Count() > 0)
               {
                  Random rnd = new Random(i + DateTime.Now.Second);
                  var snake = possibilities[rnd.Next(0, possibilities.Count() - 1)];
                  snake.GridElementType = GridElementType.Snake;
                  
                  _snakes.Add(new Snake(snake, i));
               }
               else
               {
                  // TODO: handle error condition. For now, just throw as it is unexpected (at least for 1 snake case)
                  throw new Exception("No suitable spaces for snakes");
               }
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
      public bool MoveSnake(Direction direction, int snakeNumber, out string message)
      {
         message = "";
         Snake snake = _snakes[snakeNumber];
         var head = snake.GetHead();
         var tail = snake.GetTail();
         int newRow = 0;
         int newCol = 0;

         switch (direction)
         {
            case Direction.up:

               {
                  if (!BackwardsMode && _oldDirection == Direction.down)
                     return false;
                  newRow = (int)head.Row - 1;
                  newCol = (int)head.Col;
                  break;
               }
            case Direction.down:
               {
                  if (!BackwardsMode && _oldDirection == Direction.up)
                     return false;
                  newRow = (int)head.Row + 1;
                  newCol = (int)head.Col;
                  break;
               }
            case Direction.left:
               {
                  if (!BackwardsMode && _oldDirection == Direction.right)
                     return false;
                  newRow = (int)head.Row;
                  newCol = (int)head.Col - 1;
                  break;
               }
            case Direction.right:
               {
                  if (!BackwardsMode && _oldDirection == Direction.left)
                     return false;
                  newRow = (int)head.Row;
                  newCol = (int)head.Col + 1;
                  break;
               }
            default:
               {
                  // TODO log.  Error in logic here!
                  throw new ArgumentException("Unexpected direction!");

               }
         };
         if (HitBoundary(newRow, newCol))
         {
            if (BounceMode)
            {
               // do nothing. We can hit walls. We just don't do anything
               return false;
            }
            message = "Hit Boundary";
            DebugMessage = "Game Over. Player " + snakeNumber + " loses. " + message;
            OnPropertyChanged("DebugMessage");
            GameOver = true;
            return false;
         }
         else if (newRow == tail.Row && newCol == tail.Col)
         {
            message = "You hit your own tail";
            DebugMessage = "Game Over. Player " + snakeNumber + " loses. " + message;
            OnPropertyChanged("DebugMessage");
            GameOver = true;
            return false;

         }
         else if (DifficultMode && snake.GetSnakeElement(newRow, newCol) != null)
         {
            message = "You hit your own snake body and are in difficult model";
            DebugMessage = "Game Over. Player " + snakeNumber + " loses. " + message;
            OnPropertyChanged("DebugMessage");
            GameOver = true;
            return false;
         }
         // TODO check for hitting other snakes.  Or just other snake tails?
         else if (NumberOfSnakes > 1)
         {
            for (int i = 0; i < NumberOfSnakes; i++)
            {
               if (i == snakeNumber)
                  continue;

               if (_snakes[i].GetSnakeElement(newRow, newCol) != null)
               {
                  message = "You hit your opponent's snake body.";
                  DebugMessage = "Game Over. Player " + snakeNumber + " loses. " + message;
                  OnPropertyChanged("DebugMessage");
                  GameOver = true;
                  return false;
               }
            }
         }


         if (GetGridElement(newRow, newCol).GridElementType != GridElementType.Food)
         {
            snake.DeleteTail();
         }
         snake.AddHead(GetGridElement(newRow, newCol));
         _oldDirection = direction;


         // TODO determine who ate more food and assign a winner. 
         // and/or get rid of idea of eating all the food ends the game. Let the 2 snakes battle it out!
         if (_gridElements.SelectMany(p => p).Any(n => n.GridElementType == GridElementType.Food) == false)
         {
            message = "You win! You ate all the food.";
            DebugMessage = "Game Over: " + message;
            OnPropertyChanged("DebugMessage");
            GameOver = true;
            return true;
         }

         OnPropertyChanged("GridElements");
         return true;
      }


      public void PrintSnakeInfo()
      {
         StringBuilder sb = new StringBuilder();
         foreach (Snake snake in _snakes)
         {
            sb.Append(snake.GetSnakeInfo());
            sb.AppendLine();
            
            
         }
         DebugMessage = sb.ToString();
         OnPropertyChanged("DebugMessage");
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
