using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interface
{
    public interface IRepository<T>
    {
        public Task<T> AddAsync(T entity);
        public Task<T?> GetByIdAsync(Guid id);
        public Task<List<T>?> GetAllAsync();
        public Task<T> UpdateAsync(T entity);
        public Task<T> DeleteAsync(T entity);

    }
}
