using FingerPrint.Info;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Windows;

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


        public async Task<string> GetInfoAsync(string username, string password)
        {
            try
            {
                HttpClient client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Post, "http://localhost:50211/api/persons/checkagent?username=" + "" + username + "&" + "password=" + "" + password + "" );
                var content = new MultipartFormDataContent();
                request.Content = content;
                var response = await client.SendAsync(request);
                //MessageBox.Show(await response.Content.ReadAsStringAsync());
                /*if (responses.IsSuccessStatusCode)
                {
                    //user = await responses.Content.ReadAsAsync<User>();
                    string r = await responses.Content.ReadAsStringAsync();
                    MessageBox.Show(r);
                    return await responses.Content.ReadAsStringAsync();
                }
                else
                {
                    return "not success";
                }*/
                return await response.Content.ReadAsStringAsync();


            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                return "false";
            }
        }
        

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            Application.Current.Shutdown();
        }

        private async void signIn(object sender, RoutedEventArgs e)
        {
            string username = usernameBox.Text;
            string password = passwordBox.Password.ToString();
           
            string check = await GetInfoAsync(usernameBox.Text, passwordBox.Password.ToString());
            //MessageBox.Show(check);
            if (check.Contains("true"))
            {
                //MessageBox.Show(check);
                Home h = new Home();
                h.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Username and Password doesn't match");
            }
        }
    }
}
