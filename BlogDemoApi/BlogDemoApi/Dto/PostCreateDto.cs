namespace BlogDemoApi.Dto
{
    public class PostCreateDto
    {
        public string Title { get; set; } = "";
        public string Image { get; set; } = "ai.jpg";
        public string Description { get; set; } = "";
        public int CategoryId { get; set; }
    }
}
