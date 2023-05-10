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


        static string UserNamesFile = "Users.txt";

      
        public MainWindow()
        {
            if(!File.Exists(UserNamesFile)) 
            {
                using (FileStream fs = File.Create(UserNamesFile)) // Create the file if it doesn't exist
                {
                    // Optionally, you can write some content to the file here
                    byte[] content = Encoding.UTF8.GetBytes("");
                    fs.Write(content, 0, content.Length);
                }
            }    
            InitializeComponent();
            
        }
        
        private void SignIn_Click(object sender, RoutedEventArgs e)
        {
            
            string DBcontentHashed = "";

            try
            {

                string path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, NameBox.Text + ".txt");
                DBcontentHashed = File.ReadAllText(path);
            }
            catch 
            {
                DebugLabel.Content = "Nelze se přihlásit";
            }
            string DBcontentUnhashed = AesOperation.DecryptString(key, DBcontentHashed);
            string[] Users = DBcontentUnhashed.Split("UserEnd");

            List<User> Uzivatele = new List<User>();

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
            

            foreach (User user in Uzivatele)
            {
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

        private void SignUp_Click(object sender, RoutedEventArgs e)
        {
            SignUp SU = new SignUp();
            SU.Show();
            this.Close ();
        }
    }
}
