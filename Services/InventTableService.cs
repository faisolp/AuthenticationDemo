// ตัวอย่างการใช้ LINQ กับ InventTable
// สามารถเพิ่มเมธอดเหล่านี้ใน InventTablesController หรือสร้าง Service แยกต่างหาก

using System.Linq;
using Microsoft.EntityFrameworkCore;
//using ODataDemo.Data;
using ODataDemo.Models;

namespace ODataDemo.Services
{
    public class InventTableService
    {
        private readonly ApplicationDbContext _context;

        public InventTableService(ApplicationDbContext context)
        {
            _context = context;
        }

        // ค้นหาสินค้าตามชื่อ
        public async Task<List<InventTable>> SearchByName(string searchTerm)
        {
            return await _context.InventTables
                .Where(item => item.ItemName.Contains(searchTerm))
                .ToListAsync();
        }

        // ค้นหาสินค้าโดยมีเงื่อนไขหลายอย่าง
        public async Task<List<InventTable>> SearchWithMultipleConditions(string itemIdPrefix, string nameContains)
        {
            return await _context.InventTables
                .Where(item => item.ItemId.StartsWith(itemIdPrefix) && item.ItemName.Contains(nameContains))
                .OrderBy(item => item.ItemName)
                .ToListAsync();
        }

        // อัปเดตชื่อสินค้าตาม ID
        public async Task<bool> UpdateItemName(string itemId, string newName)
        {
            var item = await _context.InventTables.FindAsync(itemId);
            if (item == null)
                return false;

            item.ItemName = newName;
            await _context.SaveChangesAsync();
            return true;
        }

        // เพิ่มสินค้าหลายรายการในครั้งเดียว
        public async Task AddMultipleItems(List<InventTable> items)
        {
            await _context.InventTables.AddRangeAsync(items);
            await _context.SaveChangesAsync();
        }

        // ลบสินค้าที่มี ID ขึ้นต้นด้วยคำที่กำหนด
        public async Task<int> DeleteItemsWithIdPrefix(string prefix)
        {
            var itemsToDelete = await _context.InventTables
                .Where(item => item.ItemId.StartsWith(prefix))
                .ToListAsync();

            _context.InventTables.RemoveRange(itemsToDelete);
            await _context.SaveChangesAsync();
            
            return itemsToDelete.Count;
        }
        
        // นับจำนวนสินค้าทั้งหมด
        public async Task<int> CountAllItems()
        {
            return await _context.InventTables.CountAsync();
        }
    }
}