using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TpFinalLab3.Models
{
    public class Usuario
    {
        [Key]
        public int IdUser { get; set; }
        public string Nick { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public string Nacionalidad { get; set; }
        public string Clave { get; set; }

    }
}
