using System;
using System.Collections.Generic;
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
        public static User U = new User(isTestUser);
        public UserInfo(User u)
        {
            Lift Cvik = new Lift();
            Cvik.Weight = 100;
            Cvik.NameOfLift = "BENCH";
            Cvik.Reps = 10;
            Cvik.Sets = 3;

            InitializeComponent();
            U = u;
            UserNameBox.Content = "Vítáme vás "+U.Name;
        }
    }
}
