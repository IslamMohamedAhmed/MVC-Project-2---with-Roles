using Demo.BLL.Interfaces;
using Demo.DAL.Contexts;
using Demo.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Repositories
{
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(DataContext dataContext) : base(dataContext)
        {
        }

        public  async Task<IEnumerable<Employee>> GetAllByNameAsync(string? name)
        {
            return await dataContext.Employees.Where(w => w.Name.ToLower().Contains(name.ToLower())).Include(w => w.department).ToListAsync();      
        }

        public async Task< IEnumerable<Employee>> GetAllWithDepartmentAsync()
        {
            return await dataContext.Employees.Include(e => e.department).ToListAsync();
        }

        
    }
}
