using Demo.BLL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Interfaces
{
    public interface IUnitOfWork
    {
        public IEmployeeRepository Employees { get;}
        public IDepartmentRepository Departments { get;}

        public Task<int> CompleteAsync();


    }
}
