using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
//using ODataDemo.Data;
using ODataDemo.Models;

namespace ODataDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventTablesController : ODataController
    {
        private readonly ApplicationDbContext _context;

        public InventTablesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/InventTables
        [HttpGet]
        [EnableQuery]
        public IActionResult Get()
        {
            return Ok(_context.InventTables);
        }

        // GET: api/InventTables/IT0001
        [HttpGet("{id}")]
        [EnableQuery]
        public async Task<IActionResult> Get(string id)
        {
            var inventTable = await _context.InventTables.FindAsync(id);

            if (inventTable == null)
            {
                return NotFound();
            }

            return Ok(inventTable);
        }

        // POST: api/InventTables
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] InventTable inventTable)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.InventTables.Add(inventTable);
            await _context.SaveChangesAsync();

            return Created($"api/InventTables/{inventTable.ItemId}", inventTable);
        }

        // PUT: api/InventTables/IT0001
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] InventTable inventTable)
        {
            if (id != inventTable.ItemId)
            {
                return BadRequest();
            }

            _context.Entry(inventTable).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await InventTableExists(id))
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

        // DELETE: api/InventTables/IT0001
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var inventTable = await _context.InventTables.FindAsync(id);
            if (inventTable == null)
            {
                return NotFound();
            }

            _context.InventTables.Remove(inventTable);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private async Task<bool> InventTableExists(string id)
        {
            return await _context.InventTables.AnyAsync(e => e.ItemId == id);
        }
    }
}