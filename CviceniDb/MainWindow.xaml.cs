using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using EncryptionDecryptionUsingSymmetricKey;


namespace CviceniDb
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window 
    {

        //Klíč použitý na rozšifrování dat z uživatelského souboru
        static string key = "b14ca5898a4e4133bbce2ea2315a1916";

        //Soubory které se vytvoří
        private static string ExistingTrainings = "ExistingTrainings.txt";
        private static string UserNamesFile = "Users.txt";
        private static string ListOfTrainingsFile = "Trainings.xml";
        private static string TrainingNamesFile = "Trainings.txt";
        private static string TypesOfLiftsFile = "Lifts.txt";
        private static string IdsFile = "IDs.txt";
        private static string TempUserFile = "TempUserFile.txt";
        string[] cviky = {
                        "Dřep s činkou",
                        "Mrtvý tah",
                        "Benčpress",
                        "Ramenní tlak",
                        "Veslování",
                        "Log lift",
                        "Curl s činkami",
                        "Tricepsová extenze",
                        "Hammer curl",
                        "Kabelový fly",
                        "Leg press",
                        "Stahování na lýtkové svaly",
                        "Hip thrust",
                        "Glute bridge",
                        "Výpady",
                        "Step-up",
                        "Rumunský mrtvý tah",
                        "Good morning",
                        "Cable pull-through",
                        "Rows v sedě",
                        "Pulldown",
                        "Přístroj na tlak hrudníku",
                        "Přístroj na ramenní tlak",
                        "Předkopy",
                        "Přístroj na hamstringy",
                        "Přístroj na svaly břicha",
                        "Russian twist",
                        "Medicinbalový slam",
                        "Bitevní provaz",
                        "Švihadlo",
                        "Kettlebell swing",
                        "Farmářská chůze",
                        "Tahání/tlačení saní",
                        "Převrácení pneumatiky"
                    };


        //Funkce na vytvoření souborů
        private static void CreateFile(string filename)
        {
            if (!File.Exists(filename))
            {
                using (FileStream fs = File.Create(filename))
                {
                    byte[] content = Encoding.UTF8.GetBytes("");
                    fs.Write(content, 0, content.Length);
                    
                }
            }
        }



        public MainWindow()
        {
            //Vytváření souborů
            CreateFile(UserNamesFile);
            CreateFile(ExistingTrainings);
            CreateFile(IdsFile);
            CreateFile(ListOfTrainingsFile);
            CreateFile(TrainingNamesFile);
            CreateFile(TypesOfLiftsFile);
            
            
            
            if(!File.Exists(TempUserFile))
            {
                using (FileStream fs = File.Create(TempUserFile))
                {
                    byte[] content = Encoding.UTF8.GetBytes("");
                    fs.Write(content, 0, content.Length);
                }
            }
            else
            {
                File.WriteAllText(TempUserFile, "");
            }


            //Do souboru s cviky se nahrají pokud tam nejsou
            if (File.ReadAllText(TypesOfLiftsFile) == "")
            {
                foreach (string cvik in cviky)
                {
                    File.AppendAllText(TypesOfLiftsFile, cvik + "|||");
                }
            }

            

            InitializeComponent();
            
        }
        
        private void SignIn_Click(object sender, RoutedEventArgs e)
        {
            
            string DBcontentHashed = "";

            try
            {
                //Najde soubor uživatele, je ve chráněném bloku protože když napíšete jméno neexistujícího uživatele program spadne
                string path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, NameBox.Text + ".txt");
                DBcontentHashed = File.ReadAllText(path);
            }
            catch 
            {
                DebugLabel.Content = "Nelze se přihlásit";
            }
            
            //Rozšifruje data ze souboru
            string DBcontentUnhashed = AesOperation.DecryptString(key, DBcontentHashed);
            string[] Users = DBcontentUnhashed.Split("UserEnd");
            
            List<User> Uzivatele = new List<User>();

            Users = Users.Take(Users.Length - 1).ToArray();
            
            //Rozdělí data ze souboru dá je do instance a uloží do listu
            foreach (string cast in Users)
            {
                string[] keyValuePairs = cast.Split('|');
                string name = keyValuePairs[0].Split(':')[1].Trim();
                string password = keyValuePairs[1].Split(':')[1].Trim();
                password = password.TrimStart();
                int id = int.Parse(keyValuePairs[2].Split(':')[1].Trim());

                User user = new User(name, password, id);
                Uzivatele.Add(user);
            }
            
            
            foreach (User user in Uzivatele)
            {
                //Úspěšné přihlášení uživatele
                if (NameBox.Text == user.Name && PasswdBox.Text == user.Passwd)
                {

                    UserInfo userInfo = new UserInfo(user);
                    userInfo.Show();
                    File.WriteAllText(TempUserFile, user.Name);

                    this.Close();
                    break;

                }
            }
            DebugLabel.Content = "Nelze se přihlásit";
            
        }


        //Otevře okno pro vytvoření uživatele
        private void SignUp_Click(object sender, RoutedEventArgs e)
        {
            SignUp SU = new SignUp();
            SU.Show();
            this.Close ();
        }

      
    }
}
