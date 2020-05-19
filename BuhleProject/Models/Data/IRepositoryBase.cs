using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace BuhleProject.Models.Data
{
    public interface IRepositoryBase<T>
    {
        T GetById(int id);
        IEnumerable<T> FindAll();
        IEnumerable<T> FindByCondition(Expression<Func<T, bool>> expression);
        void Create(T entity);
        void Update(T entity);
        void Delete(T entity);
        void Save();
    }
}
