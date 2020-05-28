using GeoinformationModeling.DataAccess.DbContext;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GeoinformationModeling.DataAccess.Repositories
{
    public class UnitOfWork : IDisposable
    {
        private readonly AppDbContext _applicationContext;


        private bool _disposed = false;

        public UnitOfWork(AppDbContext appDbContext)
        {
            _applicationContext = appDbContext;
        }

      

        public async Task SaveAsync()
        {
            await _applicationContext.SaveChangesAsync();
        }

        public virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _applicationContext.Dispose();
                }
                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public AppDbContext GetContext()
        {
            return _applicationContext;
        }
    }
}
