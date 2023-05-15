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


        //Konstruktor který používám při otevření z UserInfo.xaml.cs
        public AddTrainingWin(User u)
        {
            U = u;

           

            InitializeComponent();
        }

        //Konstruktor používám při otevírání z CreateLift. Dávám argumenty které pak doplním.
        public AddTrainingWin(string NameOfTraining, DateOnly DatumTreningu,User u)
        {
            U = u;
            InitializeComponent ();
            NameOfTrainingBox.Text = NameOfTraining;

            string XMLSoub =  System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, NameOfTraining + "Lifts" + ".xml");
            string XMLSoubContent = File.ReadAllText(XMLSoub);

            if(XMLSoubContent != "")
            {
                XmlSerializer serializer2 = new XmlSerializer(typeof(List<Lift>), new XmlRootAttribute("ArrayOfLift"));
                StringReader stringReader = new StringReader(XMLSoub);
                List<Lift> result = (List<Lift>)serializer2.Deserialize(stringReader);

                Seznam.ItemsSource = result;
            }

            //DateOfTrainingPick.SetValue(DatePicker.SelectedDateProperty,DatumTreningu);
        }

        private void Hotovo_Click(object sender, RoutedEventArgs e)
        {


            //Vytváří soubor pro tréning
            string FileName = "";

            Training NewTraining = new Training();
            DateTime CurrentTrainingDateTime = DateOfTrainingPick.SelectedDate.Value;
            DateOnly CurrentTrainingDate = DateOnly.FromDateTime(CurrentTrainingDateTime);
            NewTraining.DateOfTraining = CurrentTrainingDate;

            //Pokud není vyplněný název tak se vytvoří sám
            if (String.IsNullOrWhiteSpace(NameOfTrainingBox.Text))
            {
                using (FileStream fs = File.Create("Workout#" + NumberOfTrainings.Length.ToString() + ".xml"))
                {
                    byte[] content = Encoding.UTF8.GetBytes("");
                    fs.Write(content, 0, content.Length);
                }

                FileName = "Workout#" + NumberOfTrainings.Length.ToString() + ".xml";

               // File.AppendAllText(TrainingNamesFile, "1");
            }
            else
            {
                using (FileStream fs = File.Create(NameOfTrainingBox.Text + ".xml"))
                {
                    byte[] content = Encoding.UTF8.GetBytes("");
                    fs.Write(content, 0, content.Length);
                }

                FileName = NameOfTrainingBox.Text + ".xml";

                NewTraining.NameOfTraining = NameOfTrainingBox.Text;
                

            }

            NewTraining.OwnerOfTraining = U.Name;

            XmlSerializer serializer = new XmlSerializer(typeof(Training));
            
            using (TextWriter writer = new StreamWriter(FileName))
            {
                serializer.Serialize(writer, NewTraining);
            }

            //Pokračovat tady!!!
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

            UserInfo USI = new UserInfo(U, FileName);
            USI.Show();
            this.Close();

        }

        
        

        private void AddLiftsButt_Click(object sender, RoutedEventArgs e)
        {
            CurrentTrainingName = NameOfTrainingBox.Text;
            DateTime CurrentTrainingDateTime = DateOfTrainingPick.SelectedDate.Value;
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
            CreateLiftWin CL = new CreateLiftWin(CurrentTrainingName,CurrentTrainingDate,U);
            CL.Show();
            this.Close();
        }
    }
}
