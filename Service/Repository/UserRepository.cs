
using Repository.Models;
using Mapster;
using NuGet.Protocol;
using Forum.Logic.Repository;
using Microsoft.EntityFrameworkCore;
using Forum.Logic.Models;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;



namespace Forum.Persistance
{
    public class UserRepository(ForumContext forumContext, IDistributedCache cache) : IUserRepository<User>
    {
        private readonly ForumContext _forumContext = forumContext;
        private readonly IDistributedCache _cache = cache;

        public async Task Create(User user)
        {
            await _forumContext.Users.AddAsync(user);

        }

        public async Task Delete(Guid id)
        {
            var user = await _forumContext.Users.FirstOrDefaultAsync(i => i.Id == id);

            if (await _cache.GetAsync($"user-{id.ToString()}") is not null)
                await _cache.RemoveAsync($"{user}-{id.ToString()}");

            _forumContext.Users.Remove(user);
        }

        public async Task<User?> Get(Guid id)
        {
            var jsonString = await _cache.GetStringAsync($"user-{id.ToString()}");
            User? user = null;

            if (jsonString != null)
            {
                user = JsonConvert.DeserializeObject<User>(jsonString);
            }
            else
            {
                try
                {
                    user = await _forumContext.Users.FirstOrDefaultAsync(i => i.Id == id);
                }
                catch(Exception e)
                {

                }

                if (user is null)
                    return null;

                jsonString = user.ToJson();

                await cache.SetStringAsync($"user-{user.Id.ToString()}", jsonString, new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(2)
                });
            }

            return user;
        }

        public async Task Save()
        {
            await _forumContext.SaveChangesAsync();
        }

        public async Task<User> Update(User newUser)
        {
            var user = await _forumContext.Users.FirstOrDefaultAsync(i => i.Id == newUser.Id);

            var conf = TypeAdapterConfig<User, User>.NewConfig()
                .IgnoreIf((src, dest) => src.RoleId == 0, dest => dest.RoleId)
                .IgnoreNullValues(true)
                .BuildAdapter().Config;

            newUser.Adapt(user, typeof(User), typeof(User), conf);

            if (await _cache.GetAsync($"user-{user.Id.ToString()}") is not null)
                await _cache.SetStringAsync($"user-{user.Id.ToString()}", user.ToJson());

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
