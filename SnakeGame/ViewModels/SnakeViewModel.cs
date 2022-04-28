using SnakeGame.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.ViewModels
{
   public class SnakeViewModel : ViewModelBase
   {
      private GameGridModel _gameGridModel;
      private Snake _snake;
      private Food _food;

      public SnakeViewModel(GameGridModel gameGridModel, Snake snake, Food food)
      {
         _gameGridModel = gameGridModel;
         _snake = snake;
         _food = food;
      }
   }
}
