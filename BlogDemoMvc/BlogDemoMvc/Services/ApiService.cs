using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using BlogDemoMvc.Models.Post;

namespace BlogDemoMvc.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;

        public ApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
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
        public async Task<bool> CreatePost(PostViewModel postModel,string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                 new AuthenticationHeaderValue("Bearer", token);

            var dto = new
            {
                Title = postModel.Title,
                Image = postModel.Image,
                Description = postModel.Description,
                CategoryId = postModel.CategoryId,
            };
           
            var response = await _httpClient.PostAsJsonAsync("api/Posts/", dto);

            return response.IsSuccessStatusCode;
        }

        // Post detayını getir
        public async Task<PostViewModel> GetPostById(int id)
        {
            var response = await _httpClient.GetAsync($"api/Posts/GetPostsById?id={id}");
            if (!response.IsSuccessStatusCode)
                return null;

            return JsonConvert.DeserializeObject<PostViewModel>(await response.Content.ReadAsStringAsync());
        }

        // Post güncellemek için veri getir
        public async Task<PostUpdateViewModel> GetPostForUpdate(int id, string token)
        {

            _httpClient.DefaultRequestHeaders.Authorization =
                   new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.GetAsync($"api/Posts/{id}");
            if (!response.IsSuccessStatusCode)
                return null;

            return JsonConvert.DeserializeObject<PostUpdateViewModel>(await response.Content.ReadAsStringAsync());
        }

        // Post güncelle
        public async Task<bool> UpdatePost(int id, PostUpdateViewModel postModel, string token)
        {
            var client = _httpClient;

            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var jsonContent = new StringContent(
                JsonConvert.SerializeObject(postModel),
                Encoding.UTF8,
                "application/json");

            var response = await _httpClient.PutAsync($"api/Posts/{id}", jsonContent);

            return response.IsSuccessStatusCode;
        }


        //Postu silmek için detayı gör
        public async Task<PostViewModel> GetPostByIdForDelete(int id, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync($"api/Posts/Delete/{id}");
            if (!response.IsSuccessStatusCode)
                return null;

            return JsonConvert.DeserializeObject<PostViewModel>(await response.Content.ReadAsStringAsync());
        }

        // Post sil
        public async Task<bool> DeletePost(int id, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

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
        public async Task<bool> AddComment(CommentCreateViewModel commentDto,string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var jsonContent = new StringContent(
                JsonConvert.SerializeObject(commentDto),
                Encoding.UTF8,
                "application/json");

            var response = await _httpClient.PostAsync("api/Comments", jsonContent);
            return response.IsSuccessStatusCode;
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

