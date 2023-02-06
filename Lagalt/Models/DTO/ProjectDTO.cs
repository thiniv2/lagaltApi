using Lagalt.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lagalt.Models.DTO
{
    public class ProjectDTO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        [NotMapped]
		public string[]? Skillset { get; set; }
        public FieldType Field { get; set; }
		public string? Progress { get; set; }
		public ICollection<User>? Users { get; set; } = new List<User>();
        public ICollection<Applicant>? Applicants { get; set; } = new List<Applicant>();
        public string Owner { get; set; }
        //public bool IsPublic { get; set; } = false;
    }
}
