using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Host.DataModel
{
    public class EmployeeProfileDto
    {
        public string Id { get; set; }
        public int EmployeeProfileId { get; set; }
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleInitial { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public byte? GenderId { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string HomePhone { get; set; }
        public string CellPhone { get; set; }
        public string JobTitle { get; set; }
        public string WorkEmail { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string FkUserId { get; set; }
        public string FkInitiatedById { get; set; }
    }
}
