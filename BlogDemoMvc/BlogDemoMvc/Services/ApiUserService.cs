using BlogDemoMvc.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace BlogDemoMvc.Services
{
    public class ApiUserService
    {
        private readonly HttpClient _httpClient;

        public ApiUserService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<LoginResponseViewModel> AuthenticateUser(LoginRequestViewModel userDetails)
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync($"api/Users/Login",userDetails);
            if (!response.IsSuccessStatusCode)
                return null;

            var content=await response.Content.ReadAsStringAsync();
            var apiResponse=JsonConvert.DeserializeObject<LoginResponseViewModel>(content);

            return apiResponse;
        }
    }
}
