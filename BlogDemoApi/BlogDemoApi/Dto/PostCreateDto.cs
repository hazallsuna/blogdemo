namespace BlogDemoApi.Dto
{
    public class PostCreateDto
    {
        public string Title { get; set; } = "";
        public string Image { get; set; } = "ai.jpg";
        public string Description { get; set; } = "";
        public int UserId { get; set; } = 1; // test için sabit
        public int CategoryId { get; set; }
    }
}
