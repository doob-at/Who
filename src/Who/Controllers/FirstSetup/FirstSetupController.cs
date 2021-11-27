using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Who.Auth;
using Who.Auth.Entities;
using Who.Auth.Services;

namespace doob.Who.Controllers.FirstSetup
{
    [ApiController]
    [Route("api/first-setup")]
    [AllowAnonymous]
    public class FirstSetupController: Controller
    {

        private readonly DefaultResourcesManager _defaultResourcesManager;
        private readonly ILocalUserService _localUserService;


        public FirstSetupController(DefaultResourcesManager defaultResourcesManager,  ILocalUserService localUserService)
        {
            _defaultResourcesManager = defaultResourcesManager;
            _localUserService = localUserService;
        }


        [HttpGet]
        public async Task<IActionResult> GetSetupViewModel()
        {
            var vm = new FirstSetupViewModel();
            vm.AdminUserExists = await _defaultResourcesManager.AtLeastOneAdminUserExistsAsync();

            return Ok(vm);
        }

        [HttpPost]
        public async Task<IActionResult> CreateFirstUser([FromBody] FirstSetupModel firstSetupModel)
        {

            var exists = await _defaultResourcesManager.AtLeastOneAdminUserExistsAsync();
            if (exists)
            {
                return BadRequest("Admin User already exists!");
            }

            var adminRole = await _defaultResourcesManager.EnsureIdpAdminRoleExists();

            var user = new User();
            user.UserName = firstSetupModel.Username;
            user.Active = true;
            user.Roles.Add(adminRole);

            await _localUserService.AddUserAsync(user, firstSetupModel.Password);
           
            return Ok();
            
        }
    }

    public class FirstSetupViewModel
    {
        public bool AdminUserExists { get; set; }
    }
}
