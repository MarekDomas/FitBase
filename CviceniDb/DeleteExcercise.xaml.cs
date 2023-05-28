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
        private static string TrainingNamesFile = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Lifts.txt");
        private static string CvikyStr = File.ReadAllText(TrainingNamesFile);
        private static string[] Cviky = CvikyStr.Split("|||");
        private static List<string> Cviky2 = CvikyStr.Split("|||").ToList();

        private static User U = new User();
        public DeleteExcercise(User u)
        {
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

                Cviky2.Remove(LiftsBox.SelectedItem.ToString());

                File.WriteAllText(TrainingNamesFile, string.Join("|||", Cviky2));



                UserInfo UI = new UserInfo(U);
                UI.Show();
                this.Close();
            }
        }
    }
}
