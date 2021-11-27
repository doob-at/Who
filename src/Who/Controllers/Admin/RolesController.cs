using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using doob.Reflectensions.Common;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Who.Auth.Entities.DTO;
using Who.Auth.Managers;
using who.Auth.Services;
using Who.Auth.Services;

namespace doob.Who.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/roles")]
    [Authorize(Policy = "Admin")]
    public class RolesController : Controller
    {
        private readonly IRolesService _rolesService;
        private readonly ILocalUserService _localUserService;
        private readonly IMapper _mapper;


        public RolesController(
            IMapper mapper, ILocalUserService localUserService, IRolesService rolesService)
        {
            _mapper = mapper;
            _localUserService = localUserService;
            _rolesService = rolesService;
        }

        [HttpGet]
        public async Task<ActionResult<List<RoleListDto>>> GetRolesList()
        {

            var roles = await _rolesService.GetAllRolesAsync();
            return Ok(roles);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RoleDto>> GetRole(string id)
        {

            if (id == "create")
            {
                return Ok(new RoleDto());
            }

            var role = await _rolesService.GetRoleAsync(id.ToGuid());

            if (role == null)
                return NotFound();


            return Ok(role);


        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(RoleDto roleDto)
        {
            await _rolesService.CreateRoleAsync(roleDto);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateRole(RoleDto roleDto)
        {

            await _rolesService.UpdateRoleAsync(roleDto);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(Guid id)
        {
            await _rolesService.DeleteRole(id);
            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteRoles([FromBody] List<Guid> ids)
        {
            await _rolesService.DeleteRole(ids.ToArray());

            return NoContent();
        }


    }
}
