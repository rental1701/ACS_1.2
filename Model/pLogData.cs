using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACS.Model
{
    [Table("pLogData")]
    class pLogData
    {
        public DateTime TimeVal { get; set; }

        public int HozOrgan { get; set; }
        public string? DivisionName { get; set; }
        public int Event { get; set; } 
        public int Mode { get; set; }
        [NotMapped]
        public string LastName { get; set; } = null!;
        public string Remark { get; set; } = null!;
    }
}
