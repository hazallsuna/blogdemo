using BlogDemoApi.Data;
using BlogDemoApi.Dto;
using BlogDemoApi.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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
        private readonly IPasswordHasher<UserEntity> _passwordHasher;

        public UsersController(ApplicationDbContext dbContext, IConfiguration configuration, IPasswordHasher<UserEntity> passwordHasher)
        {
            this.dbContext = dbContext;
            this._SecretKey = configuration.GetValue<string>("ApiSettings:Secret");
            _passwordHasher = passwordHasher;
        }

        [HttpPost("Login")]
        public async Task<LoginResponse> Login(LoginRequest logindetails)
        {
            var user = dbContext.User
               // .Include(u => u.Posts)
              // .Include(u => u.Comments)
                .FirstOrDefault(u => u.Email == logindetails.Email);

            if (user == null)
            {
                return null;
            }

            var result = _passwordHasher.VerifyHashedPassword(user, user.Password, logindetails.Password);
            if (result != PasswordVerificationResult.Success)
                return null;

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

        [HttpPost("Register")]
        public async Task<ActionResult<RegisterDto>> Register(RegisterDto registerDto)
        {
            if (registerDto.Password != registerDto.PasswordConfirmed)
                return BadRequest("Passwords do not match.");

            var existingUser = await dbContext.User.FirstOrDefaultAsync(u => u.Email == registerDto.Email);
            if (existingUser != null)
                return BadRequest("Email already exists.");

            var newUser = new UserEntity
            {
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                Email = registerDto.Email
            };

            newUser.Password = _passwordHasher.HashPassword(newUser, registerDto.Password);

            dbContext.User.Add(newUser);
            await dbContext.SaveChangesAsync();

            return Ok(newUser);
        } 
    }
}
