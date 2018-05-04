using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Host.DataContext
{
    public partial class EmployeeProfile
    {
        [Key]
        public int PkEmployeeProfileId { get; set; }
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }
        [StringLength(50)]
        public string LastName { get; set; }
        [StringLength(50)]
        public string MiddleInitial { get; set; }
        [StringLength(100)]
        public string StreetAddress { get; set; }
        [StringLength(50)]
        public string City { get; set; }
        [StringLength(50)]
        public string State { get; set; }
        [StringLength(50)]
        public string ZipCode { get; set; }
        public byte? FkGenderId { get; set; }
        [Column(TypeName = "date")]
        public DateTime? DateOfBirth { get; set; }
        [StringLength(20)]
        public string HomePhone { get; set; }
        [StringLength(20)]
        public string CellPhone { get; set; }
        [StringLength(20)]
        public string JobTitle { get; set; }
        [Required]
        [StringLength(50)]
        public string WorkEmail { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedOn { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedOn { get; set; }
        [StringLength(450)]
        public string FkUserId { get; set; }
        [Required]
        [StringLength(450)]
        public string FkInitiatedById { get; set; }

        [ForeignKey("FkGenderId")]
        [InverseProperty("EmployeeProfile")]
        public Gender FkGender { get; set; }
        [ForeignKey("FkInitiatedById")]
        [InverseProperty("EmployeeProfileFkInitiatedBy")]
        public AspNetUsers FkInitiatedBy { get; set; }
        [ForeignKey("FkUserId")]
        [InverseProperty("EmployeeProfileFkUser")]
        public AspNetUsers FkUser { get; set; }
    }
}
