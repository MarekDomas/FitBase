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
using System.Xml.Serialization;

namespace CviceniDb
{
    /// <summary>
    /// Interaction logic for CreateExercise.xaml
    /// </summary>
    public partial class CreateExercise : Window
    {

        private static string TrainingNamesFile = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Lifts.txt");
        private static string CvikyStr = File.ReadAllText(TrainingNamesFile);
        private static string[] Cviky = CvikyStr.Split("|||");
        private static List<string> Cviky2 = CvikyStr.Split("|||").ToList();
        
        private static User U= new User();
        public CreateExercise(User u)
        {
            U = u;
            Cviky2 = Cviky.Take(Cviky.Length - 1).ToList();
            Cviky = Cviky.Take(Cviky.Length - 1).ToArray();
            InitializeComponent();
        }

        private void HotovoButt_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(ExcersiseNameBox.Text))
            {
                MessageBox.Show("Zadejte název cviku!");
                ExcersiseNameBox.Text = "";
            }
            else if(Cviky.Contains(ExcersiseNameBox.Text))
            {
                MessageBox.Show("Cvik existuje");
                ExcersiseNameBox.Text = "";
            }
            else if (ExcersiseNameBox.Text.Contains("|||"))
            {
                MessageBox.Show("Název cviku nemůže obsahovat |||");
            }
            else
            {
                string NewExercise = ExcersiseNameBox.Text;
                Cviky.Append(NewExercise);
                Cviky2.Add(NewExercise);

                File.WriteAllText(TrainingNamesFile, "");

                foreach(string Cvik in Cviky2)
                {

                    File.AppendAllText(TrainingNamesFile,Cvik + "|||");
                }

                UserInfo UI = new UserInfo(U);
                UI.Show();
                this.Close();
            }

            
        }
    }
}
