using BlogDemoMvc.Models;
using BlogDemoMvc.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;

namespace BlogDemoMvc.Controllers
{
    public class PostController : Controller
    {
        private readonly ApiService _apiService;
        private string baseURL = "https://localhost:7278/";

        public PostController()
        {
            _apiService = new ApiService();
        }

        public async Task<IActionResult> Index()
        {
            var posts = await _apiService.GetAllPosts();
            if (posts == null)
                return View("ErrorPage");

            return View(posts);
        }
        public async Task<IActionResult> Create()
        {
            var categoryList = await _apiService.GetAllCategories();
            if (categoryList == null)
                return View(new PostViewModel());

            var model = new PostViewModel
            {
                Categories = categoryList.Select(c => new SelectListItem
                {
                    Value = c.CategoryId.ToString(),
                    Text = c.CategoryName
                }).ToList()
            };

            return View(model);
        }

        public async Task<IActionResult> CreatePost(PostViewModel post)
        {
            bool success = await _apiService.CreatePost(post);

            if (success)
                return RedirectToAction("Index");
            else
                return View("ErrorPage"); ;
        }

        public async Task<IActionResult> Details(int id)
        {
            var postData = await _apiService.GetPostById(id);
            if (postData == null)
                return View("ErrorPage");

            var comments = await _apiService.GetCommentsByPostId(id);
            if (comments != null)
                postData.Comments = comments;

            return View(postData);
        }
        public async Task<IActionResult> GetUpdatePost(int id)
        {
            var postUpdateViewModel = await _apiService.GetPostForUpdate(id);
            if (postUpdateViewModel == null)
                return View("ErrorPage");

            postUpdateViewModel.Categories = await _apiService.GetCategoriesAsSelectItems(postUpdateViewModel.CategoryId);

            return View(postUpdateViewModel);
        }
        public async Task<IActionResult> UpdatePost(int id, PostUpdateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Categories = await _apiService.GetCategoriesAsSelectItems(model.CategoryId);
                return View("Update", model);
            }

            bool success = await _apiService.UpdatePost(id, model);

            if (success)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View("ErrorPage");
            }
        }
        public async Task<IActionResult> DeleteConfirm(int id)
        {
            var postViewModel = await _apiService.GetPostById(id);
            if (postViewModel == null)
                return View("ErrorPage");

            return View(postViewModel);
        }
        public async Task<IActionResult> Delete(int id)
        {
            bool success = await _apiService.DeletePost(id);

            if (success)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View("ErrorPage");
            }
        }
        public async Task<IActionResult> AddComment(CommentCreateViewModel model)
        {
            bool success = await _apiService.AddComment(model);

            // Add error handling
            if (!success)
            {
                // Hata loglama veya TempData'ya mesaj ekleme işlemleri burada yapılabilir
                Console.WriteLine($"API Error: Comment could not be added");
            }

            return RedirectToAction("Details", new { id = model.PostId });
        }

        public IActionResult ErrorPage()
        {
            return View();
        }

    }

}
