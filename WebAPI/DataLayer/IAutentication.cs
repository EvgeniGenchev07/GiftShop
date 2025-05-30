using BusinessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public interface IAutentication
    {
        Task<int> Login(string email, string password);
        Task<int> Register(string email, string password, string userName);
    }
}
