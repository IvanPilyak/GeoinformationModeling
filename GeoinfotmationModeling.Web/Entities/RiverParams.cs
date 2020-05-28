using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeoinformationModeling.Web.Entities
{
    public class RiverParams
    {
        public double LengthByX { get; set; }//L
        public int SplitsNumberByX { get; set; }//NVER
        public double LengthByT { get; set; }//ZT
        public int SplitsNumberByT { get; set; }//NTER
        public int OutputMultiplicityByX { get; set; }//krat_x
        public int OutputMultiplicityByT { get; set; }//krat_t
    }
}
