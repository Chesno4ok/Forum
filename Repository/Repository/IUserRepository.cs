
using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forum.Logic.Repository
{
    public interface IUserRepository<T> : IRepository<T> where T : User
    {
        Task<T?> Get(string login, string passwordHash);
        Task<T?> Get(string login);
    }
}
