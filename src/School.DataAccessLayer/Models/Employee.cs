using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace School.DataAccessLayer.Models
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }

        public string Surname { get; set; }

        public string Name { get; set; }

        public string Patronymic { get; set; }

        public string Phone { get; set; }

        public int PositionId { get; set; }

        public virtual Position Position { get; set; }

        public ICollection<RegistrationDocument> RegistrationDocuments1 { get; set; }

        public ICollection<RegistrationDocument> RegistrationDocuments2 { get; set; }

        public Employee()
        {
            RegistrationDocuments1 = new HashSet<RegistrationDocument>();
            RegistrationDocuments2 = new HashSet<RegistrationDocument>();
        }
    }
}
