
using Repository.Models;
using Mapster;
using NuGet.Protocol;
using Forum.Logic.Repository;
using Microsoft.EntityFrameworkCore;



namespace Forum.Persistance
{
    public class UserRepository : IUserRepository<User>
    {
        private ForumContext _forumContext = new ForumContext();
        public async Task Create(User user)
        {
            await _forumContext.Users.AddAsync(user);

        }

        public async Task Delete(Guid id)
        {
            var user = await _forumContext.Users.FirstOrDefaultAsync(i => i.Id == id);

            if (user is null)
                throw new ArgumentException("User doesn't exist");

            _forumContext.Users.Remove(user);
        }

        public async Task<User?> Get(Guid id)
        {
            var user = await _forumContext.Users.FirstOrDefaultAsync(i => i.Id == id);


            return user;
        }

        public async Task Save()
        {
            await _forumContext.SaveChangesAsync();
        }

        public async Task<User> Update(User newUser)
        {
            var user = await _forumContext.Users.FirstOrDefaultAsync(i => i.Id == newUser.Id);

            if (user is null)
                throw new ArgumentException("User doesn't exist");

            var conf = TypeAdapterConfig<User, User>.NewConfig()
                .IgnoreIf((src, dest) => src.RoleId == 0, dest => dest.RoleId)
                .IgnoreNullValues(true)
                .BuildAdapter().Config;

            newUser.Adapt(user, typeof(User), typeof(User), conf);

            return user;
        }

        public void Dispose()
        {
            _forumContext.Dispose();
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            return _forumContext.Users;
        }


        public async Task<User?> Get(string login)
        {
            return await _forumContext.Users.FirstOrDefaultAsync(i => i.Login == login);
        }

        public async Task<User?> Get(string login, string passwordHash)
        {
            return await _forumContext.Users.FirstOrDefaultAsync(i => i.Login == login && i.PasswordHash == passwordHash);
        }
    }
}
