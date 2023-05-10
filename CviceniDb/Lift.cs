using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CviceniDb
{
    public class Lift : INotifyPropertyChanged
    {
        public string NameOfLift { get; set; }
        public int Reps { get; set; }
        public int Sets { get; set; }
        public float Weight { get; set; }

        public DateOnly Date { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
        //public TimeSpan Rest { get; set; }

        //Nula hodin jedna minuta, tricet sekund
        //TimeSpan test = new TimeSpan(0, 1, 30);
    }
}
