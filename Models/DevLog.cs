using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TpFinalLab3.Models
{
    public class DevLog
    {
        [Key]
        public int IdDevLog { get; set; }
        public int IdProyecto { get; set; }    
        public string Titulo{ get; set; }
        public string Resumen { get; set; }
        public DateTime FechaCreacion { get; set; }


    }
}
