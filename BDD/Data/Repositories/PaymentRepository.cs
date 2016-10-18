using Domain.Accounting;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class PaymentRepository : RepositoryBase<Payment>, IPaymentRepository
    {
        public PaymentRepository() { }
        public PaymentRepository(DbContext dbContext) : base(dbContext) { }

        public decimal TotalDebt()
        {
            var total  = Total();
            return total < 0 ?Math.Abs( total) : 0m;
        }
        

        public decimal Total()
        {
            return DbContext.Set<PaymentItem>().Sum(x => x.Amount);
        }
    }
}
