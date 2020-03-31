using Common.DbTools;
using Common.Enums;
using Common.Model;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace UI_LokalniKontroler
{
    public partial class AddNewGeneratorWindow : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private DbGroup dbg = new DbGroup();

        protected void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private double minProduction;
        private double maxProduction;
        private double price;
        private string[] items = { "Solar", "Wind", "Micro hydro" };
        private bool[] _modeArray = new bool[] { true, false };

        public AddNewGeneratorWindow()
        {
            InitializeComponent();
            DataContext = this;
            cmbTypes.ItemsSource = items;
            cmbTypes.SelectedIndex = 0;
            cmb_box_grupa.ItemsSource = dbg.ReadAll(Data.Code);
        }

        public double MinProduction
        {
            get { return minProduction; }
            set
            {
                if (value != minProduction)
                {
                    minProduction = value;
                    OnPropertyChanged();
                }
            }
        }

        public double MaxProduction
        {
            get { return maxProduction; }
            set
            {
                if (value != maxProduction)
                {
                    maxProduction = value;
                    OnPropertyChanged();
                }
            }
        }

        public int SelectedMode()
        {
            return Array.IndexOf(ModeArray, true);
        }

        public double Price
        {
            get { return price; }
            set
            {
                if (value != price)
                {
                    price = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool[] ModeArray { get => _modeArray; set => _modeArray = value; }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DbLocalController dblc = new DbLocalController();
            DbGenerator dbg = new DbGenerator();
            DbGroup dbgroup = new DbGroup();

            Generator g = new Generator()
            {
                State = EState.OFFLINE,
                LCCode = Data.Code,
                MaxPower = MaxProduction,
                MinPower = MinProduction,
                WorkPrice = Price,
                CurrentPower = MinProduction,
                GroupName = cmb_box_grupa.Text
            };

            for(int i = 0; i < 10; i++)
            {
                g.Setpoints.Add(new Measurement(0));
            }

            if (dblc.IsOnline(Data.Code))
            {
                g.State = EState.ONLINE;
            }

            int selected = SelectedMode();

            if (selected == 0)
            {
                g.Control = EControl.LOCAL;
            }
            else
            {
                g.Control = EControl.REMOTE;
                g.CurrentPower = 0;
            }

            switch (cmbTypes.SelectedValue)
            {
                case "Solar":
                    g.Type = EType.SOLAR;
                    break;
                case "Wind":
                    g.Type = EType.WIND;
                    break;
                case "Micro hydro":
                    g.Type = EType.MICRO_HYDRO;
                    break;
                default:
                    g.Type = EType.SOLAR;
                    break;
            }

            dbg.Create(g);

            Close();
        }

        private void RadioRemoteOnCheck(object sender, RoutedEventArgs e)
        {
            if (txt_box_trenutna != null)
            {
                txt_box_trenutna.IsReadOnly = true;
                txt_box_trenutna.Text = "0";
            }
        }

        private void RadioLocalOnCheck(object sender, RoutedEventArgs e)
        {
            if (txt_box_trenutna != null)
            {
                txt_box_trenutna.IsReadOnly = false;
            }
        }

        private void AddNewGeneratorClosed(object sender, EventArgs e)
        {
            foreach(Window window in Application.Current.Windows)
            {
                if(window.GetType() == typeof(MainWindow))
                {
                    if((window as MainWindow).radio_btn_generator.IsChecked.Value)
                    {
                        (window as MainWindow).cmb_box_stat.ItemsSource = (window as MainWindow).dbg.ReadAllIds(Data.Code);
                    }
                }
            }            
        }
    }
}
