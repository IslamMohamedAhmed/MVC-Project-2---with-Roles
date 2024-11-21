using Demo.DAL.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Demo.PL.Models
{
    public class DepartmentViewModel
    {
        public int Id { get; set; }
        [Range(0, 500)]
        public int Code { get; set; }
        [Required(ErrorMessage = "Name is Required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Date of creation is Required")]
        [DisplayName("Created At")]
        public DateTime DateOfCreation { get; set; }

        [InverseProperty("department")]
        public ICollection<Employee>? employees { get; set; } = new HashSet<Employee>();
    }
}
