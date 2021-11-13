using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using School.DataAccessLayer.Models;
using School.DataAccessLayer.Repository.EntityFramework.Interfaces;

namespace School.DataAccessLayer.Repository.EntityFramework
{
    public class RepositoryRegistrationDocument : BaseRepository<RegistrationDocument>, IRepositoryRegistrationDocument
    {
        public RepositoryRegistrationDocument(DatabaseContext context) : base(context)
        {
        }

        public IEnumerable<RegistrationDocument> GetAllIncludeForeignKey()
        {
            return Context.RegistrationDocument
                .Include(x => x.TypeDocument)
                .Include(x => x.EmployeeCreator)
                .Include(x => x.EmployeeApprover);
        }
    }
}
