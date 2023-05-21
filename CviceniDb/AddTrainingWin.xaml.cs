using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
            U = u;          
            InitializeComponent();
        }

        //Konstruktor používám při otevírání z CreateLift. Dávám argumenty které pak doplním.
        public AddTrainingWin(string NameOfTraining, DateTime DatumTreningu,User u)
        {
            U = u;
           
            InitializeComponent ();

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
        }
        private static Training NewTraining = new Training();
        private void Hotovo_Click(object sender, RoutedEventArgs e)
        {
            //Vytváří soubor pro tréning
            string FileName = "";
            DateTime CurrentTrainingDate = new DateTime();
            try
            {
                CurrentTrainingDate = DateOfTrainingPick.SelectedDate.Value.Date;
                NewTraining.DateOfTraining = CurrentTrainingDate;

                
                if(File.Exists(NameOfTrainingBox.Text + ".xml") && !EdititngWindow)
                {
                    MessageBox.Show("Jméno tohoto tréningu existuje nebo ho má zabraný někdo jiný");
                    NameOfTrainingBox.Text = "";
                }
                else if (File.Exists(NameOfTrainingBox.Text + ".xml") && EdititngWindow && DateOfTrainingPick.SelectedDate.Value.Date != null)
                {
                    NewTraining.NameOfTraining = NameOfTrainingBox.Text;
                    if (NewTraining.OwnerOfTraining == null)
                    {
                        NewTraining.OwnerOfTraining = U.Name;
                    }
                    NewTraining.DateOfTraining = DateOfTrainingPick.SelectedDate.Value.Date;
                    FileName = NameOfTrainingBox.Text + ".xml";
                    XmlSerializer serializer = new XmlSerializer(typeof(Training));
                    string TrainingLiftsFile = NewTraining.NameOfTraining + "Lifts" + ".xml";

                    if (!File.Exists(TrainingLiftsFile))
                    {
                        using (FileStream fs = File.Create(TrainingLiftsFile))
                        {
                            byte[] content = Encoding.UTF8.GetBytes("");
                            fs.Write(content, 0, content.Length);
                        }
                    }
                    T.NameOfTraining = NewTraining.NameOfTraining;

                    using (FileStream fs = File.Create(NameOfTrainingBox.Text + ".xml"))
                    {
                        byte[] content = Encoding.UTF8.GetBytes("");
                        fs.Write(content, 0, content.Length);
                    }
                    using (StreamWriter writer = new StreamWriter(FileName))
                    {
                        serializer.Serialize(writer, NewTraining);
                    }
                    UserInfo USI = new UserInfo(U, FileName);
                    USI.Show();


                    this.Close();

                }
                else if (File.Exists(NameOfTrainingBox.Text + ".xml") && !EdititngWindow)
                {
                    MessageBox.Show("Jméno tohoto tréningu existuje nebo ho má zabraný někdo jiný");
                    NameOfTrainingBox.Text = "";
                }
                else
                {
                    NewTraining.NameOfTraining = NameOfTrainingBox.Text;
                    if (NewTraining.OwnerOfTraining == null)
                    {
                        NewTraining.OwnerOfTraining = U.Name;
                    }
                    NewTraining.DateOfTraining = DateOfTrainingPick.SelectedDate.Value.Date;
                    FileName = NameOfTrainingBox.Text + ".xml";
                    XmlSerializer serializer = new XmlSerializer(typeof(Training));
                    string TrainingLiftsFile = NewTraining.NameOfTraining + "Lifts" + ".xml";


                    if (!File.Exists(TrainingLiftsFile))
                    {
                        using (FileStream fs = File.Create(TrainingLiftsFile))
                        {
                            byte[] content = Encoding.UTF8.GetBytes("");
                            fs.Write(content, 0, content.Length);
                        }
                    }
                    T.NameOfTraining = NewTraining.NameOfTraining;


                    File.AppendAllText(TrainingNamesFile, "1");



                    if (String.IsNullOrWhiteSpace(NameOfTrainingBox.Text))
                    {
                        MessageBox.Show("Zadejte název tréningu");
                        NameOfTrainingBox.Text = "";
                    }

                    else
                    {
                        using (FileStream fs = File.Create(NameOfTrainingBox.Text + ".xml"))
                        {
                            byte[] content = Encoding.UTF8.GetBytes("");
                            fs.Write(content, 0, content.Length);
                        }
                        using (StreamWriter writer = new StreamWriter(FileName))
                        {
                            serializer.Serialize(writer, NewTraining);
                        }
                        UserInfo USI = new UserInfo(U, FileName);
                        USI.Show();


                        this.Close();
                    }
                }
            }
            catch
            {
                MessageBox.Show("Zadejte datum!");
            }
        }

        


        private static bool EdititngWindow = false;

        private void AddLiftsButt_Click(object sender, RoutedEventArgs e)
        {
            CurrentTrainingName = NameOfTrainingBox.Text;


            string TrainingLiftsFile = CurrentTrainingName + "Lifts" + ".xml";
            if (!File.Exists(TrainingLiftsFile))
            {
                using (FileStream fs = File.Create(TrainingLiftsFile))
                {
                    byte[] content = Encoding.UTF8.GetBytes("");
                    fs.Write(content, 0, content.Length);
                }
            }


            CreateLiftWin CL = new CreateLiftWin(CurrentTrainingName,U,EdititngWindow,NewTraining);
            CL.Show();
            this.Close();
        }

        private static Training EditT = new Training();
        public AddTrainingWin(Training editT, bool IsOpenedFromEditT,string NameOfTraining)
        {
            EdititngWindow = IsOpenedFromEditT;
            EditT = editT;
            InitializeComponent();
            NameOfTrainingBox.Text = NameOfTraining;


            string XMLSoub = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, NameOfTraining+ "Lifts" + ".xml");
            string XMLSoubContent = File.ReadAllText(XMLSoub);

            if (XMLSoubContent != String.Empty)
            {

                XmlSerializer serializer2 = new XmlSerializer(typeof(List<Lift>), new XmlRootAttribute("ArrayOfLift"));
                StreamReader streamReader = new StreamReader(XMLSoub);

                List<Lift> result = (List<Lift>)serializer2.Deserialize(streamReader);

                Seznam.ItemsSource = result;
            }

            NameOfTrainingBox.IsReadOnly = true;
        }

        private void DeleteLift_Click(object sender, RoutedEventArgs e)
        {
            
            if(Seznam.SelectedItem != null)
            {
                Lift DelLift = Seznam.SelectedItem as Lift;

                string XMLSoub = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, NameOfTrainingBox.Text+ "Lifts" + ".xml");

                XmlSerializer serializer2 = new XmlSerializer(typeof(List<Lift>));
                StreamReader streamReader = new StreamReader(XMLSoub);

                List<Lift> result = (List<Lift>)serializer2.Deserialize(streamReader);


                for(int i = 0; i < result.Count; i++)
                {
                    if (result[i].NameOfLift == DelLift.NameOfLift&& result[i].Reps== DelLift.Reps&& result[i].Sets== DelLift.Sets&& result[i].Weight== DelLift.Weight)
                    {
                        result.Remove((Lift)result[i]);
                        break;
                    }
                }

                streamReader.Close();
                

                File.WriteAllText(XMLSoub, "");
                using (StreamWriter writer = new StreamWriter(XMLSoub))
                {
                    serializer2.Serialize(writer, result);
                }


                IEditableCollectionView items = Seznam.Items;

                items.Remove(Seznam.SelectedItem);
            }
        }
    }
}