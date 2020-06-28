using DatingApp.Core.DTOs;
using DatingApp.Core.Filters;
using DatingApp.Core.ServiceContracts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Shared.Infrastructure.LoggingHandler;
using Shared.Infrastructure.PagingHelper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserDetailsDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<IActionResult> GetAsync(int id)
        {
         
            var userDetails = await userService.GetUserDetailsAsync(id);
            if (userDetails == null)
                return BadRequest(new { message = "Error: User does not exist " });
            return Ok(userDetails);

        }

        
        [AllowAnonymous]
        [HttpGet("all")]
        [Produces("application/json", "application/xml")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<UserListDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<IActionResult> GetAsync([FromQuery] bool exclude = false)
        {
           var response = HttpContext.ValidateUserWithJWTClaim();
            if (response.errorMsg != null) return Unauthorized(new { message = "Invalid Token" });
            var users = await userService.GetAllUsersAsync();
            if (users == null)
                return BadRequest(new { message = "No user" });
            HttpContext.Response.Headers.Add("X-Total-Count", users.Count().ToString());
            return Ok(users);

        }


        [HttpGet("some")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<UserListDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Consumes("application/json")]
        public async Task<IActionResult> GetAsync([FromQuery] UserQueryParameters userQueryParameters)
        {
            var response = HttpContext.ValidateUserWithJWTClaim();
            if (response.errorMsg != null) return Unauthorized(new { message = "Invalid Token" });
            userQueryParameters.Id = int.Parse(response.id);
            userQueryParameters.loggedInUserGender = response.Gender;
            (IEnumerable<UserListDto> userLists, PagingMetadata data) result = await userService.GetAllUsersAsync(userQueryParameters);
            if (result.userLists == null)
                return BadRequest(new { message = "No user" });

            HttpContext.Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(result.data));
            HttpContext.Response.Headers.Add("X-Total-Count", result.data.TotalCount.ToString());

            return Ok(new
            {
                data = result.userLists,
                CurrentPage = result.data.CurrentPage,
                TotalPages = result.data.TotalPages,
                PageSize = result.data.PageSize,
                TotalCount = result.data.TotalCount,
                HasPrevious = result.data.HasPrevious,
                HasNext = result.data.HasNext
            });

          //  return Ok(result.userLists);

        }

        [HttpPost("update/{id:int}")]
        public async Task<IActionResult> UpdateUserAsync(int id, [FromBody]UserForUpdateDto userForUpdateDto)
        {
            var userInDB = await userService.GetUserByIDAsync(id, false);
            if(userInDB == null) return BadRequest(new { message = "Error: User does not exist " });
            var result = await userService.UpdateUserAsync(userForUpdateDto, userInDB);
            if(result.transactionStatus == false)
            {
                ModelState.AddModelError("UpdateError", "Updating user {id} failed on save");
                return BadRequest(ModelState);
            }
            else
            {
                return NoContent();
            }
        }

        [HttpPost("{id}/like/{recipientId}")]
        public async Task<IActionResult> LikeUser(int id, int recipientId)
        {
            var response = HttpContext.ValidateUserWithJWTClaim();
            if (response.errorMsg != null) return Unauthorized(new { message = "Invalid Token" });

            var checkIfLikeAlreadtExist = await userService.GetLike(id, recipientId);
            if (checkIfLikeAlreadtExist != null) return BadRequest(new { message = "You already liked the user." });

            var recipientUserInDB = await userService.GetUserByIDAsync(recipientId, false);
            if (recipientUserInDB == null) return NotFound();

            var likeResponse = await userService.LikeUserAsync(id, recipientUserInDB.Id);
            if(likeResponse == null) return BadRequest(new { message = "Error occured while creating like" });

            return Ok(new { ResponseCode = "00", ResponseDescription = "Like created Successfully" });
        }
    }
}