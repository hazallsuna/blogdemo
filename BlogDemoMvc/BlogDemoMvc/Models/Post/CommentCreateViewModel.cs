using System.ComponentModel.DataAnnotations;


namespace BlogDemoMvc.Models.Post
{
    public class CommentCreateViewModel
    {

        public string Text { get; set; } = string.Empty;


        public int PostId { get; set; }


        public int UserId { get; set; } = 1;
    }

}
