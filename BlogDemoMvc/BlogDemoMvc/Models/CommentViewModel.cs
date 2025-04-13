namespace BlogDemoMvc.Models
{
    public class CommentViewModel
    {
        public int CommentId { get; set; }
        public string? Text { get; set; }
        public DateTime PublishedOn { get; set; }

        public int UserId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    }
}
