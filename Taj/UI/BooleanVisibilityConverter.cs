using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace Taj.UI
{
    class BooleanVisibilityConverter : IValueConverter
    {
        #region Constructors
        public BooleanVisibilityConverter() { }
        #endregion

        public bool Reverse { get; set; }

        #region IValueConverter Members
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return ((bool)value ^ Reverse) ? Visibility.Visible : Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (((Visibility)value) == Visibility.Visible) ^ Reverse;
        }
        #endregion
    }
}