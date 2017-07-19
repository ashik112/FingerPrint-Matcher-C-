using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using FingerPrint.DA;
using System.Windows.Media;
using FingerPrint.Utilities;
using SourceAFIS.Simple;
using System.Drawing;
using FingerPrint.Info;

namespace FingerPrint
{
    /// <summary>
    /// Interaction logic for InsertPerson.xaml
    /// </summary>
    /// 
   

    public partial class InsertPerson : Window
    {
        
        private Home home;
        byte[] ImageProfile;
        byte[] ImageFinger;
        string gender;
        BitmapImage FingerImage;

       

        OpenFileDialog op;
        OpenFileDialog op2;

        public InsertPerson( Home home)
        {
            op = new OpenFileDialog();
            this.home = home;
            ImageProfile = new byte[10];
            ImageFinger = new byte[10];
            InitializeComponent();


        }

        private void close_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            this.home.Show();
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
                if (op.FileName.Contains(".tif"))
                {
                    Bitmap.FromFile(op.FileName).Save(Path.GetDirectoryName(op.FileName)+"\\"+Path.GetFileNameWithoutExtension(op.FileName) + "_Profile.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
                    ImagePath = Path.GetDirectoryName(op.FileName) + "\\" + Path.GetFileNameWithoutExtension(op.FileName) + "_Profile.jpg";                   
                }   

                FileStream fs = new FileStream(ImagePath, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);
                ImageProfile = br.ReadBytes((int)fs.Length);
                br.Close();
                fs.Close();

                /*
                 * Delete Image
                 * */
                if (File.Exists(ImagePath))
                {
                    File.Delete(ImagePath);
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
               
                string ImagePath = op.FileName;

                /*
                 * TIF to JPEG
                 * */
                if (op.FileName.Contains(".tif"))
                {
                    Bitmap.FromFile(op.FileName).Save(Path.GetDirectoryName(op.FileName) + "\\" + Path.GetFileNameWithoutExtension(op.FileName) + "_Finger.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);


                    ImagePath = Path.GetDirectoryName(op.FileName) + "\\" + Path.GetFileNameWithoutExtension(op.FileName) + "_Finger.jpg";
                   
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
                if (File.Exists(ImagePath))
                {
                    File.Delete(ImagePath);
                }
                
                imageFinger.Source = new BitmapImage(new Uri(op.FileName));
                

            }
        }

        private void submit_Click(object sender, RoutedEventArgs e)
        {
            MyPerson person = CloudAFIS.GetTemplateObject(FingerImage);
            int result = CloudAFIS.CheckPerson(person);
            if (result >= 0)
            {
                MessageBox.Show("Person already exist!!!");
                return;
            }
           

            CloudAFIS cloud = new CloudAFIS();
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

            DbActions db = new DbActions();
            bool status=db.CreateUser(ImageProfile,textName.Text,gender,textDOB.Text, ImageFinger, person);
            if(status)
            {
                MessageBox.Show("Data Insert Successful!");
            }
            else
            {
                MessageBox.Show("Data Insert Failed!");
            }
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
