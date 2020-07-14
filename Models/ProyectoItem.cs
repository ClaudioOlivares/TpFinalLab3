using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TpFinalLab3.Models
{
    public class ProyectoItem
    {
        [Key]
        public int IdProyectoItem { get; set; }
        public int IdProyecto { get; set; }
        [ForeignKey("IdProyecto")]
        public Proyecto Proyecto { get; set; }
        public String TextoCompleto { get; set; }
        public String TextoResumen { get; set; }
        public String Video { get; set; }
    }
}
