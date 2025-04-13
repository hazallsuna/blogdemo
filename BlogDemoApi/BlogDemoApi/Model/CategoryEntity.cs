using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;

namespace BlogDemoApi.Model
{
    public class CategoryEntity
    {
        [Key]
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public List<PostEntity> Posts { get; set; } = new();
    }
}
