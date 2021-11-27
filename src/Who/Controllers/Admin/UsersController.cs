using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using doob.Reflectensions.Common;
using doob.Who.Models;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Who.Auth.Entities;
using Who.Auth.Entities.DTO;
using Who.Auth.Managers;
using Who.Auth.Services;

namespace doob.Who.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/users")]
    [Authorize(Policy = "Admin")]

    public class UsersController : Controller
    {

        private readonly IMapper _mapper;
        private readonly ILocalUserService _localUserService;

        public UsersController(
            IMapper mapper, ILocalUserService localUserService)
        {
            _mapper = mapper;
            _localUserService = localUserService;
        }

        [HttpGet]
        public async Task<ActionResult<List<UserListDto>>> GetAllUsers()
        {
            var users = await _localUserService.GetAllUsersAsync();

            var dtos = users.Adapt<IEnumerable<UserListDto>>(_mapper.Config);
            return Ok(dtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUser(string id)
        {

            if (id == "create")
            {
                return Ok(new UserDto());
            }

            var user = await _localUserService.GetUserByIdAsync(id.ToGuid());

            if (user == null)
                return NotFound();

            var dto = _mapper.Map<UserDto>(user);

            //var roles = await _userManager.GetRolesWithDetailsAsync(user);
            //dto.Roles = roles.Adapt<ICollection<WhoRoleListDto>>(_mapper.Config);

            return Ok(dto);


        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserDto createUserDto)
        {
            
            var user = await _localUserService.AddUserAsync(createUserDto);
            return Ok(user.Id);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser(UserDto updateUserDto)
        {


            //var user = await _localUserService.GetUserByIdAsync(updateUserDto.Id);
            //user = _mapper.Map(updateUserDto, user);

            await _localUserService.UpdateAsync(updateUserDto);

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            await _localUserService.DeleteAsync(id);
            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUsers([FromBody] List<Guid> ids)
        {
            await _localUserService.DeleteAsync(ids.ToArray());

            return NoContent();
        }


        [HttpPost("{id}/password")]
        public async Task<IActionResult> SetPassword(Guid id, SetPasswordDto passwordDto)
        {
            var user = await _localUserService.SetPassword(id, passwordDto.Password);

            return Ok();
        }

        [HttpDelete("{id}/password")]
        public async Task<IActionResult> ClearPassword(Guid id)
        {
            await _localUserService.ClearPassword(id);
            return Ok();
        }
    }
}
