using BlogDemoApi.Data;
using BlogDemoApi.Dto;
using BlogDemoApi.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace BlogDemoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        public readonly ApplicationDbContext dbContext;
        public readonly ILogger<PostsController> logger;
        public PostsController(ApplicationDbContext dbContext, ILogger<PostsController> lo
            )
        {
            this.dbContext = dbContext;
            this.logger = logger;
        }

        //tüm postları getiriyoruz
        [HttpGet]
        public async Task<List<PostDto>> GetAllPosts()
        {
            return await dbContext.Post
                .Include(p => p.Category)
                .Include(p => p.User) 
                .Select(p => new PostDto
                {
                    PostId = p.PostId,
                    Title = p.Title,
                    Image = p.Image,
                    PublishedOn = p.PublishedOn,
                    Description = p.Description,
                    UserId = p.UserId,
                    FirstName = p.User.FirstName,
                    LastName = p.User.LastName,
                    CategoryId = p.CategoryId,
                    CategoryName = p.Category.CategoryName
                })
                .ToListAsync();
        }


        //seçilen postun detayını getiriyoruz
        [HttpGet("GetPostsById")]
        public async Task<ActionResult<PostDto>> GetPostsDetails(int Id)
        {
            if (Id == 0)
            {
                logger.LogError("Post Id was not passed");
                return BadRequest();
            }

            var post = await dbContext.Post
                .Include(p => p.Category)
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.PostId == Id);

            if (post == null)
            {
                return NotFound();
            }

            var dto = new PostDto
            {
                PostId = post.PostId,
                Title = post.Title,
                Image = post.Image,
                PublishedOn = post.PublishedOn,
                Description = post.Description,
                UserId = post.UserId,
                FirstName = post.User.FirstName,
                LastName = post.User.LastName,
                CategoryId = post.CategoryId,
                CategoryName = post.Category.CategoryName
            };

            return Ok(dto);
        }


        //yeni post ekliyoruz
        [HttpPost]
        public ActionResult<PostEntity> AddPost([FromBody] PostCreateDto postDto)
        {
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var post = new PostEntity
                {
                    Title = postDto.Title,
                    Image = postDto.Image,
                    Description = postDto.Description,
                    CategoryId = postDto.CategoryId,
                    UserId = postDto.UserId,
                    PublishedOn = DateTime.Now
                };

                dbContext.Post.Add(post);
                dbContext.SaveChanges();

                return Ok(post);
            }

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PostDto>> Get(int id)
        {
            var post = await dbContext.Post
                .Include(p => p.Category)
                .FirstOrDefaultAsync(x => x.PostId == id);

            if (post == null)
            {
                return NotFound();
            }

            var postDto = new PostDto
            {
                PostId = post.PostId,
                Title = post.Title ?? string.Empty,
                Description = post.Description ?? string.Empty,
                CategoryId = post.CategoryId,
                Image = post.Image ?? "ai.jpg",
                PublishedOn = post.PublishedOn,
                CategoryName = post.Category?.CategoryName ?? ""
            };

            return Ok(postDto);
        }

        //seçilen postu güncelliyoruz
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id,
            [FromBody] PostDto postDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var postDetails = dbContext.Post.FirstOrDefault(
               x => x.PostId == id);
            if (postDetails == null)
            {
                return NotFound();
            }

            postDetails.Title = postDto.Title;
            postDetails.Description = postDto.Description;
            postDetails.CategoryId = postDto.CategoryId;
            postDetails.Image = postDto.Image;
            postDetails.PublishedOn = DateTime.Now;

            dbContext.SaveChanges(); 
            
            return Ok();
        }

        //seçilen postu siliyoruz
        [HttpDelete("{id}")]
        public async Task<ActionResult<PostEntity>> Delete(int id)
        {
            var post = await dbContext.Post
               .Include(p => p.Comments)
               .FirstOrDefaultAsync(p => p.PostId == id);

            if (post == null)
                return NotFound();

            // Yorumları temizle
            dbContext.Comment.RemoveRange(post.Comments);

            // Post'u sil
            dbContext.Post.Remove(post);
            await dbContext.SaveChangesAsync();

            return NoContent();
        }


    }
}

