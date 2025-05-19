using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VietnamBusiness.Models
{
    [Table("CompanyBusinessMapping")]
    public class CompanyBusinessMapping
    {
        [Required]
        public long CompanyId { get; set; }

        [Required]
        public long BusinessId { get; set; }

        // Navigation properties
        [ForeignKey(nameof(CompanyId))]
        public virtual Company Company { get; set; }

        [ForeignKey(nameof(BusinessId))]
        public virtual Business Business { get; set; }
    }
}
