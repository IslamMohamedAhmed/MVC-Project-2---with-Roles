using Demo.BLL.Interfaces;
using Demo.DAL.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly DataContext dataContext;

        public GenericRepository(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }
        public async Task CreateAsync(T Entity)
        {
            await dataContext.Set<T>().AddAsync(Entity);
        }

        public void Delete(T Entity)
        {
            dataContext.Set<T>().Remove(Entity);
            
        }

        public async Task<T>? GetAsync(int id)
        {
            return await dataContext.Set<T>().FindAsync(id);
            
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await dataContext.Set<T>().ToListAsync();
        }

        public void Update(T Entity)
        {
            dataContext.Set<T>().Update(Entity);
            
        }
    }
}
