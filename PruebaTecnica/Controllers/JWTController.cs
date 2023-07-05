using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PruebaTecnica.Interfaces;
using PruebaTecnica.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace PruebaTecnica.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JWTController : ControllerBase
    {
        private readonly IConfigurationRoot Configuration;
        public JWTController()
        {

            var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json");

            Configuration = configurationBuilder.Build();

        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public ActionResult Login(LoginUser userLogin)
        {
            var user = Authenticate(userLogin);
            if (user == null)
            {
                return NotFound("user not found");
            }
            var token = GenerateToken(user);
            return Ok(token);

        }

        // To generate token
        private string GenerateToken(User user)
        {
            var modelsContext = new ModelsContext(Configuration);

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,user.Email),
                new Claim(ClaimTypes.Role,modelsContext.Role.FirstOrDefault(x => x.Id == user.RoleID).Name)
            };
            var token = new JwtSecurityToken(Configuration["JWT:Issuer"],
                Configuration["JWT:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);


            return new JwtSecurityTokenHandler().WriteToken(token);

        }

        //To authenticate user
        private User Authenticate(LoginUser userLogin)
        {
            var modelsContext = new ModelsContext(Configuration);
            var currentUser = modelsContext.Users.FirstOrDefault(
                x => x.Email.ToLower() == userLogin.email.ToLower() && x.Password == userLogin.password
                );
            if (currentUser != null)
            {
                return currentUser;
            }
            return null;
        }
    }
}
