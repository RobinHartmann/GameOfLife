using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GameOfLife
{
    /// <summary>
    /// Interaktionslogik für Cell.xaml
    /// </summary>
    public partial class Cell : UserControl
    {
        public static readonly DependencyProperty IsCheckedProperty =
            DependencyProperty.Register("IsChecked", typeof(bool), typeof(Cell), new FrameworkPropertyMetadata(false, OnIsCheckedPropertyChanged));

        public Cell()
        {
            InitializeComponent();
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            IsChecked = !IsChecked;
            base.OnMouseLeftButtonDown(e);
        }

        public bool IsChecked
        {
            get
            {
                return (bool)GetValue(IsCheckedProperty);
            }
            set
            {
                SetValue(IsCheckedProperty, value);
            }
        }

        private static void OnIsCheckedPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            Cell sourceControl = (source as Cell);

            if ((bool)e.NewValue)
            {
                sourceControl.Background = Brushes.DeepSkyBlue;
            }
            else
            {
                sourceControl.Background = Brushes.Transparent;
            }
        }
    }
}
