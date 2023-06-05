using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Media.Animation;

namespace RealEstateAgency
{
    /// <summary>
    /// Логика взаимодействия для WelcomeWin.xaml
    /// </summary>
    public partial class WelcomeWin : Window
    {
        public WelcomeWin()
        {
            InitializeComponent();
            DoubleAnimation progressBarAnimation = new DoubleAnimation();
            progressBarAnimation.From = 0;
            progressBarAnimation.To = 100;
            progressBarAnimation.Duration = TimeSpan.FromSeconds(1);
            LoadPb.BeginAnimation(ProgressBar.ValueProperty, progressBarAnimation);
        }

        private void LoadPb_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (LoadPb.Value == 100)
            {
                Close();
                Application.Current.MainWindow.Show();
            }
        }
    }
}
