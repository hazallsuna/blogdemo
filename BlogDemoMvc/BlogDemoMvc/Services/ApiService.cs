using System.Net.Http.Headers;
using System.Text;
using BlogDemoMvc.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BlogDemoMvc.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private string _ApiURLPath = "https://localhost:7278/";

        public ApiService()
        {
            _httpClient = new HttpClient()
            {
                BaseAddress = new Uri(_ApiURLPath)
            };            
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
        }

        // Tüm postları getir
        public async Task<List<PostViewModel>> GetAllPosts()
        {
            var response = await _httpClient.GetAsync("api/Posts/");
            if (!response.IsSuccessStatusCode)
                return null;

            string result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<PostViewModel>>(result);
        }

        // Kategorileri getir
        public async Task<List<CategoryViewModel>> GetAllCategories()
        {
            var response = await _httpClient.GetAsync("api/Category");
            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<List<CategoryViewModel>>();
        }

        // Post oluştur
        public async Task<bool> CreatePost(PostViewModel postModel)

        {
            var dto = new
            {
                Title = postModel.Title,
                Image = postModel.Image,
                Description = postModel.Description,
                CategoryId = postModel.CategoryId,
                UserId = 1 // testlik
            };

            var response = await _httpClient.PostAsJsonAsync("api/Posts/", dto);
            return response.IsSuccessStatusCode;
        }

        // Post detayını getir
        public async Task<PostViewModel> GetPostById(int id)
        {
            var response = await _httpClient.GetAsync($"api/Posts/{id}");
            if (!response.IsSuccessStatusCode)
                return null;

            return JsonConvert.DeserializeObject<PostViewModel>(await response.Content.ReadAsStringAsync());
        }

        // Post güncelle
        public async Task<bool> UpdatePost(int id, PostUpdateViewModel postModel)
        {
            var jsonContent = new StringContent(
                JsonConvert.SerializeObject(postModel),
                Encoding.UTF8,
                "application/json");

            var response = await _httpClient.PutAsync($"api/Posts/{id}", jsonContent);
            return response.IsSuccessStatusCode;
        }

        // Post sil
        public async Task<bool> DeletePost(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/Posts/{id}");
            var errorContent = await response.Content.ReadAsStringAsync();
             

            return response.IsSuccessStatusCode;
        }

        // Yorumları getir
        public async Task<List<CommentViewModel>> GetCommentsByPostId(int postId)
        {
            var response = await _httpClient.GetAsync($"api/Comments/{postId}");
            if (!response.IsSuccessStatusCode)
                return new List<CommentViewModel>();

            return JsonConvert.DeserializeObject<List<CommentViewModel>>(await response.Content.ReadAsStringAsync());
        }

        // Yorum ekle
        public async Task<bool> AddComment(CommentCreateViewModel commentDto)
        {
            var jsonContent = new StringContent(
                JsonConvert.SerializeObject(commentDto),
                Encoding.UTF8,
                "application/json");

            var response = await _httpClient.PostAsync("api/Comments", jsonContent);
            return response.IsSuccessStatusCode;
        }

        // Post güncellemek için veri getir
        public async Task<PostUpdateViewModel> GetPostForUpdate(int id)
        {
            var response = await _httpClient.GetAsync($"api/Posts/{id}");
            if (!response.IsSuccessStatusCode)
                return null;

            return JsonConvert.DeserializeObject<PostUpdateViewModel>(await response.Content.ReadAsStringAsync());
        }

        // Kategori listesini SelectListItem formatında hazırla
        public async Task<List<SelectListItem>> GetCategoriesAsSelectItems(int selectedCategoryId = 0)
        {
            var categoryList = await GetAllCategories();
            if (categoryList == null)
                return new List<SelectListItem>();

            return categoryList.Select(c => new SelectListItem
            {
                Value = c.CategoryId.ToString(),
                Text = c.CategoryName,
                Selected = c.CategoryId == selectedCategoryId
            }).ToList();
        }

    }




}

