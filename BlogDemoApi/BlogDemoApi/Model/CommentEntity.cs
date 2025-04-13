using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;

namespace BlogDemoApi.Model
{
    public class CommentEntity
    {
        [Key]
        public int CommentId { get; set; }
        public string? Text { get; set; }
        public DateTime PublishedOn { get; set; }
        public int PostId { get; set; }
        public PostEntity Post { get; set; } = null!;
        public int UserId { get; set; }
        public UserEntity User { get; set; } = null!;
    }
}
