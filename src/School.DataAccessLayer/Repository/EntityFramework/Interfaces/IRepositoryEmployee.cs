using System.Collections.Generic;
using School.DataAccessLayer.Models;

namespace School.DataAccessLayer.Repository.EntityFramework.Interfaces
{
    public interface IRepositoryEmployee : IRepository<Employee>
    {
        IEnumerable<Employee> GetAllIncludeForeignKey();
    }
}
