using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SourceAFIS.Simple;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Diagnostics;
using System.Runtime.Serialization.Formatters.Binary;
using FingerPrint.Info;
using System.Drawing;
using System.ComponentModel;
using System.Net.Http;

namespace FingerPrint.Utilities
{
    public class Converters
    {

        


        // Byte to ImageSource Converter
        public static ImageSource ByteToImage(byte[] imageData)
        {
            BitmapImage biImg = new BitmapImage();
            MemoryStream ms = new MemoryStream(imageData);
            biImg.BeginInit();
            biImg.StreamSource = ms;
            biImg.EndInit();

            ImageSource imgSrc = biImg as ImageSource;
            BitmapImage imgSrc2 = biImg as BitmapImage;

            return imgSrc;
        }
        public static BitmapImage ByteToBitmapImage(byte[] imageData)
        {
            BitmapImage biImg = new BitmapImage();
            MemoryStream ms = new MemoryStream(imageData);
            biImg.BeginInit();
            biImg.StreamSource = ms;
            biImg.EndInit();

            // ImageSource imgSrc = biImg as ImageSource;
            BitmapImage imgSrc = biImg as BitmapImage;

            return imgSrc;
        }

        public static BitmapImage BitmapToImageSource(Bitmap bitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();

                return bitmapimage;
            }
        }

        public static BitmapImage ByteToBitmapImage2(byte[] array)
        {
            using (var ms = new MemoryStream(array))
            {
                var image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad; // here
                image.StreamSource = ms;
                image.EndInit();
                return image;
            }
        }

        //Serialize Object using JsonConvert//
        public static string ObjectSerializer(Person person1)
        {
            MyPerson person = (MyPerson)person1;
            string s = JsonConvert.SerializeObject(person);
            Trace.WriteLine("Serialize Person: "+s);
            // WriteAllText creates a file, writes the specified string to the file,
            // and then closes the file.    You do NOT need to call Flush() or Close().
            System.IO.File.WriteAllText(@"WriteText.txt", JsonConvert.SerializeObject(person));
            return s;
        }

        //Deserialize Object using JsonConvert//
        public static MyPerson ObjectDeserializer(string s)
        {
            try
            {
                MyPerson person = JsonConvert.DeserializeObject<MyPerson>(s);
                return person;
            }
            catch(Exception ex)
            {
                Trace.WriteLine(ex.Message);
                MyPerson person = null;
                return person;
            }
          
            
        }

        // 1. Convert Object to byte //
        public static byte[] ObjectToByteArray(Person obj)
        {
            if (obj == null)
                return null;

            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, obj);

            return ms.ToArray();
        }

        // 2. Convert a byte array to an Object //
        public static Person ByteArrayToObject(byte[] arrBytes)
        {
            MemoryStream memStream = new MemoryStream();
            BinaryFormatter binForm = new BinaryFormatter();
            memStream.Write(arrBytes, 0, arrBytes.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            Person obj = (Person)binForm.Deserialize(memStream);
            return obj;
        }

        public void SavePhoto(BitmapImage image)
        {
            //BitmapImage objImage = new BitmapImage(new Uri(istrImagePath, UriKind.RelativeOrAbsolute));
            BitmapImage objImage = image;

            objImage.DownloadCompleted += objImage_DownloadCompleted;
        }

        private void objImage_DownloadCompleted(object sender, EventArgs e)
        {
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            Guid photoID = System.Guid.NewGuid();
            String photolocation = photoID.ToString() + ".jpg";  //file name 

            encoder.Frames.Add(BitmapFrame.Create((BitmapImage)sender));

            using (var filestream = new FileStream(photolocation, FileMode.Create))
                encoder.Save(filestream);
        }

        public static Bitmap BytetoBitmap(byte[] byteArray)
        {
            TypeConverter tc = TypeDescriptor.GetConverter(typeof(Bitmap));
            Bitmap bitmap1 = (Bitmap)tc.ConvertFrom(byteArray);
            return bitmap1;

        }




        /*    private static BitmapImage LoadImage(byte[] imageData)
      {
          if (imageData == null || imageData.Length == 0) return null;
          var image = new BitmapImage();
          using (var mem = new MemoryStream(imageData))
          {
              mem.Position = 0;
              image.BeginInit();
              image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
              image.CacheOption = BitmapCacheOption.OnLoad;
              image.UriSource = null;
              image.StreamSource = mem;
              image.EndInit();
          }
          image.Freeze();
          return image;
      }*/

        public static void Main() { }
    }
}
