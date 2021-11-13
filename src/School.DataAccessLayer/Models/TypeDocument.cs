using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace School.DataAccessLayer.Models
{
    public class TypeDocument
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public int PositionId { get; set; }

        public virtual Position Position { get; set; }

        public ICollection<RegistrationDocument> RegistrationDocuments { get; set; }

        public TypeDocument()
        {
            RegistrationDocuments = new HashSet<RegistrationDocument>();
        }
    }
}
