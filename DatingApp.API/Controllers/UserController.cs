using DatingApp.Core.DTOs;
using DatingApp.Core.Filters;
using DatingApp.Core.ServiceContracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Shared.Infrastructure.LoggingHandler;
using Shared.Infrastructure.PagingHelper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DatingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [CheckPermission]
    public class UsersController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly ILoggerManager logger;

        public UsersController(IUserService userService, ILoggerManager logger)
        {
            this.userService = userService;
            this.logger = logger;
        }


        [HttpGet("detail/{id:int}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type =typeof(UserDetailsDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        [Consumes("application/json")]
        public async Task<IActionResult> GetAsync(int id)
        {
            var userDetails = await userService.GetUserDetailsAsync(id);
            if (userDetails == null)
                return BadRequest(new { message = "Error: User does not exist " });
            return Ok(userDetails);

        }
   
        [HttpGet("all")]
        [Produces("application/json", "application/xml")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<UserListDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        
        public async Task<IActionResult> GetAsync()
        {
            var users = await  userService.GetAllUsersAsync();
            if (users == null)
                return BadRequest(new { message = "No user" });
            return Ok(users);

        }


        [HttpGet("some")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<UserListDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Consumes("application/json")]
        public async Task<IActionResult> GetAsync([FromQuery] UserQueryParameters userQueryParameters)
        {
            (IEnumerable<UserListDto> userLists, PagingMetadata data) result = await userService.GetAllUsersAsync(userQueryParameters);
            if (result.userLists == null)
                return BadRequest(new { message = "No user" });
            HttpContext.Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(result.data));
            return Ok(result.userLists);

        }
    }
}