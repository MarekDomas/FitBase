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
        static string key = "b14ca5898a4e4133bbce2ea2315a1916";

        private static string ExistingTrainings = "ExistingTrainings.txt";
        private static string UserNamesFile = "Users.txt";
        private static string ListOfTrainingsFile = "Trainings.xml";
        private static string TrainingNamesFile = "Trainings.txt";
        private static string TypesOfLiftsFile = "Lifts.txt";
        private static string IdsFile = "IDs.txt";
        string[] cviky = {
                        "Dřep s činkou",
                        "Mrtvý tah",
                        "Benčpress",
                        "Ramenní tlak",
                        "Nakloněný veslo",
                        "Kláda nahoru",
                        "Kláda dolů",
                        "Curl s činkami",
                        "Tricepsová extenze",
                        "Vedlejší zdvih",
                        "Přední zdvih",
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
                        "Sedící veslo",
                        "Pulldown",
                        "Přístroj na tlak hrudníku",
                        "Přístroj na ramenní tlak",
                        "Přístroj na extension stehen",
                        "Přístroj na curl stehen",
                        "Přístroj na svaly břicha",
                        "Plank",
                        "Russian twist",
                        "Zvedání nohou visem",
                        "Medicinbalový slam",
                        "Bitevní provaz",
                        "Oskakování švihadlem",
                        "Box jump",
                        "Kettlebell swing",
                        "Farmářský chůze",
                        "Tahání/tlačení saní",
                        "Převrácení pneumatiky"
                    };




        public MainWindow()
        {
        
            //Vytvoření souboru se jmény uživatelů pokud není
            if(!File.Exists(UserNamesFile)) 
            {
                using (FileStream fs = File.Create(UserNamesFile)) 
                {
                    byte[] content = Encoding.UTF8.GetBytes("");
                    fs.Write(content, 0, content.Length);
                }
            }
            if (!File.Exists(ExistingTrainings))
            {
                using (FileStream fs = File.Create(ExistingTrainings))
                {
                    byte[] content = Encoding.UTF8.GetBytes("");
                    fs.Write(content, 0, content.Length);
                }
            }

            if (!File.Exists(IdsFile))
            {
                using (FileStream fs = File.Create(IdsFile))
                {
                    byte[] content = Encoding.UTF8.GetBytes("");
                    fs.Write(content, 0, content.Length);
                }
            }

            if (!File.Exists(ListOfTrainingsFile))
            {
                using (FileStream fs = File.Create(ListOfTrainingsFile))
                {
                    byte[] content = Encoding.UTF8.GetBytes("");
                    fs.Write(content, 0, content.Length);
                }
            }

            if (!File.Exists(TrainingNamesFile))
            {
                using (FileStream fs = File.Create(TrainingNamesFile))
                {
                    byte[] content = Encoding.UTF8.GetBytes("");
                    fs.Write(content, 0, content.Length);
                }
            }

            if (!File.Exists(TypesOfLiftsFile))
            {
                using(FileStream fs = File.Create(TypesOfLiftsFile))
                {
                    byte[] content = Encoding.UTF8.GetBytes("");
                    fs.Write(content, 0, content.Length);
                }
            }

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
