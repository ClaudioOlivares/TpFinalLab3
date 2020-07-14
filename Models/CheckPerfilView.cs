using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TpFinalLab3.Models
{
    public class CheckPerfilView
    {
        [Key]
        public String Email { get; set; }
    }
}
