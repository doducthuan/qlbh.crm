using System;
using System.Collections.Generic;

namespace QLBH_Dion.Models
{
    public partial class Account
    {
        public Account()
        {
            ActivityLogs = new HashSet<ActivityLog>();
            Notifications = new HashSet<Notification>();
            Orders = new HashSet<Order>();
        }

        public int Id { get; set; }
        public string? GuidId { get; set; }
        public string? GuidIdApp { get; set; }
        public int AccountStatusId { get; set; }
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string? Description { get; set; }
        public string? Info { get; set; }
        public int Active { get; set; }
        public DateTime CreatedTime { get; set; }
        public string? Search { get; set; }
        public int RoleId { get; set; }

        public virtual AccountStatus AccountStatus { get; set; } = null!;
        public virtual Role Role { get; set; } = null!;
        public virtual ICollection<ActivityLog> ActivityLogs { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
