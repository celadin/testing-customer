using System;
using System.Linq;
using System.Linq.Expressions;

namespace Customer.Domain
{
    public interface IGenericRepository<TEntity> where TEntity : Entity
    {
        Guid Save(TEntity entity);
        TEntity Get(Guid id);
        void Update(TEntity entity);
        void Delete(Guid id);
        IQueryable<TEntity> All();
        IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);
    }
}