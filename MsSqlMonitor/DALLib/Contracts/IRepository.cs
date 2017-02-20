using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DALLib.Contracts
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        Task<IEnumerable<T>> GetAllAsync();

        T Get(int id);
        Task<T> GetAsync(int id);

        IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate);
        Task<IEnumerable<T>> FindByAsync(Expression<Func<T, bool>> predicate);

        T Create(T item);

        T Update(T item);

        T Delete(int id);

        void DeleteAll();
        void DeleteAllAsync();
    }
}
