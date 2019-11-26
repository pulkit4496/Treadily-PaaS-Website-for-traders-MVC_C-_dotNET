using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Runtime.Serialization;

namespace MainSite.Models
{
    [DataContract]
    public class datapoints
    {
        public datapoints(double x, float y)
        {
            this.X = x;
            this.Y = y;
        }
        [DataMember(Name = "x")]
        public double X { get; set; }
        [DataMember(Name = "y")]
        public float Y { get; set; }
    }
}