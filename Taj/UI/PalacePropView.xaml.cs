using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Taj.UI
{
    /// <summary>
    /// Interaction logic for PalacePropView.xaml
    /// </summary>
    public partial class PalacePropView : UserControl
    {
        public static readonly DependencyProperty XProperty = DependencyProperty.Register("X", typeof(double), typeof(PalacePropView));
        public static readonly DependencyProperty YProperty = DependencyProperty.Register("Y", typeof(double), typeof(PalacePropView));

        public PalacePropView()
        {
            var rand = new Random();

            X = rand.NextDouble() * 500;
            Y = rand.NextDouble() * 500;
            InitializeComponent();
        }

        public double X
        {
            get { return (double)this.GetValue(XProperty); }
            set { this.SetValue(XProperty, value); }
        }

        public double Y
        {
            get { return (double)this.GetValue(YProperty); }
            set { this.SetValue(YProperty, value); }
        }
    }
}
