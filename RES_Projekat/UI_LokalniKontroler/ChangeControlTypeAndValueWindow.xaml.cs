using Common.DbTools;
using Common.Enums;
using Common.Model;
using System;
using System.Windows;

namespace UI_LokalniKontroler
{
    /// <summary>
    /// Interaction logic for ChangeControlTypeAndValueWindow.xaml
    /// </summary>
    public partial class ChangeControlTypeAndValueWindow : Window
    {
        Generator g;
        private DbGenerator dbg = new DbGenerator();

        public ChangeControlTypeAndValueWindow(Generator g)
        {
            txt_box_trenutnaSnaga = new System.Windows.Controls.TextBox();

            InitializeComponent();

            this.g = g;
            slider.Value = (int)g.Control;

            if (slider.Value == 0)
            {
                txt_box_trenutnaSnaga.IsReadOnly = false;
            }
            else
            {
                txt_box_trenutnaSnaga.Text = "";
                txt_box_trenutnaSnaga.IsReadOnly = true;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int newActivePowerValue = 0;

                if (slider.Value == 0)
                {
                    newActivePowerValue = Int32.Parse(txt_box_trenutnaSnaga.Text);
                }
                else
                {
                    newActivePowerValue = 0;
                }

                dbg.ChangeType(g.Id, newActivePowerValue, (EControl)slider.Value);

                Close();
            }
            catch 
            {
                MessageBox.Show("If control type is LOCAL, you need to set an initial power value.", "Error", MessageBoxButton.OK);
            }
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (slider.Value == 0)
            {
                txt_box_trenutnaSnaga.IsReadOnly = false;
            }
            else
            {
                txt_box_trenutnaSnaga.Text = "";
                txt_box_trenutnaSnaga.IsReadOnly = true;
            }
        }
    }
}
