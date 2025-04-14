namespace BlogDemoMvc.Models
{
    public class LoginResponseViewModel
    {
        public UserViewModel UserDetails { get; set; }
        public string Token { get; set; }
    }
}
