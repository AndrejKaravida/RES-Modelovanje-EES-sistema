using Common.DbTools;
using System.Windows;

namespace UI_LokalniKontroler
{
    /// <summary>
    /// Interaction logic for StartupWindow.xaml
    /// </summary>
    public partial class StartupWindow : Window
    {
        public StartupWindow()
        {
            InitializeComponent();
        }

        private void btnSubmit(object sender, RoutedEventArgs e)
        {
            string code = textBoxCode.Text;
            if (code.Equals(string.Empty))
            {
                MessageBox.Show("You must provide a code!", "Code error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                DbLocalController dblc = new DbLocalController();
                if (!dblc.IsCodeFree(code))
                {
                    this.DialogResult = true;
                    var mainWindow = new MainWindow(code);
                    mainWindow.Show();
                }
                else
                {
                    MessageBox.Show("The code you entered doesn't exist!", "Code error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void OnWindowLoad(object sender, RoutedEventArgs e)
        {
            textBoxCode.Focus();
        }
    }
}
