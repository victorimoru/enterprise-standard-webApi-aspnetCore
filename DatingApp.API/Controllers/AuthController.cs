using DatingApp.Core.DTOs;
using DatingApp.Core.ServiceContracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Infrastructure.Entities;

namespace DatingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService authService;

        public AuthController(IAuthService authService)
        {
            this.authService = authService;
        }
        // GET: api/Auth/register
        [HttpPost("register")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Consumes("application/json")]
        public IActionResult Register(RegisterUserRquestDto registerUserRquestDto)
        {
            var usernameInSmallCaps = registerUserRquestDto.Username.ToLower();
            if (authService.DoesUserExist(usernameInSmallCaps))

                return BadRequest(new { message = "Username already Exists." });

            var userToCreate = new User
            {
                Username = usernameInSmallCaps
            };

            var createdUser = authService.RegisterAsync(userToCreate, registerUserRquestDto.Password);
            return StatusCode(201);
        }

        [HttpPost("login")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Consumes("application/json")]
        public async System.Threading.Tasks.Task<IActionResult> LoginAsync(UserLoginRquestDto userLoginRquestDto)
        {
            var checkUsername = authService.DoesUserExist(userLoginRquestDto.Username.ToLower());
            if (!checkUsername)
                return BadRequest(new { message = "User does not exist." });

            var authResponse = await authService.LoginAsync(userLoginRquestDto.Username.ToLower(), userLoginRquestDto.Password);

            if (authResponse.token == null && authResponse.refreshToken == null)
                return Unauthorized(new { message = "Incorrect Password" });

            return Ok(new AuthenticationResponse
            {
                Token = authResponse.token,
                RefreshToken = authResponse.refreshToken
            });



        }

        [HttpPost("refresh")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Consumes("application/json")]
        public async System.Threading.Tasks.Task<IActionResult> RefreshTokenAsync(RefreshTokenRequestDto requestDto)
        {
            var result = await authService.RefreshTokenAsync(requestDto.Token, requestDto.RefreshToken);
            if (!string.IsNullOrEmpty(result.ErrorMsg)) return BadRequest(new { message = result.ErrorMsg });
            return Ok(new AuthenticationResponse
            {
                Token = result.Token,
                RefreshToken = result.RefreshToken
            });
        }

    }
}