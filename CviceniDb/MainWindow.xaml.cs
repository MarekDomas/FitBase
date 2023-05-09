using System;
using System.Collections.Generic;
using System.Configuration;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using EncryptionDecryptionUsingSymmetricKey;
using Microsoft.EntityFrameworkCore;

namespace CviceniDb
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window 
    {
        static string key = "b14ca5898a4e4133bbce2ea2315a1916";
        //Už nevím co to dělá NEŠAHAT!!!!!
        static string path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DB.txt");
        static string dbContentUnHashed = File.ReadAllText(path);

        //static byte[] bytesToDecode = Convert.FromBase64String(dbContentUnHashed);
        //static string decodedString = Encoding.UTF8.GetString(bytesToDecode);
        //static string decodedString2 = Convert.ToString(bytesToDecode);

        //static string[] UserData = decodedString.Split("Konecradku");

        //static string userDataStr = String.Join("",UserData);
        //Users = Users.Take(Users.Length - 1).ToArray();
        static string ObsahDb = AesOperation.DecryptString(key, dbContentUnHashed);
        static string[] Users = ObsahDb.Split("UserEnd");
        //Vytvoření listu pro uživatele které to vzalo ze souboru
        static List<User> Uzivatele = new List<User>();

        //Rozdělení ze základního tvaru do objektu a následovné uložení do listu
        private static void DelaniUzivateluDoListu()
        {
            Users = Users.Take(Users.Length - 1).ToArray();
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
        }

        


        //Array.Resize(ref Users, Users.Length - 1);
        public MainWindow()
        {
            InitializeComponent();
            DelaniUzivateluDoListu();
            foreach (User u in Uzivatele)
            {
                DebugLabel.Content += u.ToString() + "\n"; 
            }
        }
        
        private void SignIn_Click(object sender, RoutedEventArgs e)
        {
            foreach (User user in Uzivatele)
            {
                if(NameBox.Text == user.Name && PasswdBox.Text == user.Passwd)
                {
                    //MessageBox.Show("TO SES POSRAL");
                    UserInfo userInfo = new UserInfo(user);
                    userInfo.Show();
                    //TestWindow TW= new TestWindow();
                    //TW.Show();
                    this.Close();
                    break;
                    
                }
                else
                {
                    //continue;
                    //MessageBox.Show("smůla");
                    //break;
                    
                    
                }
            }
            DebugLabel.Content = "Nelze se přihlásit";
            //MessageBox.Show("smůla");
        }

        private void SignUp_Click(object sender, RoutedEventArgs e)
        {
            SignUp SU = new SignUp();
            SU.Show();
        }
    }
}
