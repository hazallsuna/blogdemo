﻿using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace BlogDemoMvc.Models.Post
{
    public class PostViewModel
    {
        public int PostId { get; set; }
        [Required(ErrorMessage = "Title is required.")]
        [StringLength(100, ErrorMessage = "Title must be at most 100 characters.")]
        public string? Title { get; set; }
        public string Image { get; set; } = "ai.jpg";
        public DateTime PublishedOn { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        [StringLength(2000, ErrorMessage = "Description must be at most 2000 characters.")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Category is required.")]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<SelectListItem>? Categories { get; set; }
        public List<CommentViewModel> Comments { get; set; } = new();
        public int CommentCount { get; set; }

    }
}
