using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using static System.Net.WebRequestMethods;
using Microsoft.EntityFrameworkCore;

namespace ingresa.Models
{
    public class Jwt
    {
        public string Key {  get; set; }
        public string Issuer { get; set; }
        public string Subject { get; set; }
        public string Audience { get; set; }
    }
}
