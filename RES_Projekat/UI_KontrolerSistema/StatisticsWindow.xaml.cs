using System;
using System.Windows;
using System.Windows.Media;

namespace UI_KontrolerSistema
{
    /// <summary>
    /// Interaction logic for ForecastWindow.xaml
    /// </summary>
    public partial class StatisticsWindow : Window
    {
        public DateTime chosenDate { get; set; }
        public StatisticsWindow(DateTime date)
        {
            InitializeComponent();
            fordate.Text = date.ToString("dd/MM/yyyy");
            chosenDate = date;

            DoStatistics();
        }

        public void DoStatistics()
        {
            Statistics statistics = new Statistics();

            try
            {
                var minimumLoad = statistics.MinimumLoad(chosenDate);
                minimumLoadtxt.Text = Math.Round(minimumLoad, 2).ToString() + " kW/h";
            }
            catch { }
            try
            {
                var maximumLoad = statistics.MaximumLoad(chosenDate);
                maximumLoadtxt.Text = Math.Round(maximumLoad, 2).ToString() + " kW/h";
            }
            catch { }
            try
            {
                var averageLoad = statistics.AverageLoad(chosenDate);
                averageLoadtxt.Text = Math.Round(averageLoad, 2).ToString() + " kW/h";
            }
            catch { }
            try
            {
                var totalGensActive = statistics.TotalGeneratorsActive(chosenDate);
                totalActivetxt.Text = totalGensActive.ToString();
            }
            catch { }
            try
            {
                var totalCost = statistics.GetTotalCost(chosenDate);
                totalCosttxt.Text = Math.Round(totalCost, 2).ToString() + " $";
            }
            catch { }

            if (minimumLoadtxt.Text == "")
            {
                minimumLoadtxt.Text = "No data for chosen date";
                minimumLoadtxt.Foreground = Brushes.Red;
            }

            if (maximumLoadtxt.Text == "")
            {
                maximumLoadtxt.Text = "No data for chosen date";
                maximumLoadtxt.Foreground = Brushes.Red;
            }

            if (averageLoadtxt.Text == "")
            {
                averageLoadtxt.Text = "No data for chosen date";
                averageLoadtxt.Foreground = Brushes.Red;
            }
            if (totalActivetxt.Text == "" || Double.Parse(totalActivetxt.Text) == 0)
            {
                totalActivetxt.Text = "No data for chosen date";
                totalActivetxt.Foreground = Brushes.Red;
            }
            if (totalCosttxt.Text == "" || Double.Parse(totalActivetxt.Text) == 0)
            {
                totalCosttxt.Text = "No data for chosen date";
                totalCosttxt.Foreground = Brushes.Red;
            }

        }
    }
}
