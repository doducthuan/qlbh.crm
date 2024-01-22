using System;
using System.Collections.Generic;

namespace QLBH_Dion.Models
{
    public partial class Setting
    {
        public int Id { get; set; }
        public int Active { get; set; }
        public string KeyValue { get; set; } = null!;
        public string Value { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime CreatedTime { get; set; }
    }
}
