using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using ODataDemo.Models;

namespace ODataDemo.Controllers.Base
{
    public abstract class BaseODataController<TEntity, TKey> : ODataController where TEntity : class
    {
        protected readonly ApplicationDbContext _context;
        protected readonly DbSet<TEntity> _dbSet;

        protected BaseODataController(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        [HttpGet]
        [EnableQuery]
        public virtual IActionResult Get()
        {
            return Ok(_dbSet);
        }

        [HttpGet("{id}")]
        [EnableQuery]
        public virtual async Task<IActionResult> Get(TKey id)
        {
            var entity = await _dbSet.FindAsync(id);

            if (entity == null)
            {
                return NotFound();
            }

            return Ok(entity);
        }

        [HttpPost]
        public virtual async Task<IActionResult> Post([FromBody] TEntity entity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _dbSet.Add(entity);
            await _context.SaveChangesAsync();

            return Created($"api/{typeof(TEntity).Name}/{GetEntityKey(entity)}", entity);
        }

        [HttpPut("{id}")]
        public virtual async Task<IActionResult> Put(TKey id, [FromBody] TEntity entity)
        {
            if (!KeysMatch(id, entity))
            {
                return BadRequest();
            }

            _context.Entry(entity).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await EntityExists(id))
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

        [HttpDelete("{id}")]
        public virtual async Task<IActionResult> Delete(TKey id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null)
            {
                return NotFound();
            }

            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        protected abstract bool KeysMatch(TKey id, TEntity entity);
        protected abstract object GetEntityKey(TEntity entity);
        protected abstract Task<bool> EntityExists(TKey id);
    }
}