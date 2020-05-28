using System;
using System.Collections.Generic;
using System.Text;

namespace GeoinformationModeling.DataAccess.Entities
{
    public class MapParams
    {
        public int Id { get; set; }
        public double PointX { get; set; }
        public double PointY { get; set; }
        public double SequenceNumber { get; set; }
        public int RiverParamsId { get; set; }
        public RiverParams RiverParams { get; set; }
    }
}
