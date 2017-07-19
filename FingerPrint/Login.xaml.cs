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
using System.Windows.Navigation;
using System.Windows.Shapes;
using FingerPrint.DA;
using FingerPrint.Info;

namespace FingerPrint
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void signIn(object sender, RoutedEventArgs e)
        {
            string username = usernameBox.Text;
            string password = passwordBox.Password.ToString();
           // MessageBox.Show(username + " "+password);
            DbActions da = new DbActions();
            //List<string>[] list = da.Read();
            List<Agent> agent = da.AgentRead(username,password);
            if(agent.Count>0)
            {
                MessageBox.Show("User Verified!");
                Home h = new Home();
                h.Show();
                this.Hide();
            }

           /* foreach(Agent a in agent)
            {
                MessageBox.Show(a.Id + " " +a.Username + " "+ agent.Count);
            }*/
           // MessageBox.Show(list[0] + " " + list[1]);

        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            Application.Current.Shutdown();
        }
    }
}
