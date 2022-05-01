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
         element.GridElementType = GridElementType.Snake;
         _snake.Add(element);
      }

      public GridElement GetHead()
      {
         return _snake.First();
      }

      public GridElement GetTail()
      {
         return _snake.Last();
      }
      
      public void AddHead(GridElement element)
      {
         element.GridElementType = GridElementType.Snake;
         _snake.Insert(0, element);

      }

      public GridElement GetSnakeElement(int row, int col)
      {
         return _snake.FirstOrDefault(p => p.Row == row && p.Col == col);
      }

      public void DeleteTail()
      {
         _snake.Last().GridElementType = GridElementType.Free;
         _snake.RemoveAt(_snake.Count - 1);
      }

      public string GetSnakeInfo()
      {
         StringBuilder sb = new StringBuilder("Snake length: ", 50);
         sb.Append(_snake.Count);
         sb.AppendLine();
         for (int i = 0; i < _snake.Count; i++)
         {
            sb.Append(i);
            sb.Append(": Row ");
            sb.Append(_snake[i].Row);
            sb.Append(", Column ");
            sb.Append(_snake[i].Col);
            sb.AppendLine();
         }
         return sb.ToString();
      }
   }
}
