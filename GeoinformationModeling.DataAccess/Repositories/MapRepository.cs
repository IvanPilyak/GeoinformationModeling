using GeoinformationModeling.DataAccess.DbContext;
using GeoinformationModeling.DataAccess.Entities;
using GeoinformationModeling.DataAccess.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoinformationModeling.DataAccess.Repositories
{
    public class MapRepository : IRepository<MapParams>
    {
        private readonly AppDbContext _appContext;
        private DbSet<MapParams> entities;

        public MapRepository(AppDbContext appDbContext)
        {
            _appContext = appDbContext;
        }

        public async Task Create(MapParams item)
        {
            await _appContext.MapParams.AddAsync(item);
        }

        public async Task Delete(int id)
        {
            MapParams category = await _appContext.MapParams.FindAsync(id);

            if (category != null)
            {
                _appContext.MapParams.Remove(category);
            }
        }

        public async Task<MapParams> Get(int id)
        {
            MapParams category = await _appContext.MapParams.FindAsync(id);

            return category;
        }

        public IEnumerable<MapParams> GetAll()
        {
            IEnumerable<MapParams> categories = _appContext.MapParams;

            return categories;
        }

        public void Update(MapParams item)
        {
            _appContext.Entry(item).State = EntityState.Modified;
        }

        public void SaveChanges()
        {
            _appContext.SaveChanges();
        }

        public virtual IQueryable<MapParams> Set
        {
            get
            {
                return Entities;
            }
        }



        protected virtual DbSet<MapParams> Entities
        {
            get
            {
                if (entities == null)
                    entities = _appContext.Set<MapParams>();
                return entities;
            }
        }
    }
}
