using BlogDemoApi.Data;
using BlogDemoApi.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BlogDemoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        public readonly ApplicationDbContext dbContext;
        private string _SecretKey;

        public UsersController(ApplicationDbContext dbContext, IConfiguration configuration)
        {
            this.dbContext = dbContext;
            this._SecretKey = configuration.GetValue<string>("ApiSettings:Secret");
        }

        [HttpPost("Login")]
        public async Task<LoginResponse> Login(LoginRequest logindetails)
        {
            var user = dbContext.User
               // .Include(u => u.Posts)
              // .Include(u => u.Comments)
                .FirstOrDefault(u => u.Email == logindetails.Email && u.Password == logindetails.Password);

            if (user == null)
            {
                return null;
            }

            //create JWT Token

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_SecretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                      new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()), 
                      new Claim(ClaimTypes.Email, user.Email)
                }),

                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token=tokenHandler.CreateToken(tokenDescriptor);
            LoginResponse loginResponse = new LoginResponse()
            {
                Token = tokenHandler.WriteToken(token),
                UserDetails = user,
            };

            return loginResponse;

        }
    }
}
