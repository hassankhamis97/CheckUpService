using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Web;

namespace LabService.Model
{
    public class LabBranchMenu
    {

        public string idFB { get; set; }
        public string govern { get; set; }
        public AddressModel address { get; set; }
        public int rating { get; set; }
        public bool isAvailableFromHome { get; set; }
        public double distance
        {
            get
            {
                return Math.Round(userGeoCoordinate.GetDistanceTo(branchGeoCoordinate) / 1000 , 2);
            }
        }
        [JsonIgnore]
        public GeoCoordinate branchGeoCoordinate { get; set; }
        [JsonIgnore]

        public GeoCoordinate userGeoCoordinate { get; set; }



    }


    public class AddressModel
    {
        public long Id { get; set; }
        public string address1 { get; set; }
        public string buildingNo { get; set; }
        public string floorNo { get; set; }
        public string apartmentNo { get; set; }
        public double longitude { get; set; }
        public double latitude { get; set; }
    }

}