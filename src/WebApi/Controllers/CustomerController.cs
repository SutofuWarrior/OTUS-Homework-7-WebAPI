using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models;
using WebApi.Repositories;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("customers")]
    public class CustomerController : Controller
    {
        private readonly ICustomerRepository repo;

        public CustomerController(ICustomerRepository repository)
        {
            repo = repository;
        }

        [HttpGet("{id:long}")]   
        public async Task<ActionResult> GetCustomerAsync([FromRoute] long id)
        {
            var customer = await repo.Get(id);

            if (customer != null)
                return Ok(customer);

            return NotFound();
        }

        [HttpPost]   
        public async Task<ActionResult> CreateCustomerAsync([FromBody] Customer customer)
        {
            if (customer == null)
                return BadRequest();

            var existing = repo.GetByName(customer.Firstname, customer.Lastname);

            if (existing != null)
                return new StatusCodeResult(StatusCodes.Status409Conflict);

            long id = await repo.CreateAsync(customer);

            return new ObjectResult(id) { StatusCode = StatusCodes.Status200OK };
        }
    }
}