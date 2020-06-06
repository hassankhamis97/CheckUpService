using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LabService.Model
{
    public class ReviewModel
    {
        public long id { get; set; }
        public string description { get; set; }
        public Nullable<int> rateNumber { get; set; }
        public string userId { get; set; }
        public string date { get; set; }
    }
}