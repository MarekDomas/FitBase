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


        public AddTrainingWin(User u)
        {
            U = u;
            InitializeComponent();
        }

        private void Hotovo_Click(object sender, RoutedEventArgs e)
        {

            string FileName = "";

            Training NewTraining = new Training();
            NewTraining.DateOfTraining = DateOnly.FromDateTime( DateOfTrainingPick.SelectedDate.Value);


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



            File.AppendAllText(TrainingNamesFile, "1");

            UserInfo USI = new UserInfo(U, FileName);
            USI.Show();
            this.Close();

        }

        private void AddLiftsButt_Click(object sender, RoutedEventArgs e)
        {
            CreateLiftWin CL = new CreateLiftWin();
            CL.Show();
            this.Close();
        }
    }
}
