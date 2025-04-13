using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace BlogDemoApi.Dto
{
    public class PostDto
    {
        public int PostId { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;
        public string Image { get; set; } = "ai.jpg";
        public DateTime PublishedOn { get; set; }

        [Required]
        public string Description { get; set; } = string.Empty;
        public int UserId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public List<SelectListItem>? Categories { get; set; }

    }

}
