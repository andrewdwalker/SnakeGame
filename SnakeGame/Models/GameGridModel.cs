using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.Models
{
   public class GameGridModel
   {
      private uint _rows;
      private uint _columns;
      private Food _food;
      private List<Snake> _snakes;
      private GridElement[,] _gridElements;
      public GameGridModel(uint rows, uint columns)//, Food food, List<Snake> snakes)
      {
         _rows = rows;
         _columns = columns;
         //_food = food;
         //_snakes = snakes;
         _gridElements = new GridElement[_rows, _columns];
      }

      public GridElement GetGridElement(uint row, uint col)
      {
         return _gridElements[row, col];
      }
   }
}
