using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeoinformationModeling.Web.Models
{
    public class MapDetail
    {
        public double Sppeed { get; set; }
        public double Area { get; set; }

        public double[,] MatrixH { get; set; }
        public double[,] MatrixU { get; set; }
    }
}
