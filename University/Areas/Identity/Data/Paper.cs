using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace University.Areas.Identity.Data
{
    public class Paper
    {
        [Column(TypeName = "int")]
        public int id { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string Firstname { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string lastname { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string StudentID { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string Year { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string type { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public int Copy { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public String Status { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public String Email { get; set; }

    }
}
