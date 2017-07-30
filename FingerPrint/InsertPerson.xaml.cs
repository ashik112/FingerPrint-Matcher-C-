using FingerPrint.Info;
using FingerPrint.Utilities;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace FingerPrint
{

    /// <summary>
    /// Interaction logic for InsertPerson.xaml
    /// </summary>
    /// 


    public partial  class  InsertPerson : Window
    {
        
        public static  Home home;
        static byte[] ImageProfile;
        static byte[] ImageFinger;
        string gender;
        BitmapImage FingerImage;

       

        OpenFileDialog op;
        //OpenFileDialog op2;

        public InsertPerson( Home h)
        {
            op = new OpenFileDialog();
            home = h;
            ImageProfile = new byte[10];
            ImageFinger = new byte[10];
            InitializeComponent();


        }

        private void close_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            home.Show();
            //this.Close();
        }

        private void uploadProfilePic_Click(object sender, RoutedEventArgs e)
        {
            op.Title = "Select a picture";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png;*.tif|" +
              "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
              "Portable Network Graphic (*.png)|*.png";
            if (op.ShowDialog() == true)
            {
                string ImagePath = op.FileName;

                /*
                 * TIF to JPEG
                 * */
                bool tif = false;
                if (op.FileName.Contains(".tif"))
                {
                    Bitmap.FromFile(op.FileName).Save(Path.GetDirectoryName(op.FileName)+"\\"+Path.GetFileNameWithoutExtension(op.FileName) + "_Profile.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
                    ImagePath = Path.GetDirectoryName(op.FileName) + "\\" + Path.GetFileNameWithoutExtension(op.FileName) + "_Profile.jpg";
                    tif = true;
                }   

                FileStream fs = new FileStream(ImagePath, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);
                ImageProfile = br.ReadBytes((int)fs.Length);
                br.Close();
                fs.Close();

                /*
                 * Delete Image
                 * */
                if(tif)
                {
                    if (File.Exists(ImagePath))
                    {
                        File.Delete(ImagePath);
                    }

                }
               
                imageProfile.Source = new BitmapImage(new Uri(op.FileName));
               
            }

        }
   

        private void uploadFingerPrint_Click(object sender, RoutedEventArgs e)
        {
            op.Title = "Select a picture";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png;*.tif|" +
              "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
              "Portable Network Graphic (*.png)|*.png";
            if (op.ShowDialog() == true)
            {
                bool tif = false;
                string ImagePath = op.FileName;

                /*
                 * TIF to JPEG
                 * */
                if (op.FileName.Contains(".tif"))
                {
                    Bitmap.FromFile(op.FileName).Save(Path.GetDirectoryName(op.FileName) + "\\" + Path.GetFileNameWithoutExtension(op.FileName) + "_Finger.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);


                    ImagePath = Path.GetDirectoryName(op.FileName) + "\\" + Path.GetFileNameWithoutExtension(op.FileName) + "_Finger.jpg";
                    tif = true;
                   
                }
                //Convert to Byte//
                FileStream fs = new FileStream(ImagePath, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);
                ImageFinger = br.ReadBytes((int)fs.Length);
                br.Close();
                fs.Close();



                //FingerImage = new BitmapImage(new Uri(ImagePath, UriKind.RelativeOrAbsolute));
                FingerImage = Converters.ByteToBitmapImage(ImageFinger);

                //De convert//

                //imageProfile = convert.ByteToImage(ImageProfile);
                /*
                * Delete Image
                * */
                if (tif)
                {
                    if (File.Exists(ImagePath))
                    {
                        File.Delete(ImagePath);
                    }

                }

                imageFinger.Source = new BitmapImage(new Uri(op.FileName));
                

            }
        }

        private async void submit_Click(object sender, RoutedEventArgs e)
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
                if (res.Length > 0 && imageFinger.Source != null)
                {
                    MessageBox.Show("Person Already Exist");
                    // CustomWindow cs = new CustomWindow(res);
                    //  cs.Show();
                }
                else
                {
                    UploadTemplate(textName.Text, gender, textDOB.Text, "", "", this, home);
                    //MessageBox.Show("Can not find a match!");
                }

                //return await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Occured: " + ex.Message);
            }



            /* MyPerson person = CloudAFIS.GetTemplateObject(FingerImage);
             List<MyPerson> p = new List<MyPerson>();
             p.Add(person);
             BinaryFormatter formatter = new BinaryFormatter();
             MemoryStream ms = new MemoryStream();
             formatter.Serialize(ms, person);
             byte[] steam = ms.GetBuffer();*/
           // UploadTemplate(textName.Text, gender, textDOB.Text, "", "", this, home);



            //  MyPerson person = CloudAFIS.GetTemplateObject(Converters.ByteToBitmapImage(ImageFinger));
            //  MyPerson person = CloudAFIS.GetTemplateObject(FingerImage);
            // string person = cloud.GetTemplate(Converters.ByteToBitmapImage(ImageFinger));


            // Converters convert = new Converters();
            // MyFingerprint fp = new MyFingerprint();
            // fp.AsBitmapSource = Converters.ByteToBitmapImage(ImageFinger);
            //  MyPerson person = new MyPerson();
            //  person.Name = textName.Text;
            // Add fingerprint to the person
            // person.Fingerprints.Add(fp);
            //  Afis.Extract(person);

            /*DbActions db = new DbActions();
            bool status=db.CreateUser(ImageProfile,textName.Text,gender,textDOB.Text, ImageFinger, person);
            if(status)
            {
                MessageBox.Show("Data Insert Successful!");
            }
            else
            {
                MessageBox.Show("Data Insert Failed!");
            }*/
        }

        public static async Task<string> UploadProfile()
        {
           

            HttpClient client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, "http://localhost:50211/api/upload/uploadprofile");
            var content = new MultipartFormDataContent();

            byte[] byteArray = ImageProfile;
            //content.Add(new ByteArrayContent(byteArray), "file", "file.jpg");
            content.Add(new ByteArrayContent(byteArray));
            request.Content = content;
            //request.Content = new ByteArrayContent(byteArray);

            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            //MessageBox.Show(await response.Content.ReadAsStringAsync());
           // return await response.Content.ReadAsStringAsync();
           return await response.Content.ReadAsStringAsync();
        }

        public static async Task<string> UploadFinger()
        {

            HttpClient client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, "http://localhost:50211/api/upload/uploadfinger");
            var content = new MultipartFormDataContent();

            byte[] byteArray = ImageFinger;
            //content.Add(new ByteArrayContent(byteArray), "file", "file.jpg");
            content.Add(new ByteArrayContent(byteArray));
            request.Content = content;
            //request.Content = new ByteArrayContent(byteArray);

            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            //MessageBox.Show(await response.Content.ReadAsStringAsync());
            return await response.Content.ReadAsStringAsync();
            //return await response.Content.ReadAsStringAsync();
        }



        public void WorkThreadFunction()
        {
            try
            {
                // do any background work
            }
            catch (Exception ex)
            {
                // log errors
            }
        }

        public static async void UploadTemplate(string name,string gender, string dob,string plocation,string flocation, InsertPerson person, Home home)
        {
            try
            {

                //Thread thread = new Thread(new ThreadStart(WorkThreadFunction));
                //thread.Start();

                Task<string> p = UploadProfile();
                plocation = await p;

                Task<string> f = UploadFinger();
                // flocation = UploadFinger().GetAwaiter().GetResult();
                flocation = await f;
                /*plocation = plocation.Replace('"', ' ').Trim();
                flocation = flocation.Replace('"', ' ').Trim();

                plocation = plocation.Replace(@"\\", @"\");
                flocation = flocation.Replace(@"\\", @"\");*/
                //MessageBox.Show(plocation+ "\n\n\n "+flocation);

                HttpClient client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Post, "http://localhost:50211/api/upload/uploadperson?name=" + "{" + name + "}&" + "gender=" + "{" + gender + "}" + "&" + "dob=" + "{" + dob + "}"
                    + "&" + "plocation=" + "{" + plocation + "}" + "&" + "flocation=" + "{" + flocation + "}");
                var content = new MultipartFormDataContent();

                //byte[] byteArray = template;
                //content.Add(new ByteArrayContent(byteArray), "file", "file.jpg");
                //content.Add(new ByteArrayContent(byteArray));
                //content.Add(new StringContent("name"));
                //content.Add(new StringContent("gender"));
                //content.Add(new StringContent("dob"));
                request.Content = content;
                // request.Content = new ByteArrayContent(byteArray);

                var response = await client.SendAsync(request);
                //response.EnsureSuccessStatusCode();
                MessageBox.Show(await response.Content.ReadAsStringAsync());
                string check = await response.Content.ReadAsStringAsync();
                if (check.Contains("Successful"))
                {
                    home.Show();
                    person.Hide();
                    //person.Close();

                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error Occured: "+ex.Message);
            }
            // return await response.Content.ReadAsStringAsync();
            //return await response.Content.ReadAsStringAsync();
        }

        

        private void radioMale_Checked(object sender, RoutedEventArgs e)
        {
            gender = radioMale.Content.ToString();
        }

        private void radioFemale_Checked(object sender, RoutedEventArgs e)
        {
            gender = radioFemale.Content.ToString();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            Application.Current.Shutdown();
        }
    }
}
