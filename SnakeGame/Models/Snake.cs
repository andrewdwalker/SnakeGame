using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.Models
{
   public class Snake
   {
      private List<GridElement> _snake = new List<GridElement>();

      public Snake(GridElement element)
      {
         _snake.Add(element);
      }
   }
}
