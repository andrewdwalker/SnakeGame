using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace SnakeGame.Views
{

   [ValueConversion(typeof(bool), typeof(bool))]
   public class InverseBooleanConverter : IValueConverter
   {
      #region IValueConverter Members

      public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
      {
         if (targetType != typeof(bool))
         {
            throw new InvalidOperationException("The target must be a nullable boolean");
         }
         bool b = (bool)value;
         return !b;
      }

      public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
      {
         return !((bool)value);
      }

      #endregion
   }
   public class BoolToVisibilityConverter : BoolToValueConverter<Visibility>
   {
      #region Constructors and Destructors

      public BoolToVisibilityConverter()
      {
         this.TrueValue = Visibility.Collapsed;
         this.FalseValue = Visibility.Visible;
      }

      #endregion
   }

   ///
   /// The following is from:
   /// https://stackoverflow.com/questions/17290409/wpf-converter-property
   /// Works great!
   /// <summary>
   /// Use as the base class for BoolToXXX style converters
   /// </summary>
   /// <typeparam name="T"></typeparam>    
   public abstract class BoolToValueConverter<T> : MarkupExtension, IValueConverter
   {
      #region Constructors and Destructors

      protected BoolToValueConverter()
      {
         this.TrueValue = default(T);
         this.FalseValue = default(T);
      }

      #endregion

      #region Public Properties

      public T FalseValue { get; set; }

      public T TrueValue { get; set; }

      #endregion

      #region Public Methods and Operators

      public object Convert(object value, Type targetType,
                            object parameter, CultureInfo culture)
      {
         return System.Convert.ToBoolean(value) ? this.TrueValue : this.FalseValue;
      }

      // Override if necessary
      public virtual object ConvertBack(object value, Type targetType,
                                        object parameter, CultureInfo culture)
      {
         return value.Equals(this.TrueValue);
      }

      public override object ProvideValue(IServiceProvider serviceProvider)
      {
         return this;
      }

      #endregion
   }
}
