using Common.Core.Contracts.Auth;
using Common.Core.Domain;
using Infrastructure.Data;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApi.Configuration.Settings;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("/api/auth")]
    [EnableCors("FactoryPolicy")]
    [Produces("application/json")]
    public sealed class AuthController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly SignInManager<Customer> _signInManager;
        private readonly IOptions<JwtSettings> _jwtSettings;
        private readonly IConfiguration _configuration;

        public AuthController(DataContext context, SignInManager<Customer> signInManager,IOptions<JwtSettings> jwtSettings, IConfiguration configuration)
        {
            _context = context;
            _signInManager = signInManager;
            _jwtSettings = jwtSettings;
            _configuration = configuration;
        }

        /// <summary>
        /// Logs in a user with the provided credentials.
        /// </summary>
        /// <param name="request">The login request containing email and password.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Returns a token if login is successful; otherwise, returns appropriate status codes.</returns>
        /// <response code="200">Returns the authentication token.</response>
        /// <response code="204">No content if the user is not found.</response>
        /// <response code="403">Forbidden if the password is incorrect.</response>
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(403)]
        [HttpPost("LogIn")]
        public async Task<IActionResult> Login([FromBody] LoginDto request,
            CancellationToken cancellationToken = default)
        {
            var user = await _signInManager.UserManager.FindByEmailAsync(request.Email);
            if (user is null)
                return NoContent();

            var isThePasswordValid = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
            if (!isThePasswordValid.Succeeded)
                return Forbid();

            var token = await GenerateToken(user);

            return Ok(token);
        }

        /// <summary>
        /// Creates a new user account with the provided details.
        /// </summary>
        /// <param name="request">The account creation request containing user details.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Returns an access token upon successful account creation.</returns>
        /// <response code="201">Returns the authentication token if the account is created successfully.</response>
        /// <response code="400">Bad request if there is an error while creating the user.</response>
        [HttpPost("SignUp")]
        public async Task<IActionResult> CreateAccount([FromBody] CreateAccountDto request,
            CancellationToken cancellationToken = default)
        { 
            var user = new Customer
            {
                Email = request.Email,
                UserName = request.Email.ToLowerInvariant().Trim(),
                FirstName = request.FirstName,
                LastName = request.LastName,
                TwoFactorEnabled = false,
            };

            var result = await _signInManager.UserManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
                return BadRequest("Error while creating users");

            // Add logic to send email confirmation in the future

            var accessToken = await GenerateToken(user);

            return StatusCode(201, accessToken);

        }

            private async Task<TokenResponseDto> GenerateToken(Customer user)
        {
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var claims = new List<Claim> 
            {
                new Claim(JwtRegisteredClaimNames.NameId,user.Id),
                new Claim(JwtRegisteredClaimNames.UniqueName,user.UserName),
            };

            var key = Encoding.UTF8.GetBytes(_jwtSettings.Value.Key);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Audience = _jwtSettings.Value.Audience,
                Issuer = _jwtSettings.Value.Issuer,
                Expires = DateTime.UtcNow.AddMinutes(15),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };
            var accessToken = jwtSecurityTokenHandler.CreateToken(tokenDescriptor);

            var token = jwtSecurityTokenHandler.WriteToken(accessToken);
            var refreshToken = CryptoHelpers.GenerateRandomString(20);

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

            await _signInManager.UserManager.UpdateAsync(user);

            return new TokenResponseDto
            {
                AccessToken = token ?? string.Empty,
                RefreshToken = refreshToken
            };
        }
    }
    
}
