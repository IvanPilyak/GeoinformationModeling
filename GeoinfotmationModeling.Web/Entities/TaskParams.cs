using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeoinformationModeling.Web.Entities
{
    public class TaskParams
    {
        public double GravityAcceleration { get; set; }
        public double FreeSurfaceWidth { get; set; }
        public double ChannelHydraulicRadius { get; set; }
        public double ShaziCoefficient { get; set; }
        public double AngleOfInclinationSine { get; set; }
        public double AngleSine { get; set; }
        public double Alpha { get; set; }
    }
}
