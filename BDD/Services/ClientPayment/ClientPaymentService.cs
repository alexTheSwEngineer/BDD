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
            //Nasty dependancy on previous db state
            var existingDebt = _dataContext.PaymentItems.GetAll()
                                           .Where(x => x.Amount < 0)
                                           .Sum(x => x.Amount);

            var paymentItem = new PaymentItem
            {
                Amount = paymentAmount + existingDebt * ExistingDebtFeeRate,
            };


            _dataContext.PaymentItems.Add(paymentItem);
            _dataContext.SaveChanges();
        }
    }
}