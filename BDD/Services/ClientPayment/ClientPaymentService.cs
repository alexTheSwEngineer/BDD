#region
using Data;
using Domain;
using Domain.Accounting;
using Services.Accounting.ClientPayment;
using Services.Accounting.ClientPayment.Models;
using System;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace Services.Accounting
{
    public partial class ClientPaymentService : IClientPaymentService
    {
        private const decimal ExistingDebtFeeRate =0.25m;
        private readonly IDataContext _dataContext;
        private readonly ISystemUser _systemUser;

        public ClientPaymentService(IDataContext dataContext,
                                    ISystemUser systemUser)
        {
            _dataContext = dataContext;
            _systemUser = systemUser;
        }
        public void MakePayment(decimal paymentAmount)
        {
            var paymentItem = new PaymentItem
            {
                Amount = paymentAmount,
            };

            _dataContext.PaymentItems.Add(paymentItem);
            _dataContext.SaveChanges();

        }

        public PaymentItem MakePaymentBlackBox(decimal paymentAmount)
        {
            return new PaymentItem
            {
                Amount = paymentAmount,
            };
        }

        public void MakePaymentNastyDependancy(decimal paymentAmount)
        {
            var debt = _dataContext.Payments.TotalDebt();
            var fee =debt * ExistingDebtFeeRate;

            var paymentItem = MakePaymentBlackBox(paymentAmount);
            paymentItem.AddFee(fee);

            _dataContext.PaymentItems.Add(paymentItem);
            _dataContext.SaveChanges();
        }
    }
}