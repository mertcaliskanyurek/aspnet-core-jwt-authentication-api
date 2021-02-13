using System;
using BaseApi.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using System.Threading.Tasks;
using BaseApi.Domain.Response;
using BaseApi.Resources;
using BaseApi.Domain.Entity.Model;
using BaseApi.Extensions;
using BaseApi.Security;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace BaseApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IGenericService<User> _userService;
        private readonly IMapper _mapper;

        public UserController(IGenericService<User> userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        /// <summary>
        /// Gets the current user.
        /// </summary>
        /// <returns>HTTP OK response with new <see cref="User"/> object
        ///          Otherwise HTTP BadRequest with string message</returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            long id = GetCurrentUserId();
            //find by id
            ObjectResponse<User> response = await _userService.FindFirstOrDefault(u=>u.ID == id);
            if (response.Success) return Ok(response.Object);
            return BadRequest(response.Message);
        }

        /// <summary>
        /// Adds new user.
        /// </summary>
        /// <param name="resource">Resource of the user. <see cref="UserResource"/></param>
        /// <returns>HTTP OK response with new <see cref="User"/> object
        ///          Otherwise HTTP BadRequest with string message</returns>
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Add(UserResource resource)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.GetErrorMessages());

            resource.PassWord = PasswordUtils.CreateMD5(resource.PassWord);
            User user = _mapper.Map<UserResource, User>(resource);
            ObjectResponse<User> response = await _userService.AddAsync(user);

            if (response.Success) return Ok(response.Object);
            return BadRequest(response.Message);
        }

        /// <summary>
        /// Updates information about the logged user.
        /// </summary>
        /// <param name="resource">Resource of the new data.</param>
        ///  <returns>HTTP OK response with new <see cref="User"/> object
        ///          Otherwise HTTP BadRequest with string message</returns>
        [HttpPut]
        public async Task<IActionResult> Update(UserResource resource)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.GetErrorMessages());

            long id = GetCurrentUserId();
            resource.PassWord = PasswordUtils.CreateMD5(resource.PassWord);
            
            User user = _mapper.Map<UserResource, User>(resource);
            user.ID = id;
            ObjectResponse<User> response = await _userService.UpdateAsync(user);

            if (response.Success) return Ok(response.Object);
            return BadRequest(response.Message);
        }

        /// <summary>
        /// Deletes logged user.
        /// </summary>
        ///  <returns>HTTP OK response with new <see cref="User"/> object with only id of deleted user
        ///          Otherwise HTTP BadRequest with string message</returns>
        [HttpDelete]
        public async Task<IActionResult> Delete()
        {
            long id = GetCurrentUserId();
            ObjectResponse<User> response = await _userService.DeleteAsync(new User() { ID = id });

            if (response.Success) return Ok(response.Object);
            return BadRequest(response.Message);
        }

        /// <summary>
        /// Returns Id of the logged user.
        /// </summary>
        /// <returns>Id of the logged user -1 otherwise.</returns>
        private long GetCurrentUserId()
        {
            string userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdString == null) return -1;
            return long.Parse(userIdString);
        }
    }
}
