
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI6_Demo.Model.BaseModel
{
    public class EntityWithAudit
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public long? ModifiedBy { get; set; }
        public DateTime? ModifiedDate{ get; set;}        
        public bool IsActive { get; set; } = true;
        public bool IsDelete { get; set; } = false;
    }
}
