using Lagalt.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lagalt.Models.DTO
{
    public class ProjectBannerDTO
	{
        public int Id { get; set; }
        public string? Title { get; set; }
        [NotMapped]
        public string Field { get; set; }
		public int TotalSkills { get; set; }
		public string? owner { get; set; }
	}
}
