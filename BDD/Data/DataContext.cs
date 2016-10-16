using Data.Utilities;
using Domain.Accounting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class DataContext : IDataContext
    {
        //
        // Ctor

        public DataContext(IRepositoryProvider repositoryProvider, EfDbContext dbContext = null)
        {
            if (dbContext == null)
            {
                DbContext = CreateDbContext();
            }
            else
            {
                DbContext = dbContext;
            }


            repositoryProvider.DbContext = DbContext;
            RepositoryProvider = repositoryProvider;
        }

        //
        // Fields

        private EfDbContext DbContext { get; set; }
        protected IRepositoryProvider RepositoryProvider { get; set; }

        public IPaymentRepository Payments
        {
            get { return GetRepo<IPaymentRepository>(); }
        }

        public IRepository<PaymentItem> PaymentItems
        {
            get { return GetStandardRepo<PaymentItem>(); }
        }



        /// <summary>
        ///     Save pending changes to the database
        /// </summary>
        public void SaveChanges()
        {
            DbContext.SaveChanges();
        }

        public Task<int> SaveChangesAsync()
        {
            return DbContext.SaveChangesAsync();
        }

        //
        // Helpers

        private static EfDbContext CreateDbContext()
        {
            var ctx = new EfDbContext("ultra magic connection string");

            // Do NOT enable proxied entities, else serialization fails
            ctx.Configuration.ProxyCreationEnabled = false;

            // Load navigation properties explicitly (avoid serialization trouble)
            ctx.Configuration.LazyLoadingEnabled = false;

            // We are setting the default explicitly so we get better overview
            ctx.Configuration.ValidateOnSaveEnabled = true;

            //DbContext.Configuration.AutoDetectChangesEnabled = false;
            // We won't use this performance tweak because we don't need 
            // the extra performance and, when autodetect is false,
            // we'd have to be careful. We're not being that careful.

            return ctx;
        }

        private IRepository<T> GetStandardRepo<T>() where T : class
        {
            return RepositoryProvider.GetRepositoryForEntityType<T>();
        }

        private T GetRepo<T>() where T : class
        {
            return RepositoryProvider.GetRepository<T>();
        }

        #region IDisposable

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (DbContext != null)
                {
                    DbContext.Dispose();
                }
            }
        }

        #endregion
    }
}
