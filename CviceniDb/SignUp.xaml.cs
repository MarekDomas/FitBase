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
        private string path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DB.txt");
        public SignUp()
        {
            InitializeComponent();
        }

        private void SignUp_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void FirstSignUp_Click(object sender, RoutedEventArgs e)
        {
            User U = new User();
            U.Name = NewNameBox.Text;
            U.Passwd = NewPasswdBox.Text;
            string userData = U.Name + U.Passwd + Environment.NewLine;
            File.WriteAllText(path, userData);
            this.Close();
        }
    }
}
