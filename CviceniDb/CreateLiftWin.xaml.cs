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
    /// Interakční logika pro CreateLiftWin.xaml
    /// </summary>
    public partial class CreateLiftWin : Window
    {
        //Přidává cviky do drop menu
        private static string TrainingNamesFile = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Lifts.txt");
        private static string CvikyStr = File.ReadAllText(TrainingNamesFile);
        private static string[] Cviky = CvikyStr.Split("|||");
        private static string NameOfCurrentTraining = "";

        private static Training T = new Training();
        private static string LiftsFile = "";

        private static User U = new User();
        private static DateTime DT = new DateTime();
        private static bool IsEditWindow = false;

        public CreateLiftWin(string NameOfTraining, DateTime DatumTreningu,User u,bool IsOpenedFromEditT,Training t)
        {
            T = t;
            U = u;
            DT = DatumTreningu;
            NameOfCurrentTraining = NameOfTraining;
            InitializeComponent();
            Cviky = Cviky.Take(Cviky.Length - 1).ToArray(); 
            LiftsBox.ItemsSource = Cviky.Select(option => new ComboBoxItem { Content = option });
            LiftsFile = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, NameOfCurrentTraining + "Lifts" + ".xml");

            IsEditWindow = IsOpenedFromEditT;
        }

        private void PřidatButt_Click(object sender, RoutedEventArgs e)
        {
            Lift L = new Lift();
            try
            {
                string LiftsBoxContent = LiftsBox.SelectedItem.ToString();
                L.NameOfLift = LiftsBoxContent.Substring(LiftsBoxContent.IndexOf(':') + 2); 
                //L.NameOfLift = LiftsBoxContent;
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

            if(LiftsFileContent != "")
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

            if (IsEditWindow)
            {
                AddTrainingWin AT = new AddTrainingWin(T,IsEditWindow,NameOfCurrentTraining);
                AT.Show();
                this.Close();
            }
            else
            {
                AddTrainingWin AT = new AddTrainingWin(NameOfCurrentTraining,DT,U);
                AT.Show();
                this.Close();
            }


        }

        
    }
}
