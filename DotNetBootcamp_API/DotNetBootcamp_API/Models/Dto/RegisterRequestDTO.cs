namespace DotNetBootcamp_API.Models.Dto
{
    public class RegisterRequestDTO
    {
        public string Username { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}
