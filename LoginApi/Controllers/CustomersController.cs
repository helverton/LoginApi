using LoginApi.Data;
using System.Security.Claims;
using LoginApi.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace LoginApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CustomersController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Customers
        [HttpGet]
        [Authorize(Policy = "all")]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomer(int page = 1, int pageSize = 10, string filter = "")
        {
            var user = _context.Users.Include(d => d.Distributor).FirstOrDefault(u => u.Id.Equals(User.FindFirstValue(ClaimTypes.NameIdentifier)));

            if (User.IsInRole("system"))
            {
                return await _context
                                .Customer.OrderBy(x => x.Codigo)
                                .Where(x => x.Codigo.Contains(filter))
                                .Skip((page - 1) * pageSize)
                                .Take(pageSize)
                                .ToListAsync();
            }
            else
            {
                string distrCode = user.Distributor.Codigo;
                return await _context
                                .Customer.OrderBy(x => x.Codigo)
                                .Where(x => x.DistribuidoraCodigo.Equals(distrCode) && x.Codigo.Contains(filter))
                                .Skip((page - 1) * pageSize)
                                .Take(pageSize)
                                .ToListAsync();
            }
        }

        // GET: api/Customers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetCustomer(long id)
        {
            var customer = await _context.Customer.FindAsync(id);

            if (customer == null)
            {
                return NotFound();
            }

            return customer;
        }

        // PUT: api/Customers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer(long id, Customer customer)
        {
            if (id != customer.Id)
            {
                return BadRequest();
            }

            _context.Entry(customer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Customers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Customer>> PostCustomer(Customer customer)
        {
            _context.Customer.Add(customer);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCustomer", new { id = customer.Id }, customer);
        }

        // DELETE: api/Customers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(long id)
        {
            var customer = await _context.Customer.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            _context.Customer.Remove(customer);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CustomerExists(long id)
        {
            return _context.Customer.Any(e => e.Id == id);
        }
    }
}
