using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TaskManagment.DTO;
using TaskManagment.Models;

namespace TaskManagment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController(UserManager<AppUser> usermanager, IConfiguration _configuration) : ControllerBase
    {

        [HttpPost("register")]
        public async Task<IActionResult> Registration(RegisterDTO appUser)
        {
            if (ModelState.IsValid)
            {
                AppUser u = new AppUser();
                u.UserName = appUser.UserName;
                u.Email = appUser.Email;
              IdentityResult result= await usermanager.CreateAsync(u, appUser.Password);
                if (result.Succeeded)
                {
                    return Ok("Account Added Successfully");
                }
                return BadRequest(result.Errors.FirstOrDefault());
            }
            return BadRequest(ModelState);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO userLogin)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid login request.");
            }

            AppUser u = await usermanager.FindByEmailAsync(userLogin.Email);
            if (u == null)
            {
                return Unauthorized("Invalid email or password.");
            }

            var isValid = await usermanager.CheckPasswordAsync(u, userLogin.Password);
            if (!isValid)
            {
                return Unauthorized("Invalid email or password.");
            }

            var token = await GenerateJwtToken(userLogin);
            if (token == null)
            {
                return StatusCode(500, "Token generation failed.");
            }

            return Ok(new { Token = token });
        }

        private async Task<string> GenerateJwtToken(LoginDTO userLogin)
        {
            if (userLogin == null || string.IsNullOrEmpty(userLogin.Email))
            {
                throw new ArgumentException("Invalid login request.");
            }

            AppUser user = await usermanager.FindByEmailAsync(userLogin.Email);
            if (user == null)
            {
                throw new UnauthorizedAccessException("User not found.");
            }

            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, user.Id),
        new Claim(ClaimTypes.Name, user.UserName),
        new Claim(ClaimTypes.Email, user.Email)
    };

            var roles = await usermanager.GetRolesAsync(user);
            foreach (var item in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, item));
            }

            string jwtKey = _configuration["JwtSettings:Key"];
            if (string.IsNullOrEmpty(jwtKey))
            {
                throw new InvalidOperationException("JWT Key is missing in configuration.");
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["JwtSettings:ExpiryMinutes"])),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

      

    }
}

