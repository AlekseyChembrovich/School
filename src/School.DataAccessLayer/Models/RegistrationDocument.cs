using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace School.DataAccessLayer.Models
{
    public class RegistrationDocument
    {
        [Key]
        public int Id { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime StartDate { get; set; }

        public string DirectionDocument { get; set; }

        public int TypeDocumentId { get; set; }

        public int EmployeeCreatorId { get; set; }

        public int EmployeeApproverId { get; set; }

        public virtual TypeDocument TypeDocument { get; set; }

        [ForeignKey("EmployeeCreatorId")]
        public virtual Employee EmployeeCreator { get; set; }

        [ForeignKey("EmployeeApproverId")]
        public virtual Employee EmployeeApprover { get; set; }
    }
}
