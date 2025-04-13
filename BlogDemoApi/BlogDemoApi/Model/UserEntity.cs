using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;

namespace BlogDemoApi.Model
{
    public class UserEntity
    {
        [Key]
        public int UserId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }

        public List<PostEntity> Posts { get; set; } = new List<PostEntity>();
        public List<CommentEntity> Comments { get; set; } = new List<CommentEntity>();
    }
}
