using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LabService.Model
{
    public class HealthProfileModel
    {
        public long Id { get; set; }
        public Nullable<bool> isSuffreDiabetes { get; set; }
        public Nullable<bool> isSuffrePressure { get; set; }
        public Nullable<bool> isSTakeantiBiotic { get; set; }
        public Nullable<bool> isTakehaemophilia { get; set; }
        public string userId { get; set; }
        public List<string> dieaseNamesArray { get; set; }
    }
}