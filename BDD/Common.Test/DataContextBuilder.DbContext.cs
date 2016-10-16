
using Data;
using Data.Utilities;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ClinicHQ.Tests.Common
{
    public partial class DataContextBuilder
    {
        /// <summary>
        /// Creates a mock implementation for Set<T> function for the given tytpe t.
        /// </summary>
        /// <typeparam name="T">The entity type</typeparam>
        /// <param name="context">The mock of the efdbcontext</param>
        /// <param name="providerListAsObject">The provider list for the entity</param>
        public static void MockDbSet<EntityType>(Mock<EfDbContext> context, object providerListAsObject) where EntityType : class
        {
            var provider = providerListAsObject as ICollection<EntityType>;
            var queryable = provider.AsQueryable();

            var dbSet = new Mock<DbSet<EntityType>>();
            dbSet.As<IQueryable<EntityType>>().Setup(m => m.Provider).Returns(queryable.Provider);
            dbSet.As<IQueryable<EntityType>>().Setup(m => m.Expression).Returns(queryable.Expression);
            dbSet.As<IQueryable<EntityType>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            dbSet.As<IQueryable<EntityType>>().Setup(m => m.GetEnumerator()).Returns(queryable.GetEnumerator());
            dbSet.Setup(x => x.Attach(It.IsAny<EntityType>()))
                 .Returns((EntityType x) => x);

            dbSet.Setup(x => x.Add(It.IsAny<EntityType>()))
                 .Callback((EntityType a) => provider.Add(a));

            dbSet.Setup(x => x.Remove(It.IsAny<EntityType>()))
                 .Callback((EntityType a) => provider.Remove(a));

            dbSet.Setup(x => x.RemoveRange(It.IsAny<IEnumerable<EntityType>>()))
                 .Callback((IEnumerable<EntityType> a) => a.ToList().ForEach(x => provider.Remove(x)));

            dbSet.Setup(x => x.AddRange(It.IsAny<IEnumerable<EntityType>>()))
                 .Callback((IEnumerable<EntityType> a) => 
                 {
                     foreach (var item in a)
                     {
                         provider.Add(item);
                     }
                 });

            context.Setup(x => x.Set<EntityType>())
                   .Returns(dbSet.Object);
        }

        /// <summary>
        /// Gets or creates an in-memory provider for a set of the given entity type.
        /// </summary>
        /// <param name="entityType"></param>
        /// <returns></returns>
        private object GetEntityProvider(Type entityType)
        {
            if (_initialEntitiyProviders.ContainsKey(entityType))
            {
                return _initialEntitiyProviders[entityType];
            }

            var listType = typeof(List<>);
            var genericArgs = entityType;
            var concreteType = listType.MakeGenericType(genericArgs);
            var provider = Activator.CreateInstance(concreteType);
            _initialEntitiyProviders.Add(entityType, provider);
            return provider;
        }

        /// <summary>
        /// Mocks implementation of Set<T>() methods for each dbSet in the efdbcontext. Creates and sets up default repos
        /// for IDataContext if <see cref="_eagerLoadRepos"/> is set to true.
        /// </summary>
        private void BuildEfDbContext()
        {
            foreach (var prop in typeof(EfDbContext).GetProperties())
            {
                Type dbSetType = prop.PropertyType;
                if (!dbSetType.IsGenericType || dbSetType.GetGenericTypeDefinition() != typeof(DbSet<>))
                {
                    continue;
                }

                var entityType = dbSetType.GenericTypeArguments[0];
                var initialState = GetEntityProvider(entityType);
                InvokeGeneric(null, nameof(MockDbSet), new[] { entityType }, new object[] { _efDbContextMock, initialState });

                if (_eagerLoadRepos)
                {
                    InvokeGeneric(nameof(IncludeRepoFor), new[] { entityType });
                }
            }

            _efDbContextMock.Setup(x => x.SaveChanges())
                       .Returns(0);
            _efDbContextMock.Setup(x => x.SaveChangesAsync())
                       .Returns(Task.FromResult(0));
        }
    }
}
