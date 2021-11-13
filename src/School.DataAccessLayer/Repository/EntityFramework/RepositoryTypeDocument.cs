using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using School.DataAccessLayer.Models;
using School.DataAccessLayer.Repository.EntityFramework.Interfaces;

namespace School.DataAccessLayer.Repository.EntityFramework
{
    public class RepositoryTypeDocument : BaseRepository<TypeDocument>, IRepositoryTypeDocument
    {
        public RepositoryTypeDocument(DatabaseContext context) : base(context)
        {
        }

        public IEnumerable<TypeDocument> GetAllIncludeForeignKey()
        {
            return Context.TypeDocument.Include(x => x.Position);
        }
    }
}
