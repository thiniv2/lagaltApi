using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Lagalt.Models
{
    public class User
    {
        public string Id { get; set; }
        public string? Username { get; set; }
        public bool IsPublic { get; set; }
        //Todo: proper type for OAuth token
        //public string? Token { get; set; }

		[NotMapped]
		public string[]? History { get; set; }
		public ICollection<Project>? Projects { get; set; } = new List<Project>();
		public ICollection<CollabProject>? CollaborationProjects { get; set; } = new List<CollabProject>();
        public string? Biography { get; set; }
        [NotMapped]
        public string[]? Skills { get; set; }
	}
}
