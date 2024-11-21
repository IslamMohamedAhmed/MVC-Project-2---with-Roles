using Demo.DAL.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Demo.PL.Models
{
    public class EmployeeViewModel
    {
        public int Id { get; set; }
        [StringLength(maximumLength: 50, MinimumLength = 3)]
        public string Name { get; set; }
        [Range(19, 60)]
        public int Age { get; set; }
        public string Address { get; set; }
        [DataType(DataType.Currency)]
        public decimal Salary { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [Phone]
        public string PhoneNumber { get; set; }

        public string? ImageName { get; set; }

        [DisplayName("Employee Image")]
        public IFormFile? Image { get; set; }

        [DisplayName("Status")]
        public bool IsActive { get; set; }

        [ForeignKey("department")]
        public int? departmentId { get; set; }
        [InverseProperty("employees")]
        public Department? department { get; set; }
    }
}
