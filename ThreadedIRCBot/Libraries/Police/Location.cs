using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Police
{
    public class Location
    {
        public class Street
        {
            public string ID { get; private set; }
            public string Name { get; private set; }

            public Street(string id, string name)
            {
                ID = id;
                Name = name;
            }
        }

        public string Latitude { get; private set; }
        public string Longitude { get; private set; }
        public Street StreetInfo { get; private set; } 

        public Location(string lat, string lng)
        {
            Latitude = lat;
            Longitude = lng;
        }
    }
}
