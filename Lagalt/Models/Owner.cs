namespace Lagalt.Models
{
	public class Owner
	{
		public int Id { get; set; }
		public string? Username { get; set; }
		public ICollection<Project>? Projects { get; set; } = new List<Project>();
	}
}
