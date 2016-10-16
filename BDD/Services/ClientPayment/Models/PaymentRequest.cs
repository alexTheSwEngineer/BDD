#region

using System;

#endregion

namespace Services.Accounting.ClientPayment.Models
{
    public class PaymentRequest
    {   
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
    }
}
