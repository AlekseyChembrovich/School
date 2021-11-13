using System.Collections.Generic;
using School.DataAccessLayer.Models;

namespace School.DataAccessLayer.Repository.EntityFramework.Interfaces
{
    public interface IRepositoryTypeDocument : IRepository<TypeDocument>
    {
        IEnumerable<TypeDocument> GetAllIncludeForeignKey();
    }
}
