using System.ComponentModel.DataAnnotations;

namespace Tutorial.Models
{
    public class BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public int CreatedBy { get; set; }
        public DateTime? LastModified { get; set; }
        public int? LastModifiedBy { get; set; }
    }
}
