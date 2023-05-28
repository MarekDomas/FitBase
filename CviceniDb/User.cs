using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CviceniDb
{
    public class User
    {
        //Atributy
        private string path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "IDs.txt");
        public string Name { get; set; }
        public string Passwd { get; set; }

        public int id { get; set; }


        //ToString se využívá při vytvoření a zapisování do souboru
        public override string ToString()
        {
            return "J:" +Name+"|"+" H: "+Passwd+"|"+ " Id: " + id + "UserEnd";
        }
        
        //Výchozí konstruktor který se využívá při prvním vytvoření objektu
        public User() 
        {
            id = File.ReadAllText(path).Length + 1;

            File.AppendAllText(path,"1");
        }


        //Jmno souboru uživatele
        public string ReturnUserFile()
        {
            string UserFileName = this.Name + ".txt";
            return UserFileName;
        }

        //Využívá se ve funkci DelaniUzivateluDoListu()
        public User(string aName, string aPasswd, int aId)
        {
            Name = aName;
            Passwd = aPasswd;
            id = aId;
        }



        //Prázdný konstruktor 
        public User(bool testUser)
        {

        }

    }
}
