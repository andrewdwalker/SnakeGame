using System;
using System.Collections.Generic;
using System.ComponentModel;
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
   public class GridElement : INotifyPropertyChanged
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

      public GridElementType GridElementType
      {
         get
         {
            return _gridElementType;
         }
         set
         {
            _gridElementType = value;
            OnPropertyChanged("GridElementType");
         }
      }
      public uint Row
      {
         get
         {
            return _row;
         }
      }
      public uint Col
      {
         get
         {
            return _col;
         }
      }

      public event PropertyChangedEventHandler PropertyChanged;

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
