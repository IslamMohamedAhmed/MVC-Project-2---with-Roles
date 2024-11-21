using Demo.BLL.Interfaces;
using Demo.DAL.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly Lazy<IEmployeeRepository> employeeRepository;
        private readonly Lazy<IDepartmentRepository> departmentRepository;
        private readonly DataContext dataContext;

        public UnitOfWork(DataContext dataContext) {
            employeeRepository = new Lazy<IEmployeeRepository>(()=>new EmployeeRepository(dataContext));
            departmentRepository = new Lazy<IDepartmentRepository>(()=>new DepartmentRepository(dataContext));
            this.dataContext = dataContext;
        }
        public IEmployeeRepository Employees => employeeRepository.Value;

        public IDepartmentRepository Departments => departmentRepository.Value; 

        public async Task<int> CompleteAsync()
        {
            return await dataContext.SaveChangesAsync();
        }

    
    }
}
