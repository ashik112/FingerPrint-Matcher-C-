using FingerPrint.DA;
using FingerPrint.Info;
using FingerPrint.Utilities;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Formatters;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace FingerPrint
{
    /// <summary>
    /// Interaction logic for CheckPerson.xaml
    /// </summary>
    public partial class CheckPerson : Window
    {
        OpenFileDialog op;
        static byte[] ImageFinger;
        BitmapImage FingerImage;
        Home home;
        public static BitmapImage CustomImageFinger;

        public CheckPerson(Home home)
        {
            this.home = home;
            op = new OpenFileDialog();
            ImageFinger = new byte[10];
            InitializeComponent();
        }

        private void UploadFingerPrint_Click(object sender, RoutedEventArgs e)
        {
            op.Title = "Select a picture";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png;*.tif|" +
              "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
              "Portable Network Graphic (*.png)|*.png";
            if (op.ShowDialog() == true)
            {
                imageFinger.Source = new BitmapImage(new Uri(op.FileName));

                //Convert to Byte//
                FileStream fs = new FileStream(op.FileName, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);
                ImageFinger = br.ReadBytes((int)fs.Length);
                br.Close();
                fs.Close();
                FingerImage = new BitmapImage(new Uri(op.FileName, UriKind.RelativeOrAbsolute));
            }
        }
        public static HttpResponseMessage ClientRequest(string RequestURI)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:50211/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = client.GetAsync(RequestURI).Result;
            return response;
        }

        public static string HTTP_POST(string Url, string Data)
        {
            string Out = String.Empty;
            System.Net.WebRequest req = System.Net.WebRequest.Create(Url);
            try
            {
                req.Method = "POST";
                req.Timeout = 100000;
                req.ContentType = "application/x-www-form-urlencoded";
                byte[] sentData = Encoding.UTF8.GetBytes(Data);
                req.ContentLength = sentData.Length;
                using (System.IO.Stream sendStream = req.GetRequestStream())
                {
                    sendStream.Write(sentData, 0, sentData.Length);
                    sendStream.Close();
                }
                System.Net.WebResponse res = req.GetResponse();
              
                System.IO.Stream ReceiveStream = res.GetResponseStream();
                using (System.IO.StreamReader sr = new System.IO.StreamReader(ReceiveStream, Encoding.UTF8))
                {
                    Char[] read = new Char[256];
                    int count = sr.Read(read, 0, 256);

                    while (count > 0)
                    {
                        String str = new String(read, 0, count);
                        MessageBox.Show(str);
                        Out += str;
                        count = sr.Read(read, 0, 256);
                    }
                }
            }
            catch (ArgumentException ex)
            {
                Out = string.Format("HTTP_ERROR :: The second HttpWebRequest object has raised an Argument Exception as 'Connection' Property is set to 'Close' :: {0}", ex.Message);
            }
            catch (WebException ex)
            {
                Out = string.Format("HTTP_ERROR :: WebException raised! :: {0}", ex.Message);
            }
            catch (Exception ex)
            {
                Out = string.Format("HTTP_ERROR :: Exception raised! :: {0}", ex.Message);
            }

            return Out;
        }

        private static async void Upload()
        {
            HttpClient client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, "http://localhost:50211/api/upload");
            var content = new MultipartFormDataContent();

            byte[] byteArray = ImageFinger;
            //content.Add(new ByteArrayContent(byteArray), "file", "file.jpg");
            content.Add(new ByteArrayContent(byteArray));
            request.Content = content;
            //request.Content = new ByteArrayContent(byteArray);

            var response = await client.SendAsync(request);
            //response.EnsureSuccessStatusCode();
            MessageBox.Show(await response.Content.ReadAsStringAsync() );
            //return await response.Content.ReadAsStringAsync();
        }
        private  void CheckPerson_Click(object sender, RoutedEventArgs e)
        {
            Upload();
            
            
            
            
            //.GetAwaiter().GetResult();
            //Console.WriteLine(responsePayload);
           
            /* string s = Convert.ToBase64String(ImageFinger);
             MessageBox.Show(s.Length+"");
             string Out= HTTP_POST("http://localhost:50211/api/Persons/PostPersonByFinger?myarray=", s);
             MessageBox.Show(Out+"");*/

            //string s4 = HttpServerUtility.UrlTokenEncode(ImageFinger);
            /* HttpResponseMessage responseEmployeeByCity = ClientRequest("api/Persons/GetPersonByFinger?finger=" + s4);
             responseEmployeeByCity.EnsureSuccessStatusCode();
             if (responseEmployeeByCity.IsSuccessStatusCode)
             {
                 MessageBox.Show("Success");
                 List<User> _Empdata = responseEmployeeByCity.Content.ReadAsAsync<List<User>>().Result;
                 foreach (User _Empdata1 in _Empdata)
                 {
                     //Console.WriteLine("{0}\t{1}\t{2}\t{3}", _Empdata1.Uid, _Empdata1.Name, _Empdata1.Address, _Empdata1.City);
                 }
             }*/



            //byte[] data = new byte[] { 1, 2, 3, 4, 5 };
            //HttpClient client = new HttpClient();
            //Uri uri = new Uri("http://localhost:50211/api/Persons/PostPersonByFinger");
            /*var content = new FormUrlEncodedContent(new[]
           {
                new KeyValuePair<string, string>("finger", s4)
            });*/
            // StringContent stringContent = new StringContent(s4);
            //ByteArrayContent byteContent = new ByteArrayContent(ImageFinger);
            /* string s= Convert.ToBase64String(ImageFinger);
             StringContent stringContent = new StringContent(s);
             HttpResponseMessage reponse = await client.PostAsync(uri, stringContent);
             MessageBox.Show(reponse.ToString());*/

            /*var jsonString = await reponse.Content.ReadAsStringAsync();
            var _Data = JsonConvert.DeserializeObject<List<User>>(jsonString);
            foreach (User user in _Data)
            {
                MessageBox.Show(user.id);
            }*/

            /*try
            {

                 MyPerson person = CloudAFIS.GetTemplateObject(FingerImage);
            
                 int result = CloudAFIS.CheckPerson(person);
                 if(result>=0)
                 {
                     DbActions db = new DbActions();
                     User u= db.GetPerson(result);
                     MessageBox.Show("Person Found!"+"\n"+"Name: "+u.name
                         +"\n"+"Gender: "+u.gender+"\n");
                 }
                 else
                 {
                     MessageBox.Show("No Person Exist!");
                 }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Please enter a finger print!\n"+"Error: "+ex.Message);
            }*/
        }
        //Initialize HTTP Client 
        /* private async void CheckPerson_Click(object sender, RoutedEventArgs e)
         {*/
        //string response1 = GET("http://localhost:50211/api/persons/");
        /* // HttpClient client = new HttpClient();
         client.BaseAddress = new Uri("http://localhost:50211");
         client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));// */
        /*
        {
            HttpClient WebClient = new HttpClient();
            Uri uri = new Uri("http://localhost:50211/api/persons/");
            HttpResponseMessage response = await WebClient.GetAsync(uri);
            var jsonString = await response.Content.ReadAsStringAsync();
            var _Data = JsonConvert.DeserializeObject<List<User>>(jsonString);
            foreach(User user in _Data)
            {*/
        //MessageBox.Show(String.Format("{0,10:X}", ));
        //  MessageBox.Show(user.template.Length.ToString());
        //Bitmap image = Converters.BytetoBitmap((user.finger));
        // CustomImageFinger = Converters.BitmapToImageSource(image);

        /*  await Task.Run(() =>
          {
              //Do work here
              Thread.Sleep(5000);
          });*/





        //imageFinger.SourceUpdated(true);
        // imageFinger.Freeze(); // necessary for cross-thread access

        //imageFinger.Dispatcher.BeginInvoke(new Action(() => imageFinger.Source = Converters.ByteToImage((user.finger))));
        /*imageFinger.Source = Converters.ByteToBitmapImage(user.finger);
    }
    MessageBox.Show("Got All Persons");

}
catch (Exception ex)
{
    MessageBox.Show("Persons not Found: "+ ex.Message);
}
CustomWindow cw = new CustomWindow(this, CustomImageFinger);
cw.Show();
// utility = new Converters();
//utility.SavePhoto(CustomImageFinger);
}*/


        /*
         * Http Get Request
         * */
        public string GET(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            try
            {
                WebResponse response = request.GetResponse();
                using (Stream responseStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                    return reader.ReadToEnd();
                }
            }
            catch (WebException ex)
            {
                WebResponse errorResponse = ex.Response;
                using (Stream responseStream = errorResponse.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.GetEncoding("utf-8"));
                    String errorText = reader.ReadToEnd();
                    // log errorText
                }
                throw;
            }
        }

        private void LogOut_Click(object sender, RoutedEventArgs e)
        {
            //AfisEngine Afis = new AfisEngine();
            // Finger fp = new Finger();

            //= imageFinger.Source;
            this.Hide();
            home.Show();
        }
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            Application.Current.Shutdown();
        }
    }
}
