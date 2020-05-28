using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeoinformationModeling.Web.Models
{
    public class MatrixesModel
    {
        public double[,] MatrixH { get; set; }
        public double[,] MatrixU { get; set; }

        public List<MapDetail> MapDetails { get; set; }
    }
}
