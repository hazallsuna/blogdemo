using BlogDemoMvc.Models.Post;
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
        public PostController(ApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> Index(int? categoryId)
        {
            var posts = await _apiService.GetAllPosts();
            if (posts == null)
                return View("ErrorPage");

            if (categoryId.HasValue)
            {
                posts=posts.Where(p=> p.CategoryId == categoryId).ToList();
            }

            ViewBag.Categories=await _apiService.GetCategoriesAsSelectItems(categoryId ?? 0);
            return View(posts);
         }
        public async Task<IActionResult> Create()
        {
            var token = HttpContext.Session.GetString("APIToken");

            if (string.IsNullOrEmpty(token))
                return RedirectToAction("Login", "AuthUser"); 

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
            var token = HttpContext.Session.GetString("APIToken");

            if (string.IsNullOrEmpty(token))
                return RedirectToAction("Login", "AuthUser");

            bool success = await _apiService.CreatePost(post, token);

            if (success)
                return RedirectToAction("Index");
            else
                return View("AccessDenied");
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
            var token = HttpContext.Session.GetString("APIToken");
            var postUpdateViewModel = await _apiService.GetPostForUpdate(id, token);

            if (postUpdateViewModel == null)
                return View("AccessDenied");

            postUpdateViewModel.Categories = await _apiService.GetCategoriesAsSelectItems(postUpdateViewModel.CategoryId);

            return View("Update", postUpdateViewModel);
        }
        public async Task<IActionResult> UpdatePost(int id, PostUpdateViewModel model)
        {
            var token = HttpContext.Session.GetString("APIToken");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "AuthUser");
            }

            if (!ModelState.IsValid)
            {
                model.Categories = await _apiService.GetCategoriesAsSelectItems(model.CategoryId);
                return View("Update", model);
            }

            bool success = await _apiService.UpdatePost(id, model, token);

            if (success)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View("AccessDenied");
            }
        }
        public async Task<IActionResult> DeleteConfirm(int id)
        {
            var token = HttpContext.Session.GetString("APIToken");
            if (string.IsNullOrEmpty(token))
                return View("AccessDenied");

            var postViewModel = await _apiService.GetPostByIdForDelete(id,token);

            if (postViewModel == null)
                return View("AccessDenied");

            return View(postViewModel);
        }
        public async Task<IActionResult> Delete(int id)
        {
            var token = HttpContext.Session.GetString("APIToken");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "AuthUser");
            }
            bool success = await _apiService.DeletePost(id,token);

            if (success)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View("AccessDenied");
            }
        }
        public async Task<IActionResult> AddComment(CommentCreateViewModel model)
        {
            var token = HttpContext.Session.GetString("APIToken");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "AuthUser");
            }

            bool success = await _apiService.AddComment(model,token);

            // Add error handling
            if (!success)
            {
                
                Console.WriteLine($"API Error: Comment could not be added");
            }

            return RedirectToAction("Details", new { id = model.PostId });
        }

        public IActionResult ErrorPage()
        {
            return View();
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

    }

}
