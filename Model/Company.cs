using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACS.Model
{
    [Table("PCompany")]
    public class Company
    {
        public int ID { get; set; }
        public string Name { get; set; } = null!;
        public List<Person>? Persons { get; set; } = null!;
    }
}
