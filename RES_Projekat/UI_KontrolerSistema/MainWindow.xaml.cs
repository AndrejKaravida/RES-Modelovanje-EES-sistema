using Common.DbTools;
using Common.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Media;

namespace UI_KontrolerSistema
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ObservableCollection<Generator> generators = new ObservableCollection<Generator>();
        private SystemController systemController = new SystemController();
        private Thread refresher;
        private DbGenerator dbg = new DbGenerator();

        public MainWindow()
        {
            if ((systemController = Data.Dbsc.GetSystemController()) == null)
            {
                MessageBox.Show("You need to start the main system controller application first!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
            }
            DataContext = this;
            InitializeComponent();
            dataGrid.ItemsSource = generators;

            refresher = new Thread(Refresh);
            refresher.Start();
        }

        private void Refresh()
        {
            while (Data.RunningFlag)
            {
      
                App.Current.Dispatcher.Invoke((Action)delegate
                {
                    textBlockGenerated.Text = Data.Dbsc.GetTotalPower().ToString();
                    textBlockPower.Text = Data.Dbsc.GetRequiredPower().ToString();
                });

                var gs = Data.dbgen.ReadAll();

                foreach (var gen in gs)
                {
                    int id = gen.Id;

                    if (generators.FirstOrDefault(g => g.Id == gen.Id) == null)
                    {
                        //Zato sto se ObservableCollection ne moze menjati u drugom threadu
                        //
                        App.Current.Dispatcher.Invoke((Action)delegate
                        {
                            generators.Add(gen);
                        });
                    }
                    else
                    {
                        App.Current.Dispatcher.Invoke((Action)delegate
                        {
                            generators.FirstOrDefault(g => g.Id == id).State = gen.State;
                            generators.FirstOrDefault(g => g.Id == id).CurrentPower = gen.CurrentPower;
                            generators.FirstOrDefault(g => g.Id == id).Control = gen.Control;
                        });
                    }
                  
                }

                Thread.Sleep(500);
            }
        }

        private void btnSubmitValue(object sender, RoutedEventArgs e)
        {
            double requiredPower = 0;

            try
            {
                requiredPower = Double.Parse(textBoxPower.Text);
                textBlockPower.Text = "";
            }
            catch
            {
                MessageBox.Show("Invalid number. Please enter valid number.", "Error", MessageBoxButton.OK);
            }

            var remoteGeneratorsList = dbg.ReadAllRemote();

            double maxPower = 0;
            remoteGeneratorsList.ForEach(x => maxPower += x.MaxPower);

            if (requiredPower > maxPower)
            {
                MessageBox.Show($"Required power[{requiredPower}] must be lower than maximum remote capacity power[{maxPower}].");
            }
            else if(requiredPower < 0)
            {
                MessageBox.Show($"Required power[{requiredPower}] must be greater than 0.");
            }
            else
            {
                Data.Dbsc.SetRequiredPower(requiredPower);
            }
        }

        private void OnClose(object sender, EventArgs e)
        {
            Data.RunningFlag = false;
            refresher.Join();
        }

        private void btnShowStatistics_Click(object sender, RoutedEventArgs e)
        {
            var date = calendar.SelectedDate;
            if(date == null)
            {
                MessageBox.Show("You need to choose date!");
            }
            else
            {
                StatisticsWindow statisticsWindow = new StatisticsWindow(date.Value);
                statisticsWindow.ShowDialog();
            }
        }

        private void btnSetDate_Click(object sender, RoutedEventArgs e)
        {           
            if (calendar.SelectedDate != null)
                chosenDate.Text = calendar.SelectedDate.Value.ToString(Data.DateFormat);
        }
    }
}
