using System.Collections.Generic;
using System.Linq;
using DALLib.EF;
using System.Data.Entity;
using DALLib.Contracts;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DALLib.Repos
{
    public abstract class BaseRepository<T> : IRepository<T> where T : class
    {
        protected MsSqlMonitorEntities context;
        protected DbSet<T> table;

        public BaseRepository(MsSqlMonitorEntities context)
        {
            this.context = context;
            table = context.Set<T>();
        }

        public virtual IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate)
        {

            return table.Where(predicate);
        }

        public virtual async Task<IEnumerable<T>> FindByAsync(Expression<Func<T, bool>> predicate)
        {
            return await table.Where(predicate).ToListAsync();
        }

        public virtual T Create(T item)
        {
            return table.Add(item);
        }

        public virtual T Delete(int id)
        {
            T item = table.Find(id);
            return table.Remove(item);
        }

        public virtual async Task<T> DeleteAsync(int id)
        {
            T item = await table.FindAsync(id);
            return table.Remove(item);
        }

        public virtual async void DeleteAllAsync()
        {
            await table.ForEachAsync(item => table.Remove(item));
        }

        public virtual void DeleteAll()
        {
            foreach (var item in table)
            {
                table.Remove(item);
            }
        }

        public virtual T Get(int id)
        {
            return table.Find(id);
        }

        public virtual Task<T> GetAsync(int id)
        {
            return table.FindAsync(id);
        }

        public virtual IEnumerable<T> GetAll()
        {
            return table;
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await table.ToListAsync();
        }

        public virtual T Update(T item)
        {
            context.Entry(item).State = EntityState.Modified;
            return item;
        }
    }
}
