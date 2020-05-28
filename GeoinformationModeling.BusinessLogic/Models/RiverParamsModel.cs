using System;
using System.Collections.Generic;
using System.Text;

namespace GeoinformationModeling.BusinessLogic.Models
{
    public class RiverParamsModel
    {
        public double LengthByX { get; set; }
        public int SplitsNumberByX { get; set; }
        public double LengthByT { get; set; }
        public int SplitsNumberByT { get; set; }
        public int OutputMultiplicityByX { get; set; }
        public int OutputMultiplicityByT { get; set; }
        public double GravityAcceleration { get; set; }
        public double FreeSurfaceWidth { get; set; }
        public double ChannelHydraulicRadius { get; set; }
        public double ShaziCoefficient { get; set; }
        public double AngleOfInclinationSine { get; set; }
        public double AngleSine { get; set; }
        public double Alpha { get; set; }
    }
}
