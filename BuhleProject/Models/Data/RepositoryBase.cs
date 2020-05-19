using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace BuhleProject.Models.Data
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        public AppDBcontext _appDbContext;
        public RepositoryBase(AppDBcontext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public void Create(T entity)
        {
            _appDbContext.Set<T>().Add(entity);
        }

        public void Delete(T entity)
        {
            _appDbContext.Set<T>().Remove(entity);
        }

        public IEnumerable<T> FindAll()
        {
            return _appDbContext.Set<T>();
        }

        public IEnumerable<T> FindByCondition(Expression<Func<T, bool>> expression)
        {

            return _appDbContext.Set<T>().Where(expression);
        }

        public T GetById(int id)
        {
            return _appDbContext.Set<T>().Find(id);
        }

        public void Save()
        {
            _appDbContext.SaveChanges();
        }

        public void Update(T entity)
        {
            _appDbContext.Set<T>().Update(entity);
        }

    }
}

