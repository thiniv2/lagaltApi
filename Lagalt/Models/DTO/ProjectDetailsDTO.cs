using Lagalt.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lagalt.Models.DTO
{
    public class ProjectDetailsDTO
	{
        public int Id { get; set; }
        public string? Title { get; set; }
        public FieldType Field { get; set; }
        public string? Description { get; set; }
		[NotMapped]
		public string[]? Skillset { get; set; }
	}
}
