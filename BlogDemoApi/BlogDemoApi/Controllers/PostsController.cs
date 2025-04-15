using BlogDemoApi.Data;
using BlogDemoApi.Dto;
using BlogDemoApi.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Security.Claims;

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
        [Authorize]
        [HttpPost]
        public ActionResult<PostEntity> AddPost([FromBody] PostCreateDto postDto)
        {
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var post = new PostEntity
                {
                    Title = postDto.Title,
                    Image = postDto.Image,
                    Description = postDto.Description,
                    CategoryId = postDto.CategoryId,
                    UserId = userId,
                    PublishedOn = DateTime.Now
                };

                dbContext.Post.Add(post);
                dbContext.SaveChanges();

                return Ok(post);
            }

        }


        //güncellenecek post için form getiriyoruz
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<PostDto>> Get(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

            var post = await dbContext.Post
                .Include(p => p.Category)
                .FirstOrDefaultAsync(x => x.PostId == id);

            if (post == null)
            {
                return NotFound();
            }

            if (post.UserId != userId)
            {
                return Forbid("Bu post size ait değil, formu göremezsiniz.");
            }
            var postDto = new PostDto
            {
                PostId = post.PostId,
                Title = post.Title ?? string.Empty,
                Description = post.Description ?? string.Empty,
                CategoryId = post.CategoryId,
                Image = post.Image,
                PublishedOn = post.PublishedOn,
                CategoryName = post.Category?.CategoryName ?? ""
            };

            return Ok(postDto);
        }

        //seçilen postu güncelliyoruz
        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id,
            [FromBody] PostDto postDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

            var postDetails = dbContext.Post.FirstOrDefault(
               x => x.PostId == id);
            if (postDetails == null)
            {
                return NotFound();
            }

            if (postDetails.UserId != userId)
            {
                return Forbid("Bu post size ait değil, güncelleyemezsiniz.");
            }

            postDetails.Title = postDto.Title;
            postDetails.Description = postDto.Description;
            postDetails.CategoryId = postDto.CategoryId;
            postDetails.Image = postDto.Image;
            postDetails.PublishedOn = DateTime.Now;

            dbContext.SaveChanges(); 
            
            return Ok();
        }

        //login olmuş kullanıcı için post detay 
        [Authorize]
        [HttpGet("Delete/{id}")]
        public async Task<ActionResult<PostDto>> GetPostByIdForDelete(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var post = await dbContext.Post
                  .Include(p => p.Category)
                  .FirstOrDefaultAsync(p => p.PostId == id);

            if (post == null)
                return NotFound();

            if (post.UserId != userId)
                return Forbid();

            var dto = new PostDto
            {
                PostId = post.PostId,
                Title = post.Title,
                Image = post.Image,
                PublishedOn = post.PublishedOn,
                Description = post.Description,
                UserId = post.UserId,
                CategoryId = post.CategoryId,
                CategoryName = post.Category.CategoryName
            };

            return Ok(dto);
        }

        //seçilen postu siliyoruz
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult<PostEntity>> Delete(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

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

        //görsel ekliyoruz
        [HttpPost("Image")]
        public async Task<IActionResult> UploadImage(IFormFile formFile)
        {
            try
            {
                if (formFile == null || formFile.Length == 0)
                {
                    return BadRequest("The file could not be selected.");
                }

                var extent = Path.GetExtension(formFile.FileName);
                var randomName = $"{Guid.NewGuid()}{extent}";
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", randomName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await formFile.CopyToAsync(stream);
                }

                return Ok(new { imageUrl = $"https://localhost:7278/img/{randomName}" });
            }
            catch (Exception ex)
            {
                // ❗ buraya düşüyorsa logla:
                return StatusCode(500, $"Sunucu hatası: {ex.Message}");
            }
        }


    }
}

