using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab4.Models
{
    class User
    {
        public int id { get; set; }
        public string? EMail { get; set; }
        public string? Password { get; set; }

    }
}
