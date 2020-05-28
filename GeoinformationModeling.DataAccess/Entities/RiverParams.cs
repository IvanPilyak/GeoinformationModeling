using System;
using System.Collections.Generic;
using System.Text;

namespace GeoinformationModeling.DataAccess.Entities
{
    public class RiverParams
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public double LengthByX { get; set; }//L
        public int SplitsNumberByX { get; set; }//NVER
        public double LengthByT { get; set; }//ZT
        public int SplitsNumberByT { get; set; }//NTER
        public int OutputMultiplicityByX { get; set; }//krat_x
        public int OutputMultiplicityByT { get; set; }//krat_t
        public double GravityAcceleration { get; set; }
        public double FreeSurfaceWidth { get; set; }
        public double ChannelHydraulicRadius { get; set; }
        public double ShaziCoefficient { get; set; }
        public double AngleOfInclinationSine { get; set; }
        public double AngleSine { get; set; }
        public double Alpha { get; set; }

        public string RiverName { get; set; }

        public virtual IEnumerable<MapParams> MapParamsList { get; set; }
    }
}
