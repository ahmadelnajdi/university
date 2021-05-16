using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace University.Areas.Identity.Data
{
    public class StudentData
    {

        [Column(TypeName = "int")]
        public int id { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public String Studentid { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string Faculty { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string Course { get; set; }

        [Column(TypeName = "int")]
        public int Note { get; set; }


      
        

    }
}
