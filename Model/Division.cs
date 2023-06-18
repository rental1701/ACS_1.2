using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ACS.Model
{
    [Table("PDivision")]
    public class Division
    {
        public int ID { get; set; }
        public string Name { get; set; } = null!;
   
        public List<Person>? Persons { get; set; } = null!;
    }
}
