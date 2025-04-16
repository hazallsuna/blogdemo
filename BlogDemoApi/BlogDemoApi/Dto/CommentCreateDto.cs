namespace BlogDemoApi.Dto
{
    public class CommentCreateDto
    {
        public string Text { get; set; } = string.Empty;
        public int PostId { get; set; }
        public int UserId { get; set; }
    }
}
