using System;
using System.Linq;
using System.Linq.Expressions;
using Customer.Domain;
using Microsoft.EntityFrameworkCore;

namespace Customer.Repository
{
    public abstract class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : Entity
    {
        private readonly CustomerDbContext _dbContext;
        private readonly DbSet<TEntity> _dbSet;

        protected GenericRepository(CustomerDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<TEntity>();
        }

        public Guid Save(TEntity entity)
        {
            entity.Id = Guid.NewGuid();
            _dbContext.Add(entity);
            _dbContext.SaveChanges();

            return entity.Id;
        }

        public TEntity Get(Guid id)
        {
            return _dbSet.Find(id);
        }

        public void Update(TEntity entity)
        {
            _dbSet.Attach(entity);
            _dbContext.Entry(entity).State = EntityState.Modified;
            _dbContext.SaveChanges();
        }

        public void Delete(Guid id)
        {
            var entity = Get(id);
            _dbSet.Remove(entity);
            _dbContext.SaveChanges();
        }

        public IQueryable<TEntity> All()
        {
            return _dbSet.AsNoTracking();
        }

        public IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return _dbSet.Where(predicate);
        }
    }
}