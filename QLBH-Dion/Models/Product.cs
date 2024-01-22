using System;
using System.Collections.Generic;

namespace QLBH_Dion.Models
{
    public partial class Product
    {
        public Product()
        {
            AuctionProducts = new HashSet<AuctionProduct>();
        }

        public int Id { get; set; }
        public int ProductCategoryId { get; set; }
        public int ProductBrandId { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }
        public string? Description { get; set; }
        public string? Photo { get; set; }
        public int? Active { get; set; }
        public DateTime CreatedTime { get; set; }
        public string? Search { get; set; }
        public int ProvinceId { get; set; }

        public virtual ProductBrand ProductBrand { get; set; } = null!;
        public virtual ProductCategory ProductCategory { get; set; } = null!;
        public virtual Province Province { get; set; } = null!;
        public virtual ICollection<AuctionProduct> AuctionProducts { get; set; }
    }
}
