using GeoinformationModeling.BusinessLogic.Services.Abstractions;
using GeoinformationModeling.DataAccess.Entities;
using GeoinformationModeling.DataAccess.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeoinformationModeling.BusinessLogic.Services
{
    public class SaveRiverDataService : ISaveRiverDataService
    {
        public IRepository<RiverParams> riverRepository;
        public IRepository<MapParams> mapRepository;
        public SaveRiverDataService(IRepository<RiverParams> riverRepository, IRepository<MapParams> mapRepository)
        {
            this.riverRepository = riverRepository;
            this.mapRepository = mapRepository;
        }
        public void SaveRiverData(RiverParams river, List<MapParams> mapParams)
        {
            mapParams.ForEach(t => t.RiverParamsId = river.Id);
            if (river.Id == 0)
            {

                river.MapParamsList = mapParams;
                riverRepository.Create(river);
            }
            else {

                var deleteMaps = mapRepository.Set.Where(t => t.RiverParamsId == river.Id).ToList();
                foreach(var map in deleteMaps)
                {
                    mapRepository.Delete(map.Id);
                }
                foreach(var createMap in mapParams)
                {
                    mapRepository.Create(createMap);
                }


                riverRepository.Update(river);
            }

            riverRepository.SaveChanges();
        }

        public List<RiverParams> GetAllRivers(string userId)
        {
            return riverRepository.Set.Include(t => t.MapParamsList).Where(m => m.UserId == userId).ToList();
        }

        public RiverParams GetRiverById(int riverId)
        {
            return riverRepository.Set.Include(t => t.MapParamsList).Where(m => m.Id == riverId).FirstOrDefaultAsync().Result;
        }
    }
}
