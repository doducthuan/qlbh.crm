using System;
using System.Collections.Generic;

namespace QLBH_Dion.Models
{
    public partial class Province
    {
        public Province()
        {
            Products = new HashSet<Product>();
        }

        public int Id { get; set; }
        public int Active { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime CreatedTime { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
