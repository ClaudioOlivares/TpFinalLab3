using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TpFinalLab3.Models
{
    public class ProyectoItem
    {
        [Key]
        public int IdProyectoItem { get; set; }
        public int IdProyecto { get; set; }
        public int Titulo { get; set; }
        public int Texto { get; set; }
        public int Multimedia { get; set; }
    }
}
