#region

#endregion

namespace Domain.Accounting
{
    public class PaymentItem
    {
        public int PaymentId { get; set; }
       
        public decimal Amount { get; set; }
        public virtual Payment Payment { get; set; }
    }
}
