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
    /// Interakční logika pro AddTrainingWin.xaml
    /// </summary>
    public partial class AddTrainingWin : Window
    {

        private static string TrainingNamesFile = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Trainings.txt");
        string NumberOfTrainings = File.ReadAllText(TrainingNamesFile);

        public AddTrainingWin()
        {
            InitializeComponent();
        }

        private void Hotovo_Click(object sender, RoutedEventArgs e)
        {
            if(String.IsNullOrWhiteSpace(NameOfTrainingBox.Text))
            {
                using (FileStream fs = File.Create("Workout#" + NumberOfTrainings.Length.ToString() + ".xml"))
                {
                    byte[] content = Encoding.UTF8.GetBytes("");
                    fs.Write(content, 0, content.Length);
                }

                File.AppendAllText(TrainingNamesFile, "1");
            }
            else
            {
                using(FileStream fs = File.Create(NameOfTrainingBox.Text + ".xml"))
                {
                    byte[] content = Encoding.UTF8.GetBytes("");
                    fs.Write(content, 0, content.Length);
                }
                File.AppendAllText(TrainingNamesFile, "1");

            }
        }

        private void AddLiftsButt_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
