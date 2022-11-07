namespace Lagalt.Models.DTO
{
    public class UserReadDTO
    {
        public string Id { get; set; }
        public string? Username { get; set; }
        public bool IsPublic { get; set; }

        public string[]? History { get; set; }
        public string? Biography { get; set; }
        public string[]? Skills { get; set; }
        public ICollection<Project>? Projects { get; set; } = new List<Project>();
        public ICollection<CollabProject>? CollaborationProjects { get; set; } = new List<CollabProject>();
    }
}
