using System.ComponentModel.DataAnnotations.Schema;

namespace Lagalt.Models
{
	public class Applicant
	{
		public int Id { get; set; }
		public string UserId { get; set; }
		public string? Username { get; set; }
		public string? Letter { get; set; }
        //public ICollection<Project>? Projects { get; set; } = new List<Project>();
        //Changed to a single project
		public int? ProjectID { get; set; }
        [NotMapped]
		public Project Project { get; set; }
	}
}
