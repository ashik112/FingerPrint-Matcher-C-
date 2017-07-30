using Microsoft.Win32;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Windows;
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

        private static async void Upload(CheckPerson cp)
        {
            try
            {
                HttpClient client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Post, "http://localhost:50211/api/check");
                var content = new MultipartFormDataContent();

                byte[] byteArray = ImageFinger;
                //content.Add(new ByteArrayContent(byteArray), "file", "file.jpg");
                content.Add(new ByteArrayContent(byteArray));
                request.Content = content;
                //request.Content = new ByteArrayContent(byteArray);

                var response = await client.SendAsync(request);
                //response.EnsureSuccessStatusCode();
                string res = await response.Content.ReadAsStringAsync();
               // MessageBox.Show(await response.Content.ReadAsStringAsync());
                if(res.Length>0&& cp.imageFinger.Source != null)
                {

                    CustomWindow cs = new CustomWindow(res);
                    cs.Show();
                }
                else
                {
                    MessageBox.Show("Can not find a match!");
                }
               
                //return await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Occured: " + ex.Message);
            }

        }
        private  void CheckPerson_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Upload(this);
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error Occured: "+ex.Message);
            }
        }
        

        private void LogOut_Click(object sender, RoutedEventArgs e)
        {
            home.Show();
            this.Hide();
           // home.Show();
        }
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.Shutdown();
        }
    }
}
