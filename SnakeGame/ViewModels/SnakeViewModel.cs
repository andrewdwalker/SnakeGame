using SnakeGame.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SnakeGame.ViewModels
{
   public class SnakeViewModel : ViewModelBase
   {
      private GameGridModel _gameGridModel;
      //private Snake _snake;
      //private Food _food;
      private uint _numberOfRows = 12;
      private uint _numberOfCols = 12;
      private uint _numberOfFoods = 12;

      private ICommand _startNewGame = null;
      private ICommand _moveSnake = null;
      private ICommand _printSnakeInfo = null;
      private ICommand _numberOfPlayersCommand;

      List<List<GridElement>> _items = new List<List<GridElement>>();

      public SnakeViewModel(GameGridModel gameGridModel)//, Snake snake, Food food)
      {
         _gameGridModel = gameGridModel;

         _gameGridModel.PropertyChanged += _gameGridModel_PropertyChanged;

         DifficultMode = false;
         BackwardsMode = true;
         BounceMode = false;

         NumberOfPlayersCollection = new ObservableCollection<NumberOfPlayersClass>();
         foreach (NumberOfPlayersEnum value in Enum.GetValues(typeof(NumberOfPlayersEnum)))
         {
            NumberOfPlayersClass myClass = new NumberOfPlayersClass();
            myClass.Player = value;
            myClass.IsChecked = value == NumberOfPlayersEnum.One ? true : false; // default to using 1 players
            myClass.Title = Enum.GetName(typeof(NumberOfPlayersEnum), value);
            NumberOfPlayersCollection.Add(myClass);
         }
      }



      #region Properties
      public string DebugMessage
      {
         get
         {
            return _gameGridModel.DebugMessage;
         }
      }
      public List<List<GridElement>> Items
      {
         get
         {
            return _gameGridModel.GridElements;
         }
      }

      public List<uint> NumberOfPossibleRows
      {
         get
         {
            return new List<uint>() { 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
         }
      }

      public uint NumberOfRows
      {
         get
         {
            return _numberOfRows;
         }
         set
         {
            _numberOfRows = value;
            NumberOfFoods = Math.Min(_numberOfFoods, _numberOfRows * _numberOfCols); ;
            OnPropertyChanged("NumberOfPossibleFoods");

         }
      }

      public List<uint> NumberOfPossibleCols
      {
         get
         {
            return new List<uint>() { 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
         }
      }

      public uint NumberOfCols
      {
         get
         {
            return _numberOfCols;
         }
         set
         {
            _numberOfCols = value;
            NumberOfFoods = Math.Min(_numberOfFoods, _numberOfRows * _numberOfCols); ;
            OnPropertyChanged("NumberOfPossibleFoods");

         }
      }

      public List<uint> NumberOfPossibleFoods
      {
         get
         {
            //return new List<uint>() { 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
            List<uint> possibilites = new List<uint>();
            uint numberOfPossibilities = NumberOfCols * NumberOfRows;
            for (uint i = 1; i <= numberOfPossibilities; i++)
            {
               possibilites.Add(i);
            }
            return possibilites;
         }
      }

      public uint NumberOfFoods
      {
         get
         {
            _numberOfFoods = Math.Min(_numberOfFoods, (uint)NumberOfPossibleFoods.Count);
            return _numberOfFoods;
         }
         set
         {
            _numberOfFoods = value;
            OnPropertyChanged("NumberOfFoods");

         }
      }

      public bool GameOver
      {
         get => _gameGridModel.GameOver;
      }

      public bool BounceMode
      {
         get => _gameGridModel.BounceMode;
         set => _gameGridModel.BounceMode = value;
      }

      public bool DifficultMode
      {
         get => _gameGridModel.DifficultMode;
         set => _gameGridModel.DifficultMode = value;
      }

      public bool BackwardsMode
      {
         get => _gameGridModel.BackwardsMode;
         set => _gameGridModel.BackwardsMode = value;
      }

      public ObservableCollection<NumberOfPlayersClass> NumberOfPlayersCollection { get; private set; }


      #endregion

      #region ICommmand and implementations
      public ICommand NumberOfPlayersCommand
      {
         get
         {
            if (_numberOfPlayersCommand == null)
            {
               _numberOfPlayersCommand = new RelayCommand(new Action<object>(ResolveCheckBoxes));

            }
            return _numberOfPlayersCommand;
         }
      }

      public ICommand StartNewGame
      {
         get
         {
            if (_startNewGame == null)
            {
               _startNewGame = new RelayCommand(param => StartNewGameImplementation());
            }
            return _startNewGame;
         }
      }

      private void StartNewGameImplementation()
      {
         NumberOfPlayersClass arg = NumberOfPlayersCollection.First<NumberOfPlayersClass>(t => t.IsChecked == true);
         if (arg == null)
         {
            // BIG problem.  Log and find out why
            throw new Exception("Could not resolve number of players in StartNewGameImplementation");
         }
         _gameGridModel.SetupGameGridModel(NumberOfRows, NumberOfCols, NumberOfFoods, Convert.ToUInt32(arg.Player)); 
      }

      public ICommand MoveSnake
      {
         get
         {
            if (_moveSnake == null)
            {
               _moveSnake = new RelayCommand(new Action<object>(MoveSnakeImplementation));

            }
            return _moveSnake;
         }
      }

      public ICommand PrintSnakeInfo
      {
         get
         {
            if (_printSnakeInfo == null)
            {
               _printSnakeInfo = new RelayCommand(p => PrintSnakeInfoImplementation());
            }
            return _printSnakeInfo;
         }
      }

      private void PrintSnakeInfoImplementation()
      {
         _gameGridModel.PrintSnakeInfo();
      }

      private void MoveSnakeImplementation(object direction)
      {
         int snakeNumber = 0;
         Direction myDirection = Direction.none;
         // TODO Don't use strings for this in ViewModel. Enum or at least an int! Can we use our enum in View?
         string test = Convert.ToString(direction).ToLower();
         switch (test)
         {
            case "w":
               snakeNumber = 1;
               goto case "up";
            case "up":
               {
                  myDirection = Direction.up;
                  break;
               }

            case "s":
               snakeNumber = 1;
               goto case "down";
            case "down":
               {
                  myDirection = Direction.down;
                  break;
               }

            case "a":
               snakeNumber = 1;
               goto case "left";
            case "left":
               {
                  myDirection = Direction.left;
                  break;
               }

            case "d":
               snakeNumber = 1;
               goto case "right";
            case "right":
               {
                  myDirection = Direction.right;
                  break;
               }

            default:
               throw new ArgumentException("Unrecognized direction");

         }
         string message = "";
         bool result = _gameGridModel.MoveSnake(myDirection, snakeNumber, out message);
         if (!result)
         {
            Console.WriteLine("Game over: " + message);
         }
      }



      #endregion

      private void _gameGridModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
      {
         switch (e.PropertyName)
         {
            case "GridElements":
               {
                  OnPropertyChanged("Items");
                  break;
               }
            case "DebugMessage":
               {
                  OnPropertyChanged("DebugMessage");
                  break;
               }
            case "GameOver":
               {
                  OnPropertyChanged("GameOver");
                  break;
               }
         }

      }

      private void ResolveCheckBoxes(object checkBoxNumber)
      {

         NumberOfPlayersEnum myEnum = (NumberOfPlayersEnum)checkBoxNumber;
         NumberOfPlayersClass theClass = NumberOfPlayersCollection.First<NumberOfPlayersClass>(t => t.Player == myEnum);

         // ok, they want to check this one, let them and uncheck all else
         foreach (NumberOfPlayersClass iter in NumberOfPlayersCollection)
         {
            iter.IsChecked = false;
         }
         theClass.IsChecked = true;



      }

      #region internal classes

      public enum NumberOfPlayersEnum
      {
         One = 1,
         Two = 2,
         //Three = 3,
         //Four = 4,
      }
      public class NumberOfPlayersClass : ViewModelBase
      {
         public NumberOfPlayersClass()
         {
            IsChecked = false;
         }
         public NumberOfPlayersEnum Player { get; set; }
         private bool _isChecked = false;

         public bool IsChecked
         {
            get
            {
               return _isChecked;
            }
            set
            {
               _isChecked = value;
               OnPropertyChanged("IsChecked");
            }

         }
         public string Title { get; set; }

      }

      #endregion internal classes


   }
}
