using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.Models
{

   // TODO Not used. ERASE!
   public class Food
   {
      List<GridElement> _foods = new List<GridElement>();

      public Food(List<GridElement> foods)
      {
         // TODO: verify that these GridElement really are of GridElementType food!
         // And that food is non null
         // for now, simply change each to food
         _foods = foods;
         foreach (GridElement element in _foods)
         {
            element.GridElementType = GridElementType.Food;
         }
      }

      public void AddFood(GridElement food)
      {
         food.GridElementType = GridElementType.Food;
         _foods.Add(food);
      }
   }
}
