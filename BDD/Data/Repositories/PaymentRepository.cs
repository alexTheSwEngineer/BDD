using Domain.Accounting;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    class PaymentRepository : RepositoryBase<Payment>, IPaymentRepository
    {
        public PaymentRepository(DbContext dbContext) : base(dbContext) { }
        public decimal VeryComplexSumOfAllPaymentItems(int id)
        {
            return DbContext.Set<PaymentItem>().Where(x => x.PaymentId == id).Sum(x=>x.Amount);
        }
    }
}
