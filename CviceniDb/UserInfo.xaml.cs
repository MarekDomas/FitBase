using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
//using System.Windows.Form;

namespace CviceniDb
{
    /// <summary>
    /// Interakční logika pro UserInfo.xaml
    /// </summary>
    public partial class UserInfo : Window
    {
        static bool isTestUser = true;
        private static User U = new User(isTestUser);
        private static string TempUserFileName = "TempUserFile.txt";
        private static string TempUserName = File.ReadAllText(TempUserFileName);
        private static string UsersTypesOfLiftsFile = "";
        private static string BasicLifts = File.ReadAllText("Lifts.txt");

        #region

        //Funkce se používá jen v jednom konstruktoru protože když ji použiju v tom druhém nevím proč nefunguje
        private static void LoadTrainings(ListView seznam, string UsersTrainingFile)
        {
            
            string XMLSoub = File.ReadAllText(UsersTrainingFile);

            XmlSerializer serializer2 = new XmlSerializer(typeof(List<string>), new XmlRootAttribute("ArrayOfString"));
            StringReader stringReader = new StringReader(XMLSoub);
            List<string> result = (List<string>)serializer2.Deserialize(stringReader);
            result = result.Distinct().ToList();


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
                    /*if(obj.OwnerOfTraining == U.Name)
                    {
                        Treningy.Add(obj);
                    }*/
                    Treningy.Add(obj);
                    
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
            UsersTypesOfLiftsFile = U.Name + "Excercises.txt";
            UserNameBox.Content = "Vítáme vás " + U.Name;
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

            if (!File.Exists(UsersTypesOfLiftsFile))
            {
                using (FileStream fs = File.Create(UsersTypesOfLiftsFile))
                {
                    byte[] content = Encoding.UTF8.GetBytes("");
                    fs.Write(content, 0, content.Length);
                }
                File.WriteAllText(UsersTypesOfLiftsFile, BasicLifts);
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
                result = result.Distinct().ToList();

                //Do listu se zadávají jména souborů
                List<string> SouboryTreningu = new List<string>();
                foreach (string Sou in result)
                {
                    SouboryTreningu.Add(Sou + ".xml");

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

            
            //Při dvojkliku se otevře okno na editování
            Seznam.MouseDoubleClick += (s, e) =>
            {
                //Kontroluje se jestli vybraný item není null protože při missclicku by to spadlo
                if(Seznam.SelectedItem != null)
                {
                    bool IsEdit = true;
                    Training SelectedT = Seznam.SelectedItem as Training;

                    AddTrainingWin AT2 = new AddTrainingWin(SelectedT,IsEdit,SelectedT.NameOfTraining);
                    AT2.Show();
                    this.Close();
                }
            };


            

            
        }

        //Konstruktor který se použije při otevření po přidání tréningu 
        public UserInfo(User u, string NameOfFile)
        {
            Training T = new Training();
            T.NameOfTraining = NameOfFile.Split(".")[0];
            InitializeComponent();

            U = u;
            U.Name = File.ReadAllText(TempUserFileName);
            string UsersTrainingFile = File.ReadAllText(TempUserFileName) + ".xml";
            
       
            string UsersFileContent = File.ReadAllText(UsersTrainingFile);

            //Přidá jméno prvního vytvořeného tréningu pokud 
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
            //Přidává tréning pokud tam už jsou jiné tréningy
            else
            {
                string XMLSoub = File.ReadAllText(UsersTrainingFile);

                XmlSerializer serializer2 = new XmlSerializer(typeof(List<string>), new XmlRootAttribute("ArrayOfString"));
                StringReader stringReader = new StringReader(XMLSoub);
                List<string> result = (List<string>)serializer2.Deserialize(stringReader);
                result = result.Distinct().ToList();
                result.Add(T.NameOfTraining);

                StringWriter stringWriter = new StringWriter();
                serializer2.Serialize(stringWriter, result);
                string newXml = stringWriter.ToString();

                File.WriteAllText(UsersTrainingFile, newXml);
                                
            }


            //Načte tréningy do ListView
            LoadTrainings(Seznam, UsersTrainingFile);

            //Dvojklik na editaci tréningů
            Seznam.MouseDoubleClick += (s,e) => 
            {
                if(Seznam.SelectedItem  != null)
                {
                    bool IsEdit = true;
                    Training SelectedT = Seznam.SelectedItem as Training;
                    AddTrainingWin AT2 = new AddTrainingWin(SelectedT,IsEdit,SelectedT.NameOfTraining);
                    AT2.Show();
                    this.Close();
                }
            };

            UserNameBox.Content = "Vítáme vás " + U.Name;
        }

        
        //Otevře okno na přidání tréningu
        private void AddTraining_Click(object sender, RoutedEventArgs e)
        {
            AddTrainingWin AT = new AddTrainingWin(U);
            AT.Show();
            this.Close();
        }

        //Otevře okno na vytvoření neexistujícího cviku
        private void CreateExcersiseButt_Click(object sender, RoutedEventArgs e)
        {
            CreateExercise CE = new CreateExercise(U);
            CE.Show();
            this.Close();
        }

        //Tlačítko na odhlášení
        private void LogOutButt_Click(object sender, RoutedEventArgs e)
        {
            MainWindow MW = new MainWindow();
            MW.Show();
            this.Close();
        }

        //Maže tréningy
        private void DeleteTrainingButt_Click(object sender, RoutedEventArgs e)
        {
            if(Seznam.SelectedItem != null) 
            {
                //Vytvoří vybraný objekt a vymaže soubor tréningu, vymaže soubor se cviky tréningu a vymaže odkaz na tréning a vymaže ho z ListView
                Training DelT = Seznam.SelectedItem as Training;
                string FilePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, DelT.NameOfTraining+ ".xml");
                File.Delete(FilePath);

                string UsersTrainings = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, U.Name + ".xml");

                string XMLSoub = File.ReadAllText(UsersTrainings);
                XmlSerializer serializer2 = new XmlSerializer(typeof(List<string>), new XmlRootAttribute("ArrayOfString"));
                StringReader stringReader = new StringReader(XMLSoub);
                List<string> result = (List<string>)serializer2.Deserialize(stringReader);
                result = result.Distinct().ToList();
                result.Remove(DelT.NameOfTraining);


                StringWriter stringWriter = new StringWriter();
                serializer2.Serialize(stringWriter, result);
                string newXml = stringWriter.ToString();

                File.WriteAllText(UsersTrainings, newXml);


                string LiftsFile = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, DelT.NameOfTraining+ "Lifts" + ".xml");
                File.Delete(LiftsFile);

                //Seznam.Items.Remove(Seznam.SelectedItem);

                IEditableCollectionView items = Seznam.Items;

                items.Remove(Seznam.SelectedItem);

            }

        }

        //Otevře okno pro odstranění typu cviku
        private void DeleteExcerciseButt_Click(object sender, RoutedEventArgs e)
        {
            DeleteExcercise DE = new DeleteExcercise(U);
            DE.Show();
            this.Close();
        }
    }
}
