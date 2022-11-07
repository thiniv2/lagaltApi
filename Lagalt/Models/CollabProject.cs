namespace Lagalt.Models
{
	public class CollabProject
	{
		public int Id { get; set; }
		public string? Title { get; set; }
		public string? Description { get; set; }
		public bool IsPublic { get; set; } = false;
		public string? Skillset { get; set; }
		public FieldType Field { get; set; }

		public ICollection<User>? Users { get; set; } = new List<User>();
		//public ICollection<Applicant>? Applicants { get; set; } = new List<Applicant>();
		//public ICollection<Owner>? Owners { get; set; } = new List<Owner>();
	}
}
