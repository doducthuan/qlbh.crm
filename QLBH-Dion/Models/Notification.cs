using System;
using System.Collections.Generic;

namespace QLBH_Dion.Models
{
    public partial class Notification
    {
        public int Id { get; set; }
        public int Active { get; set; }
        public int AccountId { get; set; }
        public int NotificationStatusId { get; set; }
        public string? Name { get; set; }
        public int? SenderId { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedTime { get; set; }

        public virtual Account Account { get; set; } = null!;
        public virtual NotificationStatus NotificationStatus { get; set; } = null!;
    }
}
