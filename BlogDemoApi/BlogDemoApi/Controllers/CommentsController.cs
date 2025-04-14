using BlogDemoApi.Data;
using BlogDemoApi.Dto;
using BlogDemoApi.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BlogDemoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        public CommentsController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        //GET
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCommentsByPost(int id)
        {
            var comments = await dbContext.Comment
                .Include(c => c.User)
                .Where(c => c.PostId == id)
                .OrderByDescending(c => c.PublishedOn)
                .Select(c => new
            {
                c.CommentId,
                c.Text,
                c.PublishedOn,
                c.UserId,
                c.User.FirstName,
                c.User.LastName
                })
                .ToListAsync();

            return Ok(comments);
        }

        //POST
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddComment([FromBody] CommentCreateDto model)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

            var comment = new CommentEntity
            {
                Text = model.Text,
                PublishedOn = DateTime.Now,
                PostId = model.PostId,
                UserId = userId
            };

            dbContext.Comment.Add(comment);
            await dbContext.SaveChangesAsync();

            return Ok(new
            {
                comment.CommentId,
                comment.Text,
                comment.PublishedOn,
                comment.PostId,
                comment.UserId
            });
        }
    }
}
