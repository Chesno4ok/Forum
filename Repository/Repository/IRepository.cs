using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forum.Logic.Repository
{
    public interface IRepository<T> : IDisposable
    {
        Task<IEnumerable<T>> GetAll();
        Task<T?> Get(Guid id);
        Task Create(T item);
        Task<T> Update(T item);
        Task Delete(Guid id);
        Task Save();
    }
}
