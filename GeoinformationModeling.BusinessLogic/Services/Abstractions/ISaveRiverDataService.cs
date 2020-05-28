using GeoinformationModeling.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace GeoinformationModeling.BusinessLogic.Services.Abstractions
{
    public interface ISaveRiverDataService
    {
        void SaveRiverData(RiverParams river, List<MapParams> mapParams);
        List<RiverParams> GetAllRivers(string userId);
        RiverParams GetRiverById(int riverId);
    }
}
