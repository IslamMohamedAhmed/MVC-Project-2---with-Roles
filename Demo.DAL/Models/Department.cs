using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DAL.Models
{
    public class Department
    {
        public int Id { get; set; }

        public int Code { get; set; }

        public string Name { get; set; }


        public DateTime DateOfCreation { get; set; }

        
        public ICollection<Employee>? employees { get; set; } = new HashSet<Employee>();
    }
}
