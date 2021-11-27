using System;
using System.Linq;
using System.Threading.Tasks;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Who.Auth.Entities;
using Who.Auth.Entities.DTO;
using Who.Auth.Stores;

namespace doob.Who.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/clients")]
    [Authorize(Policy = "Admin")]
    public class ClientsController: Controller
    {
        
        private readonly ClientsStore _clientsStore;
        private readonly IMapper _mapper;

        public ClientsController(ClientsStore clientsStore, IMapper mapper)
        {
            _clientsStore = clientsStore;
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<IActionResult> GetClients(int? count, int? offset)
        {
            var applications = await _clientsStore.ListAsync(count, offset, HttpContext.RequestAborted).ToListAsync();
            return Ok(applications);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetClient(string id)
        {
            if (id == "create")
            {
                return Ok(new ClientDto());
            }

            
            var client = await _clientsStore.FindByIdAsync(id, HttpContext.RequestAborted);
            if (client == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<ClientDto>(client));
        }

        [HttpPost]
        public async Task<IActionResult> CreateClient(CreateClientDto clientDto)
        {
           
            await _clientsStore.CreateAsync(clientDto, HttpContext.RequestAborted);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateClient(ClientDto clientDto)
        {
            

            await _clientsStore.UpdateAsync(clientDto, HttpContext.RequestAborted);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(Guid id)
        {
            var clientInDB = await _clientsStore.FindByIdAsync(id.ToString(), HttpContext.RequestAborted);

            await _clientsStore.DeleteAsync(clientInDB, HttpContext.RequestAborted);
            return NoContent();
        }

        
    }
}
