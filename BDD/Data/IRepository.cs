#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

#endregion

namespace Data
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> GetAll();
        T GetById(int id);
        void Add(T entity);
        void Attach(T entity);
        void Update(T entity);
        void Delete(T entity);
        void Delete(int id);
        void DeleteAll(List<T> entities);
        void DeleteAll(Expression<Func<T, bool>> where);
        void DeleteAll();
        bool Exists(Expression<Func<T, bool>> where);
    }
}