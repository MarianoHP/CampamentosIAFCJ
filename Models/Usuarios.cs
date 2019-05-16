using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CampIAFCJ.Models
{
    public class Usuarios
    {
        [Key]
        [Required]
        public int ID_Usuario { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Required]
        public int Edad { get; set; }
        [Required]
        public string Genero { get; set; }
        [Required]
        public string EstadoCivil { get; set; }
        [Required]
        public int NumIglesia { get; set; }
        [Required]
        public string Correo { get; set; }
        [Required]
        public string Camiseta { get; set; }
        [Required]
        public string Talla { get; set; }
        //[Required]
        public int id_campa { get; set; }
    }
}