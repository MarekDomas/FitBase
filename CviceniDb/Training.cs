using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CviceniDb
{
    [Serializable]
    public class Training : INotifyPropertyChanged
    {
        //Datum tréningu, používá se DateTime protože vzbýrání datumu vrací proměnouy DateTime a převádět to na DateOnly je otrvné
        public DateTime DateOfTraining { get; set; }
        public string NameOfTraining{ get; set; }

        //OwnerOfTraining se už nepoužívá protože se navzájem přepisovali majitelé
        public string OwnerOfTraining { get;set; }


        public Training(DateTime aDateOfTraining, string aNameOfTraining) 
        {
            aDateOfTraining = DateOfTraining;
            aNameOfTraining = NameOfTraining;
        }

        public Training() { }

        public event PropertyChangedEventHandler? PropertyChanged;

        public string ReturnFileOfLifts()
        {
            string FileOfLifts = NameOfTraining + ".xml";
            return FileOfLifts;
        }

        
    }
}
