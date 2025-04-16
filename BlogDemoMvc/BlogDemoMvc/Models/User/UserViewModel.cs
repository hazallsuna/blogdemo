using BlogDemoMvc.Models.Post;

namespace BlogDemoMvc.Models.User
{
    public class UserViewModel
    {
        public int UserId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }

        public List<PostViewModel> Posts { get; set; } = new List<PostViewModel>();
        public List<CommentViewModel> Comments { get; set; } = new List<CommentViewModel>();
    }
}
