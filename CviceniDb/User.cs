using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CviceniDb
{
    internal class User
    {
        public string Name { get; set; }
        public string Passwd { get; set; }



        public override string ToString()
        {
            return Name+" "+" "+Passwd;
        }
    }
}
