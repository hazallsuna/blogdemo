using Azure;
using System.ComponentModel.DataAnnotations;

namespace BlogDemoApi.Model
{
    public class PostEntity
    {
        [Key]
        public int PostId { get; set; }
        public string? Title { get; set; }
        public string Image { get; set; } = "ai.jpg";
        public DateTime PublishedOn { get; set; }
        public string? Description { get; set; }
        public int UserId { get; set; }
        public UserEntity User { get; set; } = null!;
        public int CategoryId { get; set; }
        public CategoryEntity Category { get; set; } = null!;
        public List<CommentEntity> Comments { get; set; } = new List<CommentEntity>();
    }
}
