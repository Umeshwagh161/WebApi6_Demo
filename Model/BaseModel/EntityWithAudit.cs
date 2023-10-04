
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI6_Demo.Model.BaseModel
{

    //EntityWithAudit contain comman property that we have to use in every table so we declared it as base class.
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
