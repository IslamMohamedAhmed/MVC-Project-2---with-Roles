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

    public class DepartmentRepository : GenericRepository<Department>, IDepartmentRepository
    {
        public DepartmentRepository(DataContext dataContext) : base(dataContext)
        {
        }

        public async Task<IEnumerable<Department>> GetDepartmentWithNameAsync(string? Name)
        {
            return await dataContext.Departments.Where(w => w.Name.ToLower().Contains(Name.ToLower())).ToListAsync();
        }
    }
}
