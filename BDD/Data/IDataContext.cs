#region

using Domain.Accounting;
using System;
using System.Threading.Tasks;

#endregion

namespace Data
{
    /// <summary>
    ///     Interface for our Data Context
    /// </summary>
    public interface IDataContext : IDisposable
    {
        IPaymentRepository Payments { get; }
        IRepository<PaymentItem> PaymentItems { get; }

        /// <summary>
        ///     Save pending changes to the database
        /// </summary>
        void SaveChanges();

        Task<int> SaveChangesAsync();
    }
}