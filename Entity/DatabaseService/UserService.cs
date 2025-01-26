using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository;
using Repository.Models;
using Mapster;
using System.Security.Cryptography;
using Forum.Logic.Repository;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using NuGet.Protocol;

namespace Forum.Application.DatabaseService
{
    public class UserService(IUserRepository<User> userRpository) : IDisposable
    {
        private readonly IUserRepository<User> _userRepository = userRpository;

        public void Dispose()
        {
            _userRepository.Dispose();
        }

        public async Task<bool> IsLoginAvailable(string login)
        {
            return !_userRepository.GetAll().Result.Any(i => i.Login == login);
        }

        public async Task<User> RegisterUser(User user)
        {
            await _userRepository.Create(user);
            await _userRepository.Save();

            return user;
        }
        public async Task<string?> LoginUser(string login, string password)
        {
            var user = await _userRepository.Get(login);
            var checkedUser = await _userRepository.Get(login, Auth.AuthService.HashPassword(password, user.Salt));

            if (checkedUser is null)
                return null;

            var jwt = Auth.AuthService.CreateToken(user.Id);
            
            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        public async Task<User> UpdateUser(User user)
        {
            var newUser = await _userRepository.Update(user);
            await _userRepository.Save();

            return newUser;
        }

        public async Task DeleteUser(Guid guid)
        {
            await _userRepository.Delete(guid);
            await _userRepository.Save();
        }

        public async Task<User?> GetUser(Guid guid)
        {


            var user = await _userRepository.Get(guid);

            

            return user;
        }

        public async Task<User?> GetUser(string login)
        {
            return await _userRepository.Get(login);
        }

    }
}
