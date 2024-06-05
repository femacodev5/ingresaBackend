using ingresa.Context;
using ingresa.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ingresa.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AuthController:ControllerBase
    {
               public IConfiguration _configuration;
        public AuthController(AppDBcontext context, IConfiguration  configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        private readonly AppDBcontext _context;
         
        private string GenerateToken(string username)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            Console.WriteLine(credentials);
            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Issuer"],
                null,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }



        [HttpPost]
        [Route("AuthLogin")]
        public async Task<dynamic> LoginAuthAsync([FromBody] Object optData)
        {

            dynamic data = JsonConvert.DeserializeObject<dynamic>(optData.ToString());

            string username = data.username.ToString();
            string password = data.password.ToString();

            var user = await _context.Usuarios.Include(u => u.Persona).FirstOrDefaultAsync(u => u.NombreUsuario == username);
            
            if (user != null)
            {
                if (user.Clave == password)
                {

                    var token = GenerateToken(user.Clave);



                    return new { success = true, message = "Login successful",
                    token,
                    user,
                    };
                }
                else
                {
                    return new { success = false, message = "Invalid password" };
                }
            }
            else
            {
                return new { success = false, message = "User not found" };
            }

    
        }
    }
}
