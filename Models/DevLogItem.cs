using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TpFinalLab3.Models
{
    public class DevLogItem
    {
        [Key]
        public int IdDevLogItem { get; set; }
        public int IdDevLog { get; set; }
        [ForeignKey("IdDevLog")]
        public DevLog DevLog { get; set; }
        public string Titulo { get; set; }
        public string Texto { get; set; }
        public string Multimedia { get; set; }

    }
}
