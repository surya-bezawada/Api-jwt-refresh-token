using jsontoken.Context;
using jsontoken.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace jsontoken.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class RegisterController : ControllerBase
    {
        private readonly ApplicationContext _datacontext;


        public RegisterController(ApplicationContext datacontext)
        {
            _datacontext = datacontext;

        }




        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetEmployee()
        {
            return await _datacontext.users.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetEmployee(int id)
        {
            var employee = await _datacontext.users.FindAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            return employee;
        }

       
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployee(int id, User employee)
        {
            if (id != employee.UserId)
            {
                return BadRequest();
            }

            _datacontext.Entry(employee).State = EntityState.Modified;

            try
            {
                await _datacontext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeExists(id))
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

       
        [HttpPost]
        //[Authorize]
        public async Task<ActionResult<User>> PostEmployee(User employee)
        {
            _datacontext.users.Add(employee);
            await _datacontext.SaveChangesAsync();

            return CreatedAtAction("GetEmployee", new { id = employee.UserId }, employee);
        }

        
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var employee = await _datacontext.users.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            _datacontext.users.Remove(employee);
            await _datacontext.SaveChangesAsync();

            return NoContent();
        }

        private bool EmployeeExists(int id)
        {
            return _datacontext.users.Any(e => e.UserId == id);
        }
    }
}
