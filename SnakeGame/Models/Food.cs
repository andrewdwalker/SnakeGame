using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.Models
{
   public class Food
   {
      GridElement[] _food;

      public Food(GridElement[] food)
      {
         // TODO: verify that these GridElement really are of GridElementType food!
         // And that food is non null
         // for now, simply change each to food
         _food = food;
         foreach (GridElement element in _food)
         {
            element.GridElementType = GridElementType.Food;
         }
      }
   }
}
