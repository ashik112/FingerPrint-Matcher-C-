using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Media.Imaging;
using Newtonsoft.Json;
using FingerPrint.DA;
using FingerPrint.Info;
using SourceAFIS.Simple; // import namespace SourceAFIS.Simple
using System.Diagnostics;
using System.Drawing;
using System.Runtime.Serialization;

namespace FingerPrint.Utilities
{
    public class CloudAFIS
    {
        static AfisEngine Afis;

        public CloudAFIS()
        {
            // Initialize SourceAFIS
        }

      

        // Initialize path to images
       // static readonly string ImagePath = Path.Combine(Path.Combine("..", ".."), "images");

        // Shared AfisEngine instance (cannot be shared between different threads though)
        

        public byte[] GetTemplate(BitmapImage image)
        {
            Afis = new AfisEngine();
            MyFinger fp = new MyFinger();
            fp.AsBitmapSource = image;
            //    MyPerson person = new MyPerson();
            //person.Name = name;
            // Add fingerprint to the person
            MyPerson person = new MyPerson();
            person.Fingerprints.Add(fp);
            Afis.Extract(person);
            byte[] s=Converters.ObjectToByteArray(person);
            return s;
        }

        public static MyPerson GetTemplateObject(BitmapImage image)
        {
            
            Afis = new AfisEngine();
            MyFinger fp = new MyFinger();
            fp.AsBitmapSource = image;
            MyPerson person = new MyPerson();
            //person.Name = name;
            // Add fingerprint to the person
          //  MyPerson person = new MyPerson();
            person.Fingerprints.Add(fp);
               Afis.Extract(person);
           
            //string s = Converters.ObjectSerializer(person);
            return person;
        }

        //Deserialize byte[] data to object//
        private static T Deserialize<T>(byte[] param)
        {
            using (MemoryStream ms = new MemoryStream(param))
            {
                IFormatter br = new BinaryFormatter();
                return (T)br.Deserialize(ms);
            }
        }

        public static int CheckPerson(MyPerson person)
        {
            DbActions action = new DbActions();
            Afis = new AfisEngine();
            List<MyPerson> persons = new List<MyPerson>();
            List<User> user = action.GetPersons();
            if(user.Count > 0)
            {
                foreach (User u in user)
                {         

                    //Deserialize byte[] data to object//
                    MyPerson p = Deserialize<MyPerson>(u.template);


                    p.id = u.id;
                    Trace.WriteLine(u.template);
                    persons.Add(p);
                }
            }
            MyPerson match = Afis.Identify(person, persons).FirstOrDefault() as MyPerson;
            // Null result means that there is no candidate with similarity score above threshold
            if (match == null)
            {
                Debug.WriteLine("No matching person found.");
                return -1;
                //return;
            }
            // Print out any non-null result
            else
            {
                //Trace.WriteLine("Matches registered person {0}",  match.id);
                // Compute similarity score
                float score = Afis.Verify(person, match);
                Debug.WriteLine("Similarity score between person and match ,"+score+" ID: "+match.id);
                return Convert.ToInt32(match.id);
            }
        }

        public static void Main()
        {
            

        }

    }
}
