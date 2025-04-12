using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;
using ODataDemo.Controllers.Base;
using ODataDemo.Models;

namespace ODataDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventTablesController : BaseODataController<InventTable, string>
    {
        public InventTablesController(ApplicationDbContext context) 
            : base(context)
        {
        }

        protected override bool KeysMatch(string id, InventTable entity)
        {
            return id == entity.ItemId;
        }

        protected override object GetEntityKey(InventTable entity)
        {
            return entity.ItemId;
        }

        protected override async Task<bool> EntityExists(string id)
        {
            return await _context.InventTables.AnyAsync(e => e.ItemId == id);
        }
    }
}