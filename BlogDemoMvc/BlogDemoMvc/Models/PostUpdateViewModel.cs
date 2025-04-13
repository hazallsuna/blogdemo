using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace BlogDemoMvc.Models
{
    public class PostUpdateViewModel
    {
        public int PostId { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        [StringLength(100)]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description is required.")]
        [StringLength(300)]
        public string Description { get; set; } = string.Empty;

        public string Image { get; set; } = "ai.jpg";

        [Required]
        public int CategoryId { get; set; }
        public DateTime PublishedOn { get; set; }

        public int UserId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        public string CategoryName { get; set; } = string.Empty;
        public List<SelectListItem>? Categories { get; set; }

    }

}
