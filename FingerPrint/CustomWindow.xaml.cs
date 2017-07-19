using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FingerPrint
{
    /// <summary>
    /// Interaction logic for CustomWindow.xaml
    /// </summary>
    public partial class CustomWindow : Window
    {
        CheckPerson cp;
        BitmapImage finger;
        public CustomWindow(CheckPerson cp, BitmapImage finger)
        {
            this.cp = cp;
            this.finger = finger;
            //ImageFinger.Source = null;
            //MessageBox.Show(finger.Height+" "+finger.Width);
            InitializeComponent();
            ImageFinger.Source = finger;

        }
       

        private void buttonOK_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            this.Close();
            //cp.Show();
        }
    }
}
