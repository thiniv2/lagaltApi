namespace Lagalt.Models.DTO
{
    public class UserEditDTO
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public bool IsPublic { get; set; }
        public string[]? History { get; set; }
    }
}
