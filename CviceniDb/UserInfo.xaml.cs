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
    /// Interakční logika pro UserInfo.xaml
    /// </summary>
    public partial class UserInfo : Window
    {
        static bool isTestUser = true;
        private static User U = new User(isTestUser);
        

        /*private static ListView GetSeznam()
        {
            return Seznam;
        }*/

        #region
        private static void LoadTrainings(ListView seznam, string UsersTrainingFile)
        {
            //string UsersTrainingFile = uzivatel.Name + ".xml";
            string XMLSoub = File.ReadAllText(UsersTrainingFile);

            XmlSerializer serializer2 = new XmlSerializer(typeof(List<string>), new XmlRootAttribute("ArrayOfString"));
            StringReader stringReader = new StringReader(XMLSoub);
            List<string> result = (List<string>)serializer2.Deserialize(stringReader);


            List<string> SouboryTreningu = new List<string>();
            foreach (string Sou in result)
            {
                SouboryTreningu.Add(Sou + ".xml");
                //string path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Sou + ".xml");

            }

            XmlSerializer serializer3 = new XmlSerializer(typeof(Training));
            List<Training> Treningy = new List<Training>();

            foreach (string file in SouboryTreningu)
            {
                using (StreamReader reader = new StreamReader(file))
                {
                    Training obj = (Training)serializer3.Deserialize(reader);
                    Treningy.Add(obj);
                    // Add obj to your ListView here
                }
            }

            seznam.ItemsSource = Treningy;
        }
        #endregion
        //static string UsersTrainingFile = U.Name + ".xml";


        //Konstruktor který se používá jenom při prvním načtení
        public UserInfo(User u)
        {

            InitializeComponent();
            U = u;
            //Vytvoří soubor ve kterém se ukládají jména tréningů a dohledávají se
            string UsersTrainingFile = U.Name + ".xml";
            if (!File.Exists(UsersTrainingFile))
            {
                using (FileStream fs = File.Create(UsersTrainingFile))
                {
                    byte[] content = Encoding.UTF8.GetBytes("");
                    fs.Write(content, 0, content.Length);
                }
            }

            



            string UsersTrainingFileContent = File.ReadAllText(UsersTrainingFile);
            
            //Tréningy se načítají do listview
            if(UsersTrainingFileContent != "")
            {
                string XMLSoub = File.ReadAllText(UsersTrainingFile);
                
                //Kořenový atribut je ArrayOfString. Deserializovaný obsah se ukládá do listu.
                XmlSerializer serializer2 = new XmlSerializer(typeof(List<string>), new XmlRootAttribute("ArrayOfString"));
                StringReader stringReader = new StringReader(XMLSoub);
                List<string> result = (List<string>)serializer2.Deserialize(stringReader);

                //Do listu se zadávají jména souborů
                List<string> SouboryTreningu = new List<string>();
                foreach (string Sou in result)
                {
                    SouboryTreningu.Add(Sou + ".xml");
                    //string path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Sou + ".xml");

                }
                
                
                XmlSerializer serializer3 = new XmlSerializer(typeof(Training));
                List<Training> Treningy = new List<Training>();

                foreach (string file in SouboryTreningu)
                {
                    using (StreamReader reader = new StreamReader(file))
                    {
                        //Tréningy se přečtou ze souboru a přidají do listu
                        Training obj = (Training)serializer3.Deserialize(reader);
                        Treningy.Add(obj);
                        
                    }
                }

                //Nakonec se přidají do listview
                Seznam.ItemsSource = Treningy;
            }
           


            //string CurrentUsersFile = U.Name + ".xml";

            UserNameBox.Content = "Vítáme vás "+U.Name;
        }

        //Konstruktor který se použije při otevření po přidání tréningu 
        public UserInfo(User u, string NameOfFile)
        {
            Training T = new Training();
            T.NameOfTraining = NameOfFile.Split(".")[0];
            InitializeComponent();

            U = u;
            string UsersTrainingFile = U.Name + ".xml";
            if (!File.Exists(UsersTrainingFile))
            {
                using (FileStream fs = File.Create(UsersTrainingFile))
                {
                    byte[] content = Encoding.UTF8.GetBytes("");
                    fs.Write(content, 0, content.Length);
                }
            }


            /*
            XmlSerializer serializer = new XmlSerializer(typeof(Training));


            
            using (Stream reader = new FileStream(NameOfFile, FileMode.Open))
            {
               T = (Training)serializer.Deserialize(reader);

            }

            List<Training> Trainings = new List<Training>();

            Trainings.Add(T);

            Seznam.ItemsSource = Trainings;*/

            


            


            string UsersFileContent = File.ReadAllText(UsersTrainingFile);

            if(UsersFileContent == "") 
            {
                List<string> L= new List<string>();
                L.Add(T.NameOfTraining);
                XmlSerializer UsersTrainings = new XmlSerializer(typeof(List<string>));

                using (StreamWriter writer = new StreamWriter(UsersTrainingFile))
                {
                    UsersTrainings.Serialize(writer, L);
                }
            }

            
            else
            {
                string XMLSoub = File.ReadAllText(UsersTrainingFile);

                XmlSerializer serializer2 = new XmlSerializer(typeof(List<string>), new XmlRootAttribute("ArrayOfString"));
                StringReader stringReader = new StringReader(XMLSoub);
                List<string> result = (List<string>)serializer2.Deserialize(stringReader);
                result.Add(T.NameOfTraining);

                StringWriter stringWriter = new StringWriter();
                serializer2.Serialize(stringWriter, result);
                string newXml = stringWriter.ToString();

                File.WriteAllText(UsersTrainingFile, newXml);

                
            }



            LoadTrainings(Seznam, UsersTrainingFile);

            UserNameBox.Content = "Vítáme vás " + U.Name;
        }

        private void AddTraining_Click(object sender, RoutedEventArgs e)
        {
            AddTrainingWin AT = new AddTrainingWin(U);
            AT.Show();
            this.Close();
        }

        private void CreateExcersiseButt_Click(object sender, RoutedEventArgs e)
        {
            CreateExercise CE = new CreateExercise(U);
            CE.Show();
            this.Close();
        }
    }
}
