using System.Collections.Generic;
using School.DataAccessLayer.Models;

namespace School.DataAccessLayer.Repository.EntityFramework.Interfaces
{
    public interface IRepositoryRegistrationDocument : IRepository<RegistrationDocument>
    {
        IEnumerable<RegistrationDocument> GetAllIncludeForeignKey();
    }
}
