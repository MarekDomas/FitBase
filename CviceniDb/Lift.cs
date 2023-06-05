using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CviceniDb
{
    [Serializable]
    public class Lift : INotifyPropertyChanged
    {

        /// <summary>
        /// Název cviku
        /// </summary>
        public string NameOfLift { get; set; }
        /// <summary>
        /// Počet opakování cviku
        /// </summary>
        public int Reps { get; set; }
        /// <summary>
        /// Počet sérií
        /// </summary>
        public int Sets { get; set; }
        /// <summary>
        /// Váha udávaná v kg
        /// </summary>
        public float Weight { get; set; }

        

        public event PropertyChangedEventHandler? PropertyChanged;
        //public TimeSpan Rest { get; set; }

        //Nula hodin jedna minuta, tricet sekund
        //TimeSpan test = new TimeSpan(0, 1, 30);
    }
}
