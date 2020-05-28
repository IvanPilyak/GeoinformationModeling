using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoinformationModeling.DataAccess.Repositories.Abstractions
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();

        Task<T> Get(int id);

        Task Create(T item);

        void Update(T item);

        Task Delete(int id);

        void SaveChanges();
        IQueryable<T> Set { get; }

    }
}
