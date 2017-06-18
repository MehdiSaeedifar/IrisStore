using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iris.DomainClasses
{
    public class Factor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public DateTime IssueDate { get; set; } = DateTime.Now;

        public FactorStatus Status { get; set; } = FactorStatus.Paid;

        public ApplicationUser User { get; set; }
        public int UserId { get; set; }

        public virtual ICollection<FactorProduct> Products { get; set; }
    }

    public enum FactorStatus
    {
        [Description("پرداخت شده")]
        Paid,
        [Description("لغو شده")]
        Cancelled,
        [Description("ارسال شده")]
        Sent,
        [Description("تحویل شده")]
        Delivered
    }
}
