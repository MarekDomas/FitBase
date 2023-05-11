﻿using EncryptionDecryptionUsingSymmetricKey;
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

        
       
        public SignUp()
        {
            InitializeComponent();
        }
        
        
        
        //static string UserNamesFile = "Users.txt";
        
        //Soubor kde jsou uložená jména uživatelů, používá se ke kontrole jestli uživatel už existuje
        static string UserNamesFile = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Users.txt");

        //Vytváří se nový uživatel a zapisuje se do souboru
        private async void FirstSignUp_Click(object sender, RoutedEventArgs e)
        {
            
            string ContentOfUserNamesFile = File.ReadAllText(UserNamesFile);
            string[] UserNames = ContentOfUserNamesFile.Split(" ");
            
            // Kontrola jestli uživatel už existuje
            if (UserNames.Contains(NewNameBox.Text))
            {
                MessageBox.Show("Tento uživatel už existuje");
            }
            //Heslo nemůže obsahovat : nebo | protože by to dělalo bordel při rozdělování dat
            else if(NewPasswdBox.Text.Contains("|") || NewPasswdBox.Text.Contains(":"))
            {
                NewPasswdBox.Text = "";
                CheckPasswd.Text = "";
                MessageBox.Show("Heslo nemůže obsahovat : nebo |");
            }
            
            //Vytváření uživatele a zapisování do vlastního souboru
            else if (NewPasswdBox.Text == CheckPasswd.Text)
            {


                var key = "b14ca5898a4e4133bbce2ea2315a1916";

                //Vytváření nové instance
                User U = new User();
                U.Name = NewNameBox.Text;
                U.Passwd = NewPasswdBox.Text;
                File.AppendAllText(UserNamesFile, U.Name+ " ");


                using (FileStream fs = File.Create(U.ReturnUserFile())) // Create the file if it doesn't exist
                {
                    // Optionally, you can write some content to the file here
                    byte[] content = Encoding.UTF8.GetBytes("");
                    fs.Write(content, 0, content.Length);
                }
                string path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, U.ReturnUserFile());
                //Převádění do Base64
                string userData = U.ToString(); /*"Konecradku"*/
                
                var encryptedString = AesOperation.EncryptString(key, userData) + Environment.NewLine;
                File.AppendAllText(path, encryptedString);

                MainWindow MW = new MainWindow();
                MW.Show();
        
                this.Close();
            }
            else
            {
                MessageBox.Show("Hesla se neshodují!");
            }
        }
    }
}
