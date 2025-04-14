namespace BlogDemoApi.Model
{
    public class LoginResponse
    {
        public UserEntity UserDetails { get; set; }
        public string Token {  get; set; }
    }
}
