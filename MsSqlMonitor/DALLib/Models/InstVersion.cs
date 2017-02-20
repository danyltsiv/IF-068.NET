using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DALLib.Models
{
    [Table("InstanceVersion")]
    public class InstanceVersion
    {
        [Key, Required, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; internal set; }

        [Required, MaxLength(20)]
        public string Version { get; set; }

        public virtual ICollection<Instance> Instances { get; set; } = new HashSet<Instance>();
    }
}
