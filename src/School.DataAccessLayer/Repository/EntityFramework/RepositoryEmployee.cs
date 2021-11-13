using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using School.DataAccessLayer.Models;
using School.DataAccessLayer.Repository.EntityFramework.Interfaces;

namespace School.DataAccessLayer.Repository.EntityFramework
{
    public class RepositoryEmployee : BaseRepository<Employee>, IRepositoryEmployee
    {
        public RepositoryEmployee(DatabaseContext context) : base(context)
        {
        }

        public IEnumerable<Employee> GetAllIncludeForeignKey()
        {
            return Context.Employee.Include(x => x.Position);
        }
    }
}
