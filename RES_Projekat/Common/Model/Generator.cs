using Common.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Common.Model
{
    public class Generator : INotifyPropertyChanged
    {
        private int id;
        private string lcCode;
        private EType type;
        private EControl control;
        private EState state;
        private double currentPower;
        private double minPower;
        private double maxPower;
        private double workPrice;
        private string groupName;
        private ICollection<Measurement> measurementHistory;
        private List<Measurement> setpoints;

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(String propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public Generator()
        {
            MeasurementHistory = new List<Measurement>();
            Setpoints = new List<Measurement>();
        }

        public int Id { get => id; set => id = value; }
        public string LCCode { get => lcCode; set => lcCode = value; }
        public EType Type { get => type; set => type = value; }
        public double MinPower { get => minPower; set => minPower = value; }
        public double MaxPower { get => maxPower; set => maxPower = value; }
        public double WorkPrice { get => workPrice; set => workPrice = value; }
        public string GroupName { get => groupName; set => groupName = value; }
        public ICollection<Measurement> MeasurementHistory { get => measurementHistory; set => measurementHistory = value; }
        public List<Measurement> Setpoints { get => setpoints; set => setpoints = value; }

        public EControl Control 
        {
            get
            {
                return control;
            }
            set
            {
                if (control != value)
                {
                    control = value;
                    OnPropertyChanged("Control");
                }
            }
        }

        public double CurrentPower
        {
            get
            {
                return Math.Round(currentPower, 2);
            }
            set
            {
                if(currentPower > 0)
                    AddMeasurementToHistory();

                if (currentPower != value)
                {
                    currentPower = value;
                    OnPropertyChanged("CurrentPower");
                }
            }
        }

        public EState State 
        {
            get
            {
                return state;
            }
            set
            {
                if (state != value)
                {          
                    state = value;
                    OnPropertyChanged("State");         
                }
            }
        }

        public void AddMeasurementToHistory()
        {
            Measurement newMeasurement = new Measurement(currentPower);
            MeasurementHistory.Add(newMeasurement);
        }
    }
}
