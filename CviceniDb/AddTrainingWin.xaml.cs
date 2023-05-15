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
    /// Interakční logika pro AddTrainingWin.xaml
    /// </summary>
    public partial class AddTrainingWin : Window
    {

        private static string TrainingNamesFile = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Trainings.txt");
        string NumberOfTrainings = File.ReadAllText(TrainingNamesFile);
        static bool isTestUser = true;
        private static User U = new User(isTestUser);
        private static string CurrentTrainingName = "";
        private static Training T = new Training();
        private static string ExistingTrainingsFile = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ExistingTrainings.txt");
        private static string ExistingTrainingsFileContent = "";
        private static string[] ExistingTrainings = { };


        //Konstruktor který používám při otevření z UserInfo.xaml.cs
        public AddTrainingWin(User u)
        {
            ExistingTrainingsFileContent = File.ReadAllText(ExistingTrainingsFile);
            U = u;
            if (ExistingTrainingsFileContent != "")
            {
                ExistingTrainings = ExistingTrainingsFileContent.Split("|||");
                ExistingTrainings = ExistingTrainings.Take(ExistingTrainings.Length - 1).ToArray();

                string WordToremove = U.Name + ":::";
                //foreach(string s in ExistingTrainings)
                //{
                //    s = s.Replace(WordToremove, "");
                //}

                for(int i = 0; i < ExistingTrainings.Length; i++)
                {
                    if (ExistingTrainings[i].Contains(WordToremove))
                    {

                        ExistingTrainings[i] = ExistingTrainings[i].Replace(WordToremove, "");
                    }
                    else
                    {
                        ExistingTrainings[i] = ExistingTrainings[i];
                    }
                }
            }
           

            InitializeComponent();
        }

        //Konstruktor používám při otevírání z CreateLift. Dávám argumenty které pak doplním.
        public AddTrainingWin(string NameOfTraining, DateTime DatumTreningu,User u)
        {
            U = u;
            ExistingTrainingsFileContent = File.ReadAllText(ExistingTrainingsFile);
            if (ExistingTrainingsFileContent != "")
            {
                ExistingTrainings = ExistingTrainingsFileContent.Split("|||");
                ExistingTrainings = ExistingTrainings.Take(ExistingTrainings.Length - 1).ToArray();

                string WordToremove = U.Name + ":::";
                //foreach(string s in ExistingTrainings)
                //{
                //    s = s.Replace(WordToremove, "");
                //}

                for (int i = 0; i < ExistingTrainings.Length; i++)
                {
                    if (ExistingTrainings[i].Contains(WordToremove))
                    {

                       ExistingTrainings[i] = ExistingTrainings[i].Replace(WordToremove, "");
                    }
                    else
                    {
                        ExistingTrainings[i] = ExistingTrainings[i];
                    }
                }
            }
            InitializeComponent ();



            DateOfTrainingPick.SelectedDate = DatumTreningu;
            NameOfTrainingBox.Text = NameOfTraining;

            string XMLSoub =  System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, NameOfTraining + "Lifts" + ".xml");
            string XMLSoubContent = File.ReadAllText(XMLSoub);


            string _byteOrderMarkUtf8 = Encoding.UTF8.GetString(Encoding.UTF8.GetPreamble());
            if (XMLSoubContent.StartsWith(_byteOrderMarkUtf8)) 
            {
                XMLSoubContent = XMLSoub.Remove(0, _byteOrderMarkUtf8.Length);
            }

            if(XMLSoubContent != "")
            {

                XmlSerializer serializer2 = new XmlSerializer(typeof(List<Lift>), new XmlRootAttribute("ArrayOfLift"));
                StreamReader streamReader = new StreamReader(XMLSoub);
                
                List<Lift> result = (List<Lift>)serializer2.Deserialize(streamReader);

                Seznam.ItemsSource = result;
            }

            //DateOfTrainingPick.SetValue(DatePicker.SelectedDateProperty,DatumTreningu);
        }

        private void Hotovo_Click(object sender, RoutedEventArgs e)
        {


            //Vytváří soubor pro tréning
            string FileName = "";

            Training NewTraining = new Training();
            //string DateTime = DateOfTrainingPick.SelectedDate.Value.Date.ToShortDateString();

            DateTime CurrentTrainingDate = new DateTime();
            try
            {

                CurrentTrainingDate = DateOfTrainingPick.SelectedDate.Value.Date;
            }
            catch
            {
                MessageBox.Show("Zadejte datum!");
            }
            NewTraining.DateOfTraining = CurrentTrainingDate;

            if (ExistingTrainingsFileContent != "")
            {
                ExistingTrainings = ExistingTrainingsFileContent.Split("|||");
                ExistingTrainings = ExistingTrainings.Take(ExistingTrainings.Length - 1).ToArray();

                string WordToremove = U.Name + ":::";
                //foreach(string s in ExistingTrainings)
                //{
                //    s = s.Replace(WordToremove, "");
                //}

                for (int i = 0; i < ExistingTrainings.Length; i++)
                {
                    if (ExistingTrainings[i].Contains(WordToremove))
                    {

                        ExistingTrainings[i] = ExistingTrainings[i].Replace(WordToremove, "");
                    }
                    else
                    {
                        ExistingTrainings[i] = ExistingTrainings[i];
                    }
                }
            }





            File.AppendAllText(ExistingTrainingsFile,  U.Name+":::"+NameOfTrainingBox.Text + "|||");

            //Pokud není vyplněný název tak se vytvoří sám
            /*
            if (String.IsNullOrWhiteSpace(NameOfTrainingBox.Text))
            {
                using (FileStream fs = File.Create("Workout#" + NumberOfTrainings.Length.ToString() + ".xml"))
                {
                    byte[] content = Encoding.UTF8.GetBytes("");
                    fs.Write(content, 0, content.Length);
                }

                FileName = "Workout#" + NumberOfTrainings.Length.ToString() + ".xml";

               // File.AppendAllText(TrainingNamesFile, "1");
            }*/
            
            
            using (FileStream fs = File.Create(NameOfTrainingBox.Text + ".xml"))
            {
                byte[] content = Encoding.UTF8.GetBytes("");
                fs.Write(content, 0, content.Length);
            }

            FileName = NameOfTrainingBox.Text + ".xml";

            NewTraining.NameOfTraining = NameOfTrainingBox.Text;
                

            

            NewTraining.OwnerOfTraining = U.Name;

            XmlSerializer serializer = new XmlSerializer(typeof(Training));
            
            using (TextWriter writer = new StreamWriter(FileName))
            {
                serializer.Serialize(writer, NewTraining);
            }

            
            string TrainingLiftsFile = NewTraining.NameOfTraining + "Lifts" + ".xml";
            if (!File.Exists(TrainingLiftsFile ))
            {
                using (FileStream fs = File.Create(TrainingLiftsFile))
                {
                    byte[] content = Encoding.UTF8.GetBytes("");
                    fs.Write(content, 0, content.Length);
                }
            }

            T.NameOfTraining = NewTraining.NameOfTraining;
            //T.OwnerOfTraining = NewTraining.OwnerOfTraining;

            File.AppendAllText(TrainingNamesFile, "1");



            if (String.IsNullOrWhiteSpace(NameOfTrainingBox.Text))
            {
                MessageBox.Show("Zadejte název tréningu");
                NameOfTrainingBox.Text = "";
            }
            else if (ExistingTrainings.Contains(NameOfTrainingBox.Text))
            {
                MessageBox.Show("Název tréningu je už zabraný");
                NameOfTrainingBox.Text = "";
            }
            else
            {
                UserInfo USI = new UserInfo(U, FileName);
                USI.Show();
                this.Close();
            }


        }

        
        

        private void AddLiftsButt_Click(object sender, RoutedEventArgs e)
        {
            CurrentTrainingName = NameOfTrainingBox.Text;
            DateTime CurrentTrainingDateTime = DateOfTrainingPick.SelectedDate.Value.Date;
            DateOnly CurrentTrainingDate = DateOnly.FromDateTime(CurrentTrainingDateTime);

            string TrainingLiftsFile = CurrentTrainingName + "Lifts" + ".xml";
            if (!File.Exists(TrainingLiftsFile))
            {
                using (FileStream fs = File.Create(TrainingLiftsFile))
                {
                    byte[] content = Encoding.UTF8.GetBytes("");
                    fs.Write(content, 0, content.Length);
                }
            }
            CreateLiftWin CL = new CreateLiftWin(CurrentTrainingName,CurrentTrainingDateTime,U);
            CL.Show();
            this.Close();
        }
    }
}
