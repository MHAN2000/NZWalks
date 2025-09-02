using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly ITokenRepository tokenRepository;

        public AuthController(UserManager<IdentityUser> userManager, ITokenRepository tokenRepository)
        {
            this.userManager = userManager;
            this.tokenRepository = tokenRepository;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
        {
            IdentityUser identityUser = new IdentityUser
            {
                UserName = request.Username,
                Email = request.Username
            };

            IdentityResult identityResult = await userManager.CreateAsync(identityUser, request.Password);

            if (identityResult.Succeeded)
            {
                // Add roles to this User
                if (request.Roles != null && request.Roles.Any())
                    identityResult = await userManager.AddToRolesAsync(identityUser, request.Roles);

                if (identityResult.Succeeded)
                    return Ok("User was registered successfully");
            }

            return BadRequest("Something went wrong");
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            IdentityUser? user = await userManager.FindByEmailAsync(request.Username);
            if (user == null)
                return NotFound("The user with the given email was not found");

            bool loggedIn = await userManager.CheckPasswordAsync(user, request.Password);

            if (!loggedIn) return BadRequest("Wrong password, try again");

            // Get roles
            IList<string> roles = await userManager.GetRolesAsync(user);
            if (roles != null)
            {
                // Create token
                var jwtToken = tokenRepository.CreateJWTToken(user, roles.ToList());
                LoginResponseDto response = new LoginResponseDto { JwtToken = jwtToken };
                return Ok(response);
            }

            return BadRequest("Roles not found, make sure the User has a role(s)");
        }
    }
}
