using Common.Core.Domain;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WebApi.Configuration.Settings;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("/api/auth")]
    public sealed class AuthController : ControllerBase
    {
        private readonly DataContext _context;
        //private readonly SignInManager<Customer> _signInManager;
        private readonly IOptions<JwtSettings> _jwtSettings;
        private readonly IConfiguration _configuration;

        public AuthController(DataContext context, /*SignInManager<Customer> signInManager*/ IOptions<JwtSettings> jwtSettings, IConfiguration configuration)
        {
            _context = context;
            //_signInManager = signInManager;
            _jwtSettings = jwtSettings;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request,
            CancellationToken cancellationToken = default)
        {
            //var user = await _signInManager.UserManager.FindByEmailAsync(request.Email);
            
        }
    }
}
