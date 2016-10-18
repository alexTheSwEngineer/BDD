#region

using Domain.Accounting;
using System.Data.Entity;
#endregion

namespace Data
{
    public class EfDbContext : DbContext
    {
        static EfDbContext()
        {
            Database.SetInitializer<EfDbContext>(null);
        }
        public EfDbContext() { }
        public EfDbContext(string connString) : base(connString) { }
        

        public DbSet<Payment> Payments { get; set; }
        public DbSet<PaymentItem> PaymentItems { get; set; }
    }
}