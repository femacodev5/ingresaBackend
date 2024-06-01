using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ingresa.Context;
using Microsoft.EntityFrameworkCore;

namespace ingresa.Services
{
    public class AuthService
    {
        private readonly AppDBcontext _context;

        public AuthService(AppDBcontext context)
        {
            _context = context;
        }

        public async Task<dynamic> ValidarTokenAsync(ClaimsIdentity identity)
        {
            try
            {
                if (identity.Claims.Count() == 0)
                {
                    return new
                    {
                        success = false,
                        message = "Verificar token"
                    };
                }

                var id = identity.Claims.FirstOrDefault(x => x.Type == "id")?.Value;
                if (id == null)
                {
                    return new
                    {
                        success = false,
                        message = "Token does not contain user id"
                    };
                }

                var user = await _context.Users.Include(u => u.Person).FirstOrDefaultAsync(u => u.UserId.ToString() == id);
                if (user == null)
                {
                    return new
                    {
                        success = false,
                        message = "User not found"
                    };
                }

                return new
                {
                    success = true,
                    message = "Token valid",
                    user = new
                    {
                        user.UserId,
                        user.UserName,
                        user.Role,
                        user.Person.FirstName,
                        user.Person.LastName,
                        user.Person.Email,
                        user.Person.DocumentNumber
                    }
                };
            }
            catch (Exception ex)
            {
                return new
                {
                    success = false,
                    message = "Catch: " + ex.Message,
                    result = ""
                };
            }
        }
    }
}
