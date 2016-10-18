#region

using Domain.Accounting;
using Services.Accounting.ClientPayment.Models;

#endregion

namespace Services.Accounting.ClientPayment
{
    /// <summary>
    ///     Use this service to pay open AppointmentAnimals for public clients or
    ///     an invoice for a volume client
    /// </summary>
    public interface IClientPaymentService
    {
        void MakePayment(decimal amount);
        PaymentItem MakePaymentBlackBox(decimal amount);
        void MakePaymentNastyDependancy(decimal amount);
    }
}
