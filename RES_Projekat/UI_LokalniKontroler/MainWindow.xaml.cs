using Common.DbTools;
using Common.Model;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;
using System.Windows.Input;

namespace UI_LokalniKontroler
{
    public partial class MainWindow : Window
    {      
        public DbGenerator dbg = new DbGenerator();
        public DbGroup dbgroup = new DbGroup();
        private BindingList<Generator> generators = new BindingList<Generator>();


        public MainWindow(string code)
        {
            DataContext = this;
            InitializeComponent();
            Data.Code = code;
            textBlockCode.Text = code;
            radio_btn_generator.IsChecked = true;
            generatorsDataGrid.ItemsSource = generators;

            Thread refresher = new Thread(Refresh);
            refresher.Start();
        }

        private void ButtonNewGenerator(object sender, RoutedEventArgs e)
        {
            DbGroup dbgroup = new DbGroup();
            var groups = dbgroup.ReadAll(Data.Code);

            if (groups.Count == 0)
            {
                MessageBox.Show("Please add one group first!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                AddNewGeneratorWindow add = new AddNewGeneratorWindow();
                add.Show();
            }
        }

        private void ButtonNewGroup(object sender, RoutedEventArgs e)
        {
            AddNewGroupWindow add = new AddNewGroupWindow();
            add.Show();
        }

        private void DataGridRow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Window promenaTipa = new ChangeControlTypeAndValueWindow((Generator)generatorsDataGrid.SelectedItem);
            promenaTipa.Show();
        }

        private void Refresh()
        {         
            while (true)
            {
                var gs = dbg.ReadAllForLC(Data.Code);

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
                            generators.FirstOrDefault(g => g.Id == id).CurrentPower = gen.CurrentPower;
                            generators.FirstOrDefault(g => g.Id == id).Control = gen.Control;
                        });
                    }
                }              

                Thread.Sleep(500);
            }
        }

        private void RadioButton_Groups(object sender, RoutedEventArgs e)
        {
            //popuniti combo box sa grupama
            cmb_box_stat.ItemsSource = dbgroup.ReadAll(Data.Code);
            cmb_box_stat.SelectedIndex = 0;
        }

        private void RadioButton_Generators(object sender, RoutedEventArgs e)
        {
            //popuniti combo box sa generatorima
            cmb_box_stat.ItemsSource = dbg.ReadAllIds(Data.Code);
            cmb_box_stat.SelectedIndex = 0;
        }

        private void Btn_pocetni_datum_Click(object sender, RoutedEventArgs e)
        {
            if (calendar.SelectedDate != null)
                txt_box_poc_datum.Text = calendar.SelectedDate.Value.ToString(Data.DateFormat);
        }

        private void Btn_krajnji_datum_Click(object sender, RoutedEventArgs e)
        {
            if (calendar.SelectedDate != null)
                txt_box_kraj_datum.Text = calendar.SelectedDate.Value.ToString(Data.DateFormat);
        }

        private void Btb_Show_Stats(object sender, RoutedEventArgs e)
        {
            try
            {
                Calculations calculations = new Calculations();
                
                DateTime dateFrom = DateTime.ParseExact(txt_box_poc_datum.Text, Data.DateFormat, null);
                DateTime dateTo = DateTime.ParseExact(txt_box_kraj_datum.Text, Data.DateFormat, null);

                int id = 0;
                string tmp = string.Empty;

                if(radio_btn_generator.IsChecked == true)
                {
                    if(!Int32.TryParse(cmb_box_stat.SelectedValue.ToString(), out id))
                    {
                        throw new Exception("Selected generator ID is invalid.");
                    }
                }
                else
                {
                    tmp = cmb_box_stat.SelectedValue.ToString();
                }


                if (radio_btn_generator.IsChecked == true)
                {
                    txt_box_min.Text = calculations.MinGeneratorPower(id, dateFrom, dateTo).ToString("0.##") + " kW";
                    txt_box_srednja.Text = calculations.MeanGeneratorPower(id, dateFrom, dateTo).ToString("0.##") + " kW";
                    txt_box_max.Text = calculations.MaxGeneratorPower(id, dateFrom, dateTo).ToString("0.##") + " kW";
                }
                else
                {
                    txt_box_min.Text = calculations.MinGroupPower(tmp, dateFrom, dateTo).ToString("0.##") + " kW";
                    txt_box_srednja.Text = calculations.MeanGroupPower(tmp, dateFrom, dateTo).ToString("0.##") + " kW";
                    txt_box_max.Text = calculations.MaxGroupPower(tmp, dateFrom, dateTo).ToString("0.##") + " kW";
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show("Error while parsing data. Check all fields and try again. " + exc.Message, "Parse Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
   
        }
    }
}

