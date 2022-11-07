using System.ComponentModel.DataAnnotations;

namespace Lagalt.Relationships
{
    public class ProjectUser
    {
        public int ProjectId { get; set; }
        public int UserId { get; set; }
    }
}
