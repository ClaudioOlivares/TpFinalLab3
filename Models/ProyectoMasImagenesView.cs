using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TpFinalLab3.Models
{
    public class ProyectoMasImagenesView
    {

        [Key]
        public int IdProyecto { get; set; }
        public string Titulo { get; set; }
        public string Genero { get; set; }
        public string Status { get; set; }
        public string Plataforma { get; set; }
        public DateTime FechaCreacion { get; set; }
        public String Portada { get; set; }
        [NotMapped]
        public IFormFile PortadaFile { get; set; }
        public String Video { get; set; }
        [NotMapped]
        public IFormFile VideoFile { get; set; }
        public int IdUser { get; set; }
        [ForeignKey("IdUser")]
        public Usuario User { get; set; }
        public String TextoResumen { get; set; }
        public String TextoCompleto { get; set; }
        public String VideoTrailer { get; set; }
        public List<ImagenProyecto> imagenes { get; set; }

    }
}
