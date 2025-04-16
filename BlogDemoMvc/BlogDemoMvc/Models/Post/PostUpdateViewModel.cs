using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace BlogDemoMvc.Models.Post
{
    public class PostUpdateViewModel
    {

        public int PostId { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        [StringLength(100)]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description is required.")]
        [StringLength(2000)]
        public string Description { get; set; } = string.Empty;

        public string Image { get; set; } = "ai.jpg";

        [Required]
        public int CategoryId { get; set; }
        public DateTime PublishedOn { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public List<SelectListItem>? Categories { get; set; }

    }

}
