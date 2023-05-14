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
        public DateOnly DateOfTraining { get; set; }
        public string NameOfTraining{ get; set; }

        public string OwnerOfTraining { get;set; }


        public Training(DateOnly aDateOfTraining, string aNameOfTraining) 
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

        /*public override string ToString()
        {
            return Da
        }*/
    }
}
