using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using FingerPrint.Info;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Net.Http.Headers;

namespace FingerPrint
{
    /// <summary>
    /// Interaction logic for CustomWindow.xaml
    /// </summary>
    public partial class CustomWindow : Window
    {
        string response;
        public CustomWindow(string response)
        {
            this.response = response;
           // MessageBox.Show(response);
            
            //JObject json = JObject.Parse(response);
            //MessageBox.Show(json.ToString());
            // JToken jtoken= json["id"];
            //MessageBox.Show(jtoken.ToString());
            //JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            // User user = JsonConvert.DeserializeObject<User>(json); 
            //MessageBox.Show(user.id);
            //this.cp = cp;
            //this.finger = finger;
            //ImageFinger.Source = null;
            //MessageBox.Show(finger.Height+" "+finger.Width);
            InitializeComponent();
            GetInfoAsync(this);
            //ImageFinger.Source = finger;

        }

        public async Task GetInfoAsync(CustomWindow cw)
        {
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("http://localhost:50211/api/Persons/GetPerson/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                User user = new User();
                HttpResponseMessage responses = await client.GetAsync("http://localhost:50211/api/Persons/GetPerson/"+response);
                if (responses.IsSuccessStatusCode)
                {
                    //user = await responses.Content.ReadAsAsync<User>();
                    string r = await responses.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<User>(r);
                    //MessageBox.Show("Custom UI: "+result.id);
                    string name= result.name.Replace("{", "");
                    name = name.Replace("}", "");
                    string gender = result.gender.Replace("{", "");
                    gender = gender.Replace("}", "");
                    string dob = result.dob.Replace("{", "");
                    dob = dob.Replace("}", "");
                    cw.textName.Text = name;
                    cw.textGender.Text = gender;
                    cw.textDOB.Text = dob;
                  
                    

                    BitmapImage bi4 = new BitmapImage();
                    bi4.BeginInit();
                    bi4.UriSource = new Uri(result.profile, UriKind.RelativeOrAbsolute);
                    bi4.CacheOption = BitmapCacheOption.OnLoad;
                    bi4.EndInit();

                    cw.ImageProfile.Source = bi4;
                  
                }
               

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void buttonOK_Click(object sender, RoutedEventArgs e)
        {
            Hide();
           // this.Close();
            //cp.Show();
        }
    }
}
