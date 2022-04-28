using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.Models
{
   public enum GridElementType
   {
      Free = 0,
      Food = 1,
      Snake = 2,
      //SnakeBody = 2,
      //SnakeHead = 3
   }
   public class GridElement
   {
      private uint _row;
      private uint _col;
      private GridElementType _gridElementType = GridElementType.Free;
      public GridElement(uint row, uint col, GridElementType gridElementType = GridElementType.Free)
      {
         _row = row;
         _col = col;
         _gridElementType = gridElementType;
      }

      public GridElementType GridElementType { get; set; }
   }

   
}
