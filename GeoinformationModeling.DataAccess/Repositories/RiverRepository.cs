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
    public class RiverRepository : IRepository<RiverParams>
    {
        private readonly AppDbContext _appContext;
        private DbSet<RiverParams> entities;

        public RiverRepository(AppDbContext appDbContext)
        {
            _appContext = appDbContext;
        }

        public async Task Create(RiverParams item)
        {
            await _appContext.RiverParamsSet.AddAsync(item);
        }

        public async Task Delete(int id)
        {
            RiverParams category = await _appContext.RiverParamsSet.FindAsync(id);

            if (category != null)
            {
                _appContext.RiverParamsSet.Remove(category);
            }
        }

        public async Task<RiverParams> Get(int id)
        {
            RiverParams category = await _appContext.RiverParamsSet.FindAsync(id);

            return category;
        }

        public IEnumerable<RiverParams> GetAll()
        {
            IEnumerable<RiverParams> categories = _appContext.RiverParamsSet;

            return categories;
        }

        public void Update(RiverParams item)
        {
            _appContext.Entry(item).State = EntityState.Modified;
        }

        public void SaveChanges()
        {
            _appContext.SaveChanges();
        }

        public virtual IQueryable<RiverParams> Set
        {
            get
            {
                return Entities;
            }
        }



        protected virtual DbSet<RiverParams> Entities
        {
            get
            {
                if (entities == null)
                    entities = _appContext.Set<RiverParams>();
                return entities;
            }
        }
    }
}
