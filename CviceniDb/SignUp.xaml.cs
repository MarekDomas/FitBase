using EncryptionDecryptionUsingSymmetricKey;
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

namespace CviceniDb
{

    
    /// <summary>
    /// Interakční logika pro SignUp.xaml
    /// </summary>
    public partial class SignUp : Window
    {
        //private string currentDir = Directory.GetCurrentDirectory();
        // private string path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DB.txt");

        // Název souboru. Je vytvořený ve složce bin
        private string path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DB.txt"); //verze od Martina

        //Stack overflow verze
        //private string path = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "DB.txt");
        //string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
        

        public SignUp()
        {
            InitializeComponent();
        }

        
        //Vytváří se nový uživatel a zapisuje se do souboru
        private async void FirstSignUp_Click(object sender, RoutedEventArgs e)
        {
            if(NewPasswdBox.Text == CheckPasswd.Text)
            {
                var key = "b14ca5898a4e4133bbce2ea2315a1916";

                //Vytváření nové instance
                User U = new User();
                U.Name = NewNameBox.Text;
                U.Passwd = NewPasswdBox.Text;

                //Převádění do Base64
                string userData = U.ToString(); /*"Konecradku"*/
                /*byte[] userDatabyty = Encoding.UTF8.GetBytes(userData) ;

                userData = Environment.NewLine + Convert.ToBase64String(userDatabyty);*/
                var encryptedString = AesOperation.EncryptString(key, userData) + Environment.NewLine;
                File.AppendAllText(path, encryptedString);

                //File.AppendAllText(projectDirectory, "KOZY");
                this.Close();
            }
            else
            {
                MessageBox.Show("Hesla se neshodují!");
            }
        }
    }
}