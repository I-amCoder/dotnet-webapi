using api_with_auth.Data;
using api_with_auth.Models;
using api_with_auth.Models.Dto;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace api_with_auth.Controllers.Authorization
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private string secretKey;

        public AuthorizationController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, AppDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _signInManager = signInManager;
            secretKey = configuration.GetValue<string>("ApiSettings:SecretKey");
        }

        [HttpPost("Login")]
        public async Task<ActionResult<LoginResponseDto>> Login(LoginRequestDto request)
        {
            var user = await _dbContext.ApplicationUsers.FirstOrDefaultAsync(u => u.UserName.ToLower() == request.Username.ToLower());
            bool isValid = await _userManager.CheckPasswordAsync(user, request.Password);

            if (user == null || ! isValid) 
            {
                return BadRequest("Invalid Crendetials");
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new System.Security.Claims.ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.UserName.ToString()),
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            LoginResponseDto response = new LoginResponseDto
            {
                Username = user.UserName,
                Token = tokenHandler.WriteToken(token)
            };

            return Ok(response);
        }


        [HttpPost("Register")]
        public async Task<ActionResult<LoginRequestDto>> Register(RegisterRequestDto request)
        {
            // Validate Username and Email Unqiue
            var unique_username = await _dbContext.ApplicationUsers
                .Where(u => u.UserName.ToLower() == request.UserName.ToLower())
                .Where(u => u.Email.ToLower() == request.Email.ToLower())
                .FirstOrDefaultAsync();
           
            if (unique_username != null) {
                ModelState.AddModelError("Username", "Username or Email Has Already Been Taken");
            }

            ApplicationUser user = new()
            {
                UserName = request.UserName,
                Name = request.Name,
                Email = request.Email,
                NormalizedEmail = request.Email.ToUpper(),

            };

            try
            {


                var result = await _userManager.CreateAsync(user, request.Password);
                if (result.Succeeded)
                {
                    return Ok("User regsitered, you can now login");
                }
                return BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.ToString());

            }

        }
        
    }
}
