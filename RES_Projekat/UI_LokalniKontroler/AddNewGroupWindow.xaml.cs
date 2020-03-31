using Common.DbTools;
using Common.Model;
using System.Linq;
using System.Windows;

namespace UI_LokalniKontroler
{
    /// <summary>
    /// Interaction logic for AddNewGroupWindow.xaml
    /// </summary>
    public partial class AddNewGroupWindow : Window
    {
        public AddNewGroupWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DbGroup dbGroup = new DbGroup();
            DbLocalController dblc = new DbLocalController();

            Group group = new Group();

            group.LCCode = Data.Code;
            group.Name = txt_box_kod.Text;
            group.MaxProduction = 0;
            group.NumOfUnits = 0;
            group.CurrentProduction = 0;
            
            dbGroup.Create(group);

            Close();
        }

        private void AddNewGroupClosed(object sender, System.EventArgs e)
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(MainWindow))
                {
                    if (!(window as MainWindow).radio_btn_generator.IsChecked.Value)
                    {
                        (window as MainWindow).cmb_box_stat.ItemsSource = (window as MainWindow).dbgroup.ReadAll(Data.Code);
                    }
                }
            }
        }
    }
}
