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
    /// Interakční logika pro UserInfo.xaml
    /// </summary>
    public partial class UserInfo : Window
    {
        static bool isTestUser = true;
        private static User U = new User(isTestUser);

        //static string UsersTrainingFile = U.Name + ".xml";


        public UserInfo(User u)
        {

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

            UserNameBox.Content = "Vítáme vás "+U.Name;
        }

        public UserInfo()
        {
            InitializeComponent();
        }

        private void AddTraining_Click(object sender, RoutedEventArgs e)
        {
            AddTrainingWin AT = new AddTrainingWin(U);
            AT.Show();
            this.Close();
        }
    }
}
