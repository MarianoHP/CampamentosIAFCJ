using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CampIAFCJ.Models
{
    public class Campamentos
    {
        [Key]
        [Required]
        public int ID_Campamento { get; set; }
        [Required]
        public string Descripcion { get; set; }
        [Required]
        public string Lugar { get; set; }
        [Required]
        public DateTime Fecha { get; set; }
    }
}