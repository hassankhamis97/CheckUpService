using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace LabService.Model
{
    public class UserTest
    {
        public long TestId { get; set; }
        public string UserId { get; set; }
        
        public AddressModel address { get; set; }
        public string timeForTakingSample { get; set; }
        public string dateForTakingSample { get; set; }
        public string generatedCode { get; set; }
        public DateTime? TakingSampleDate { get {
           // var cultureInfo = CultureInfo.CreateSpecificCulture("ar-SA");

            return DateTime.ParseExact(dateForTakingSample + " " + timeForTakingSample, "MMM d, yyyy h:mm tt", 
                null); 
        } }


    }
}