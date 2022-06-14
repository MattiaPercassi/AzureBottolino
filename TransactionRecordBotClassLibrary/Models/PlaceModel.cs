using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransactionRecordBotClassLibrary.Models
{
    public class PlaceModel
    {
        public string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public PlaceModel()
        {
            Name = "Not registered";
            Latitude = 0;
            Longitude = 0;
        }
        public PlaceModel(string name, double lat, double lon)
        {
            Name = name;
            Latitude = lat;
            Longitude = lon;
        }
    }
}
