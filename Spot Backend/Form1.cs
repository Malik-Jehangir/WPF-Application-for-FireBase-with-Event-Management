using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Spot_Backend
{
    public partial class Form1 : Form
    {
        int s = 0;
        private const string DATA = @"{""object"":{""name"":""Name""}}";
        List<Events> event_upb = new List<Events>();
        
        public Form1()
        {
            InitializeComponent();
        }


        void deleteAll()
        {
            var request = WebRequest.CreateHttp("https://spot-db52f.firebaseio.com/.json");
            request.Method = "DELETE";
            request.ContentType = "application/json";
            var response = request.GetResponse();
        }

        void sendResult()
        {
            deleteAll();//delete all the previous records
            int i = 0;
               foreach(var item in event_upb)
            {
                i = i + 1;
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(new
                {
                    
                    Event = item
                });

                var request = (HttpWebRequest)WebRequest.Create("https://spot-db52f.firebaseio.com/" + i + ".json");
                request.Method = "PUT";
                request.ContentType = "application/json";
                var buffer = Encoding.UTF8.GetBytes(json);
                request.ContentLength = buffer.Length;
               
                request.GetRequestStream().Write(buffer, 0, buffer.Length);
               

                var response = request.GetResponse()as HttpWebResponse;
                StreamReader sr = new StreamReader(response.GetResponseStream());
                json = (sr.ReadToEnd());
                
                response.Dispose();
            }

            event_upb.Clear();


        }
        int j = 0;
        int p = 0;
        private void button2_Click(object sender, EventArgs e)
        {


            Int32.TryParse(DateTime.Now.Month.ToString(), out j);
            Int32.TryParse(DateTime.Now.Year.ToString(), out p);


            for (int k = j; k <= 12; k++)
            {
                
                var request = WebRequest.CreateHttp("https://asta.uni-paderborn.de/events/"+p.ToString()+"-"+k.ToString("00")+"/?ical=1");
                request.Method = "POST";
                request.ContentType = "application/json";
                request.ContentLength = DATA.Length;
                StreamWriter requestWriter = new StreamWriter(request.GetRequestStream(), System.Text.Encoding.ASCII);
                requestWriter.Write(DATA);
                requestWriter.Close();

                WebResponse webResponse = request.GetResponse();
                Stream webStream = webResponse.GetResponseStream();
                StreamReader responseReader = new StreamReader(webStream);
                string response = responseReader.ReadToEnd();
                //MessageBox.Show(response.ToString());
                responseReader.Close();


                String[] splitText2;
                String[] splitText1 = response.Split(new string[] { "BEGIN" }, StringSplitOptions.None);
                for (int i = 2; i < splitText1.Length; i++)
                {
                    Events ev = new Events();
                    splitText2 = splitText1[i].Split('\n');

                    for (int j = 0; j < splitText2.Length; j++)
                    {
                        if (splitText2[j].Contains("DESCRIPTION") == true)
                        {
                            try
                            {
                                ev.Description = splitText2[j].Split(new string[] { "DESCRIPTION:" }, StringSplitOptions.None)[1];

                            }
                            catch
                            {
                                ev.Description = "";
                            }
                        }
                        else if (splitText2[j].Contains("DTEND") == true)
                        {
                            try
                            {
                                ev.End_date_time = splitText2[j].Split(new string[] { "DTEND;TZID=Europe/Berlin:" }, StringSplitOptions.None)[1];
                            }
                            catch
                            {
                                ev.End_date_time = "";
                            }
                        }
                        else if (splitText2[j].Contains("URL") == true)
                        {
                            try
                            {
                                ev.Image_url = splitText2[j].Split(new string[] { "URL:" }, StringSplitOptions.None)[1];

                            }
                            catch
                            {
                                ev.Image_url = "";
                            }
                        }
                        else if (splitText2[j].Contains("SUMMARY") == true)
                        {
                            try
                            {
                                ev.Name = splitText2[j].Split(new string[] { "SUMMARY:" }, StringSplitOptions.None)[1];
                            }
                            catch
                            {
                                ev.Name = "";
                            }

                        }
                        else if (splitText2[j].Contains("DTSTART") == true)
                        {
                            try
                            {
                                ev.Start_date_time = splitText2[j].Split(new string[] { "DTSTART;TZID=Europe/Berlin:" }, StringSplitOptions.None)[1];
                            }
                            catch
                            {
                                ev.Start_date_time = "";
                            }


                        }



                    }
                    ev.Location = "ASTA Paderborn";
                    event_upb.Add(ev);
                }
            }
           

            MessageBox.Show("Events updated in Firebase successfully!");
            sendResult();
        }

    
    }
    
}
