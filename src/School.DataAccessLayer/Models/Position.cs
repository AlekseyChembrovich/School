using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace School.DataAccessLayer.Models
{
    public class Position
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<TypeDocument> TypesDocuments { get; set; }

        public ICollection<Employee> Employees { get; set; }

        public Position()
        {
            TypesDocuments = new HashSet<TypeDocument>();
            Employees = new HashSet<Employee>();
        }
    }
}
