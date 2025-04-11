using System.ComponentModel.DataAnnotations;

namespace ODataDemo.Models
{
    public class InventTable
    {
        [Key]
        public string ItemId { get; set; } = string.Empty;
        public string ItemName { get; set; } = string.Empty;
    }
}