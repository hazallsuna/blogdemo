namespace BlogDemoMvc.Models.User
{
    public class LoginResponseViewModel
    {
        public UserViewModel UserDetails { get; set; }
        public string Token { get; set; }
    }
}
