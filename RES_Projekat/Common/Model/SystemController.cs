using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Model
{
    public class SystemController : INotifyPropertyChanged
    {
        private int id;
        private double requiredPower;
        private double totalPower;
        private List<LocalController> localControllers;

        public int Id { get => id; set => id = value; }
        public List<LocalController> LocalControllers { get => localControllers; set => localControllers = value; }

        public double RequiredPower
        {
            get => requiredPower;
            set
            {
                if(value != requiredPower)
                {
                    requiredPower = value;
                    OnPropertyChanged("RequiredPower");
                }
            }
        }

        public double TotalPower
        {
            get => totalPower;
            set
            {
                if(value != totalPower)
                {
                    totalPower = value;
                    OnPropertyChanged("TotalPower");
                }
            }
        }

        public SystemController()
        {
            LocalControllers = new List<LocalController>();
            RequiredPower = 0;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(String propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
