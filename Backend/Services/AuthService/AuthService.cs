using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using ProjectSpace.Dtos.Auth;
using ProjectSpace.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ProjectSpace.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;


        public AuthService(UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }


        public async Task<AuthResponse> GetCurrentUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null) return null;

            return new AuthResponse
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email
            };
        }


        public async Task<LoginResponseDto> LoginAsync(LoginDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email) ?? throw new Exception("Invalid email or password.");

            // Hash the entered password and compares it against the stores PasswordHash
            var isPasswordValid = await _userManager.CheckPasswordAsync(user, dto.Password);

            if (!isPasswordValid)
            {
                throw new Exception("Invalid email or password.");
            }

            var token = GenerateJwtToken(user);

            return new LoginResponseDto
            {
                Token = token,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName
            };
        }


        public async Task<AuthResponse> RegisterAsync(RegisterDto dto)
        {
            var userExists = await _userManager.FindByEmailAsync(dto.Email);

            if (userExists != null)
            {
                throw new Exception($"{dto.Email} is already registered.");
            }

            var user = new ApplicationUser
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                UserName = dto.Email
            };

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
            {
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
            }

            return new AuthResponse
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email
            };
        }

        private string GenerateJwtToken(ApplicationUser user)
        {
            var jwtSettings = _configuration.GetSection("Jwt");

            //Claims: Pieces of user information stored inside the token
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(ClaimTypes.GivenName, user.FirstName),
                new Claim(ClaimTypes.Surname, user.LastName),
            };

            //Convert secret key into a format that JWT signing can use
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]!));

            //use 'key' and HmacSha256 algorithm to sign the key
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expires = DateTime.UtcNow.AddMinutes(
                Convert.ToDouble(jwtSettings["ExpiryMinutes"]));

            //Token creation
            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: expires,
                signingCredentials: credentials
                );

            //Convert token object into the long JWT string that gets sent to the FE
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
