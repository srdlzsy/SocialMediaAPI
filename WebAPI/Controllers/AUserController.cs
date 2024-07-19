using DTO;
using entity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Repository.EfCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace WebAPI.Controllers
{
    
        public class AUserController : ControllerBase
        {
            private readonly MyContext _context;
            private readonly UserManager<AppUser> _userManager;
            private readonly SignInManager<AppUser> _signInManager;
            private readonly IConfiguration _configuration;
            public AUserController(MyContext context, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IConfiguration configuration)
            {
                _userManager = userManager;
                _context = context;
                _signInManager = signInManager;
                _configuration = configuration;

            }
            [HttpPost("/api/register")]
            public async Task<IActionResult> Create(UserDTO model)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var user = new AppUser
                {
                    UserName = model.UserName,
                    Email = model.Email,
                   
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    return StatusCode(201);
                }
                return
                BadRequest(result.Errors);
            }
        [HttpPost("/api/Login")]
        public async Task<IActionResult> Login(LoginDTO model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                return BadRequest(new { message = "email hatalı" });
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);

            if (result.Succeeded)
            {
                return Ok(
                    new { token = GenerateJWT(user) }
                );
            }
            return Unauthorized();
        }

        private string GenerateJWT(AppUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration.GetSection("AppSettings:Secret").Value ?? "");

  
            var userClaims = new List<Claim>
        {
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Name, user.UserName ?? "")
        };
     
            // E-posta adresine göre admin rolü ekleme
            if (user.Email == "info@serdalozsoy.com")
            {
                userClaims.Add(new Claim(ClaimTypes.Role, "admin"));
            }
            // Diğer roller de eklenebilir
            // else
            // {
            //     userClaims.Add(new Claim(ClaimTypes.Role, "user"));
            // }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(userClaims),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }


    }
}

