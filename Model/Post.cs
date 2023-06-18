using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace ACS.Model
{
    [Table("PPost")]
    public class Post
    {
        public int ID { get; set; }
        public string Name { get; set; } = null!;

        public List<Person>? Persons { get; set; }
    }
}
