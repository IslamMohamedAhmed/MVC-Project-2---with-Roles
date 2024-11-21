using Demo.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task CreateAsync(T Entity);
        void Delete(T Entity);
        Task<T>? GetAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        void Update(T Entity);
    }
}
