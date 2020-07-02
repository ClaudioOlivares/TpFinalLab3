﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TpFinalLab3.Models
{
    public class Proyecto
    {
        [Key]
        public int IdProyecto { get; set; }
        public string Titulo { get; set; }    
        public string Genero { get; set; }
        public string Status { get; set; }
        public string Plataforma { get; set; }
        public DateTime FechaCreacion { get; set; }
        public String  Portada { get; set; }
        [NotMapped]
        public IFormFile PortadaFile { get; set; }
        public int IdUser { get; set; }
        [ForeignKey("IdUser")]
        public Usuario User { get; set; }

    }
}