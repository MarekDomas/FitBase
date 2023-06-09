﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;

namespace CviceniDb
{
    /// <summary>
    /// Interaction logic for CreateExercise.xaml
    /// </summary>
    public partial class CreateExercise : Window
    {
        //Přečte existující cviky a dá je do listu
        private static string TrainingNamesFile = "";
        private static string CvikyStr = "";
        private static string[] Cviky = { };
        private static List<string> Cviky2 = new List<string>();
        
        private static User U= new User();
        public CreateExercise(User u)
        {
            U = u;
            TrainingNamesFile = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory,U.Name+ "Excercises.txt");
            CvikyStr = File.ReadAllText(TrainingNamesFile);
            Cviky = CvikyStr.Split("|||");
            Cviky2 = CvikyStr.Split("|||").ToList();
            //Délka - 1 se používá protože to vždy vytvoří poslední prázdnou položku
            Cviky2 = Cviky.Take(Cviky.Length - 1).ToList();
            Cviky = Cviky.Take(Cviky.Length - 1).ToArray();
            InitializeComponent();
        }

        private void HotovoButt_Click(object sender, RoutedEventArgs e)
        {
            //Zjistí zdali byli data zadána správně a zdali cvik existuje a zapíše ho do souboru
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
