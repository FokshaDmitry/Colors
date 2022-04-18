using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.EntityContext
{
    [Serializable]
    public class Entity
    {
        public Guid id { get; set; }
        public string? Name { get; set; }
        public string? VendorName { get; set; }
        [NotMapped]
        public byte[] Image { get; set; }
    }
}
