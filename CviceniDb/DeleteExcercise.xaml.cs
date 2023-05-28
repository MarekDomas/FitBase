using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace CviceniDb
{
    /// <summary>
    /// Interakční logika pro DeleteExcercise.xaml
    /// </summary>
    public partial class DeleteExcercise : Window
    {
        //Přečte cviky ze souboru
        private static string TrainingNamesFile = "";
        private static string CvikyStr = "";
        private static string[] Cviky = { };
        private static List<string> Cviky2 = new List<string>();

        private static User U = new User();
        public DeleteExcercise(User u)
        {
            TrainingNamesFile = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Lifts.txt");
            CvikyStr = File.ReadAllText(TrainingNamesFile);
            Cviky = CvikyStr.Split("|||");
            Cviky2 = CvikyStr.Split("|||").ToList();
            U = u;
            //Délka - 1 se používá protože to vždy vytvoří poslední prázdnou položku
            Cviky2 = Cviky.Take(Cviky.Length - 1).ToList();
            Cviky = Cviky.Take(Cviky.Length - 1).ToArray();
            InitializeComponent();
            //Nahraje cviky do ComboBoxu
            LiftsBox.ItemsSource = Cviky2.Select(option => new ComboBoxItem { Content = option });
        }


        private void DeleteExcerciseButt_Click(object sender, RoutedEventArgs e)
        {
            if((LiftsBox.SelectedItem == null))
            {
                MessageBox.Show("Vyberte cvik");
            }
            else
            {

                Cviky2.Remove(((ComboBoxItem)LiftsBox.SelectedItem).Content.ToString());

                File.WriteAllText(TrainingNamesFile, "");

                File.WriteAllText(TrainingNamesFile, string.Join("|||", Cviky2));






                UserInfo UI = new UserInfo(U);
                UI.Show();
                this.Close();
            }
        }
    }
}
