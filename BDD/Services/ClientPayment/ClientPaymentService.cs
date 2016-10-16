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
        private const decimal ExistingDebtFeeRate = 0.3m;
        private readonly IDataContext _dataContext;
        private readonly ISystemUser _systemUser;

        public ClientPaymentService(IDataContext dataContext,
                                    ISystemUser systemUser)
        {
            _dataContext = dataContext;
            _systemUser = systemUser;
        }
        public void MakePayment(PaymentRequest request)
        {
            //Nasty dependancy on previous db state
            var existingDebt = _dataContext.PaymentItems.GetAll()
                                               .Where(x => x.Amount < 0)
                                               .Sum(x => x.Amount);

            var payment = new Domain.Accounting.Payment
            {
                Date = request.Date,
                PaymentItems = new List<PaymentItem>()
                {
                    new PaymentItem
                    {
                        Amount = request.Amount+existingDebt*ExistingDebtFeeRate,
                    }
                }
            };
            

            _dataContext.Payments.Add(payment);
            _dataContext.SaveChanges();
        }
    }
}