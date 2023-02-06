using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Lagalt.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
		public bool IsPublic { get; set; } = false;
		public int TotalSkills { get; set; } = 0;
		public FieldType Field { get; set; }
		public string? Progress { get; set; }
		[NotMapped]
		public string[]? Skillset { get; set; }
		public ICollection<User>? Users { get; set; } = new List<User>();
        //public List<int> ApplicantIDs { get; set; } = new List<int>();
		public ICollection<Applicant>? Applicants { get; set; } = new List<Applicant>();
		public string Owner { get; set; }
	}

    public enum FieldType
    {
        Music,
        Film,
        GameDevelopment,
        WebDevelopment
    }
}
