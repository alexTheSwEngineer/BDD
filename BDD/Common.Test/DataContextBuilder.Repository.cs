
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
        private static RepositoryFactories RepositoryFactories;
        private readonly IRepositoryProvider _repoProviderImpl;
        private readonly Dictionary<Type, object> _initialEntitiyProviders;
        private readonly Mock<RepositoryFactories> _repoFactoriesMock;
        private readonly Mock<EfDbContext> _efDbContextMock;
        private readonly bool _eagerLoadRepos;

        static DataContextBuilder()
        {
            RepositoryFactories = new RepositoryFactories();
        }

        public DataContextBuilder(bool createAllRepos = false)
        {
            _eagerLoadRepos = createAllRepos;
            _initialEntitiyProviders = new Dictionary<Type, object>();
            _repoFactoriesMock = new Mock<RepositoryFactories>();
            _efDbContextMock = new Mock<EfDbContext>();
            _repoProviderImpl = new RepositoryProvider(RepositoryFactories);
            _repoProviderImpl.DbContext = EfDbContext;
            BuildEfDbContext();
        }

        public EfDbContext EfDbContext
        {
            get
            {
                return _efDbContextMock.Object;
            }
        }

        public IDataContext BuildIDataContext()
        {
            var repoProvider = new RepositoryProvider(_repoFactoriesMock.Object);
            var dataContext = new DataContext(repoProvider, EfDbContext);
            repoProvider.DbContext = EfDbContext;
            return dataContext;
        }

        /// <summary>
        /// Sets up IDatacontext to use <paramref name="repo"/> as the repository for the <typeparamref name="EntityType"/>
        /// Using this method for setting up repos will overide any previous calls to either itself or <see cref="With{EntityType}(IEnumerable{EntityType})"/>
        /// for the same entity type.
        /// </summary>
        /// <typeparam name="RepoType">The most derived type of the repository</typeparam>
        /// <typeparam name="EntityType">The type of the enitty</typeparam>
        /// <param name="repo">The implementation of the repository</param>
        /// <returns></returns>
        public DataContextBuilder WithRepo<RepoType, EntityType>(RepoType repo) where RepoType : class, IRepository<EntityType> where EntityType : class
        {
            Func<DbContext, object> factoryFunction = x => repo;

            _repoFactoriesMock.Setup(x => x.GetRepositoryFactory<RepoType>())
                          .Returns(factoryFunction);
            _repoFactoriesMock.Setup(x => x.GetRepositoryFactoryForEntityType<EntityType>())
                          .Returns(factoryFunction);
            return this;
        }

        /// <summary>
        /// Adds all of <paramref name="newItems"/> to the default in-memory repo for <typeparamref name="EntityType"/>. If such (default in-memory) 
        /// repo doesn't exist it will create one, and set up IDataContext to use it.
        /// Using this method after invoking <see cref="WithRepo{RepoType, EntityType}(RepoType)"/> will result with unexpected behaviour.
        /// </summary>
        public DataContextBuilder With<EntityType>(IEnumerable<EntityType> newItems) where EntityType : class
        {
            var entityType = typeof(EntityType);
            if (_initialEntitiyProviders.ContainsKey(entityType))
            {
                var existing = _initialEntitiyProviders[entityType] as List<EntityType>;
                existing.AddRange(newItems);
            }
            else
            {
                _initialEntitiyProviders.Add(entityType, newItems.ToList());
            }

            return IncludeRepoFor<EntityType>();
        }

        /// <summary>
        /// Creates a default in-memory repo for <typeparamref name="EntityType"/> and sets up IDataContext to use it.
        /// </summary>
        public DataContextBuilder IncludeRepoFor<EntityType>() where EntityType : class
        {

            Type entityType = typeof(EntityType);
            Type baseRepoType = typeof(IRepository<EntityType>);
            Type derivedRepoType = RepositoryFactories.RepoTypes
                                   .SingleOrDefault(x => baseRepoType.IsAssignableFrom(x));
            Type repoType = derivedRepoType ?? baseRepoType;
            return (DataContextBuilder)InvokeGeneric(nameof(IncludeRepo), new[] { repoType, entityType });
        }

        /// <summary>
        /// Creates a specific in-memory implementation of a repo for <typeparamref name="RepoType"/> and sets up IDataContext to use it.
        /// </summary>
        /// <typeparam name="RepoType"></typeparam>
        /// <typeparam name="EntityType"></typeparam>
        /// <returns></returns>
        public DataContextBuilder IncludeRepo<RepoType, EntityType>() where RepoType : class, IRepository<EntityType> where EntityType : class
        {
            var entityType = typeof(EntityType);
            IRepository<EntityType> realImplementation;
            try
            {
                realImplementation = _repoProviderImpl.GetRepository<RepoType>();
            }
            catch (NotImplementedException exc)
            {
                realImplementation = _repoProviderImpl.GetRepositoryForEntityType<EntityType>();
            }

            var repo = InvokeGeneric(nameof(CreateRepoMock), new[] { realImplementation.GetType(), entityType }, null);

            return WithRepo<RepoType, EntityType>(repo as RepoType);
        }



        /// <summary>
        /// Creates a specific implementation of a repository, wired with the in-memoy data context.
        /// </summary>
        /// <typeparam name="RepoType">The thye of the repo implementation to be mocked</typeparam>
        /// <typeparam name="EntityType">The entity type this repo provides</typeparam>
        /// <returns></returns>
        public IRepository<EntityType> CreateRepoMock<RepoType, EntityType>() where RepoType : RepositoryBase<EntityType> where EntityType : class
        {
            var provider = GetEntityProvider(typeof(EntityType)) as List<EntityType>;
            var mock = new Mock<RepoType>();
            mock.CallBase = true;
            mock.Object.SetDbContext(EfDbContext);

            mock.Setup(x => x.Add(It.IsAny<EntityType>()))
                .Callback((EntityType x) => provider.Add(x));
            mock.Setup(x => x.Delete(It.IsAny<EntityType>()))
                .Callback((EntityType x) => provider.Remove(x));
            mock.Setup(x => x.Exists(It.IsAny<Expression<Func<EntityType, bool>>>()))
                .Callback((Expression<Func<EntityType, bool>> x) => provider.AsQueryable().Any(x));
            mock.Setup(x => x.GetAll())
                .Returns(provider.AsQueryable());

            mock.Setup(x => x.DeleteAll())
                .Callback(() => provider.Clear());
            mock.Setup(x => x.DeleteAll(It.IsAny<Expression<Func<EntityType, bool>>>()))
                .Callback((Expression<Func<EntityType, bool>> x) =>
                {
                    var tobeDeleted = provider.AsQueryable().Where(x).ToList();
                    provider.RemoveAll(y => tobeDeleted.Contains(y));
                });

            mock.Setup(x => x.Update(It.IsAny<EntityType>()))
                .Callback(() => { });
            mock.Setup(x => x.Attach(It.IsAny<EntityType>()))
                .Callback((EntityType x) => { });

            return mock.Object as RepoType;
        }

    }
}
