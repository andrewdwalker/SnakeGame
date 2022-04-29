﻿using SnakeGame.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace SnakeGame.Views
{
   //[ValueConversion(typeof(GridElementType), typeof(Color))]
   public class TileToColorConverter : IValueConverter
   {
      public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
      {
         if (!(value is GridElementType))
            return null;
         switch ((GridElementType)value)
         {
            case GridElementType.Food:
               return System.Windows.Media.Brushes.Green;
               break;
            case GridElementType.Free:
               return System.Windows.Media.Brushes.White;////Colors.White;
               break;
            case GridElementType.Snake:
               return System.Windows.Media.Brushes.Blue; //Colors.Blue;
               break;
            default:
               return null; // TODO. Throw exception? This is a new GridElementType that was introduced
         }

      }

      public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
      {
         throw new NotImplementedException();
      }
   }

}
