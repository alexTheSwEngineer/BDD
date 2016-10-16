using Domain.Accounting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public interface IPaymentRepository : IRepository<Payment>
    {
        decimal VeryComplexSumOfAllPaymentItems(int id);
    }
}
