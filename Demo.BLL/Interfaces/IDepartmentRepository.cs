using Demo.BLL.Interfaces;
using Demo.DAL.Models;

namespace Demo.BLL.Repositories
{
    public interface IDepartmentRepository : IGenericRepository<Department>
    {
       Task<IEnumerable<Department>> GetDepartmentWithNameAsync(string? Name);
    }
}