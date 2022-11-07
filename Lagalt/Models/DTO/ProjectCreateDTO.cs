using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lagalt.Models.DTO
{
    public class ProjectCreateDTO
    {

        public string? Title { get; set; }
        public string? Description { get; set; }
        [NotMapped]
        public string[]? Skillset { get; set; }
        public FieldType Field { get; set; }
        public string OwnerId { get; set; }
		public string progress { get; set; }
	}
}
