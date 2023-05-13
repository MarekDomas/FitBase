using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Shapes;

namespace CviceniDb
{
    /// <summary>
    /// Interakční logika pro CreateLiftWin.xaml
    /// </summary>
    public partial class CreateLiftWin : Window
    {

        private static string TrainingNamesFile = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Lifts.txt");

        private static string CvikyStr = File.ReadAllText(TrainingNamesFile);
        private static string[] Cviky = CvikyStr.Split("|||");

        public CreateLiftWin()
        {
            InitializeComponent();
            Cviky = Cviky.Take(Cviky.Length - 1).ToArray();
            LiftsBox.ItemsSource = Cviky.Select(option => new ComboBoxItem { Content = option });
        }
    }
}
