using BlogDemoApi.Data;
using BlogDemoApi.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogDemoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {

        public readonly ApplicationDbContext dbContext;
        public readonly ILogger<CategoryController> logger;

        public CategoryController(ApplicationDbContext dbContext, ILogger<CategoryController> logger)
        {
            this.dbContext = dbContext;
            this.logger = logger;
        }


        //kategori listele
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categories = await dbContext.Category.ToListAsync();
            return Ok(categories);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CategoryEntity model)
        {
            var category = new CategoryEntity
            {
               CategoryName = model.CategoryName
            };

            dbContext.Category.Add(category);
            await dbContext.SaveChangesAsync();

            return Ok(category);
        }

    }
}
