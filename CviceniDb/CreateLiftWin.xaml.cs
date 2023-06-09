﻿using Microsoft.Extensions.Options;
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
    /// Interakční logika pro CreateLiftWin.xaml
    /// </summary>
    public partial class CreateLiftWin : Window
    {
        //Přidává cviky do drop menu
        private static string TrainingNamesFile = "";
        private static string CvikyStr = "";
        private static string[] Cviky = {};
        private static string NameOfCurrentTraining = "";

        private static Training T = new Training();
        private static string LiftsFile = "";

        private static User U = new User();
        private static DateTime DT = new DateTime();
        private static bool IsEditWindow = false;

        private static string UserName = File.ReadAllText("TempUserFile.txt");

        public CreateLiftWin(string NameOfTraining, User u,bool IsOpenedFromEditT,Training t)
        {
            U = u;
            T = t;
            InitializeComponent();
            TrainingNamesFile = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory,UserName+ "Excercises.txt");
            CvikyStr = File.ReadAllText(TrainingNamesFile);
            Cviky = CvikyStr.Split("|||");
            NameOfCurrentTraining = NameOfTraining;
            Cviky = Cviky.Take(Cviky.Length - 1).ToArray(); 
            LiftsBox.ItemsSource = Cviky.Select(option => new ComboBoxItem { Content = option });
            LiftsFile = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, NameOfCurrentTraining + "Lifts" + ".xml");

            IsEditWindow = IsOpenedFromEditT;
        }

        private void PřidatButt_Click(object sender, RoutedEventArgs e)
        {
            Lift L = new Lift();

            //Zjistí zdali byli zadané správné informace. Pak zjistí jestli už existuje soubor se cviky pro daný tréning a zapíše cvik do souboru
            if (LiftsBox.SelectedItem == null || String.IsNullOrWhiteSpace(RepsBox.Text) || String.IsNullOrWhiteSpace(SetsBox.Text) || String.IsNullOrWhiteSpace(WeightBox.Text))
            {
                MessageBox.Show("Zadejte všechny informace");
            }
            else
            {

                try
                {
                    string LiftsBoxContent = LiftsBox.SelectedItem.ToString();
                    L.NameOfLift = LiftsBoxContent.Substring(LiftsBoxContent.IndexOf(':') + 2); 
                    L.Reps = int.Parse(RepsBox.Text);
                    L.Sets = int.Parse(SetsBox.Text);
                    L.Weight = int.Parse(WeightBox.Text);
                }
                catch 
                {
                    MessageBox.Show("Zadejte správné informace");
                    RepsBox.Text = string.Empty;
                    SetsBox.Text = string.Empty;
                    WeightBox.Text = string.Empty;
                }

                string LiftsFileContent = File.ReadAllText(LiftsFile);


                if (LiftsFileContent != "")
                {
                    string XMLSoub = File.ReadAllText(LiftsFile);

                    XmlSerializer serializer2 = new XmlSerializer(typeof(List<Lift>), new XmlRootAttribute("ArrayOfLift"));
                    StringReader stringReader = new StringReader(XMLSoub);
                    List<Lift> result = (List<Lift>)serializer2.Deserialize(stringReader);
                    result.Add(L);

                    StringWriter stringWriter = new StringWriter();
                    serializer2.Serialize(stringWriter, result);
                    string newXml = stringWriter.ToString();

                    File.WriteAllText(LiftsFile, newXml);
                }
                else
                {
                    List<Lift> ListOfL = new List<Lift>();
                    ListOfL.Add(L);

                    XmlSerializer serializer = new XmlSerializer(typeof(List<Lift>));
                    using (TextWriter writer = new StreamWriter(LiftsFile))
                    {
                        serializer.Serialize(writer, ListOfL);
                    }
                }


                //Zjistí zdali bylo okno otevřeno z editovacího okna a podle toho vybere konstruktor a předá mu správné data
                if (IsEditWindow)
                {
                    AddTrainingWin AT = new AddTrainingWin(T, IsEditWindow, NameOfCurrentTraining);
                    AT.Show();
                    this.Close();
                }
                else
                {
                    AddTrainingWin AT = new AddTrainingWin(NameOfCurrentTraining, DT, U);
                    AT.Show();
                    this.Close();
                }
            }

        }

        
    }
}
