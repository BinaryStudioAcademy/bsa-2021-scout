using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ISecurityService
    {
        string HashPassword(string password, byte[] salt);
        bool ValidatePassword(string password, string hash, string salt);
        byte[] GetRandomBytes(int length = 32);
    }
}
