using GeoinformationModeling.Web.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeoinformationModeling.Web.Models
{
    public class GeoinformationModelingViewModel
    {
        public RiverParams PiverParams { get; set; }
        public TaskParams TaskParams { get; set; }
        public List<double> Lengthes { get; set; }
        public TestExample TestExample { get; set; }

        public List<MapPointModel> MapPoints {get;set;}

        public string UserId { get; set; }
        public int RiverId { get; set; }
        public string RiverName { get; set; }
    }
}
