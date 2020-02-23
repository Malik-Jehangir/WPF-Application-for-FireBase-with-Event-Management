using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spot_Backend
{
    class Events
    {
        private string description;
        private string end_date_time;
        private string image_url;
        private string location;
        private string name;
        private string start_date_time;

        public Events(string description, string end_date_time, string image_url, string location, string name, string start_date_time)
        {
            Description = description;
            End_date_time = end_date_time;
            Image_url = image_url;
            Location = location;
            Name = name;
            Start_date_time = start_date_time;
            
        }

        public Events()
        {
            Description = null;
            End_date_time = null;
            Image_url = null;
            Location = null;
            Name = null;
            Start_date_time = null;
           

        }

        public string Description { get => description; set => description = value; }
        public string End_date_time { get => end_date_time; set => end_date_time = value; }
        public string Image_url { get => image_url; set => image_url = value; }
        public string Location { get => location; set => location = value; }
        public string Name { get => name; set => name = value; }
        public string Start_date_time { get => start_date_time; set => start_date_time = value; }


    }
}
