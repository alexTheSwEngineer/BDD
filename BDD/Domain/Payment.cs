#region

using System;
using System.Collections.Generic;

#endregion

namespace Domain.Accounting
{
    public partial class Payment 
    {
        public Payment()
        {
            PaymentItems = new List<PaymentItem>();
        }
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public ICollection<PaymentItem> PaymentItems { get; set; }
    }
}
