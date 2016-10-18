#region

#endregion

namespace Domain.Accounting
{
    public class PaymentItem
    {
        public int PaymentId { get; set; }
       
        public decimal Amount { get; set; }
        public virtual Payment Payment { get; set; }

        public void AddFee(decimal debtFee)
        {
            Amount = Amount+ debtFee;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (obj == this)
            {
                return true;
            }

            var other = obj as PaymentItem;
            if (other == null)
            {
                return false;
            }
            return Amount == other.Amount;
        }

        public override int GetHashCode()
        {
            return Amount.GetHashCode();
        }
    }
}
