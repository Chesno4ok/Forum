using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forum.Application.Auth
{
    public struct HashedPassword(string hash, byte[] salt)
    {
        public string Hash { get; set; } = hash;
        public byte[] Salt { get; set; } = salt;
    }
}
