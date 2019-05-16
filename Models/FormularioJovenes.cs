using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//
using System.ComponentModel.DataAnnotations;

namespace CampIAFCJ.Models
{
    public class FormularioJovenes
    {
        //Solo letras
        [Required]
        [Display(Name = "Nombre Completo")]
        [StringLength(maximumLength: 40, MinimumLength = 5, ErrorMessage = "El nombre debe tener al menos 5 caracteres y máximo 40")]
        //[RegularExpression("^[a-zA-Z ]*$", ErrorMessage ="Solo se permiten letras")]
        [RegularExpression("^[a-zA-ZÀ-ÿ\u00f1\u00d1 ]+([a - zA - ZÀ - ÿ\u00f1\u00d1 ]*)*[a-zA-ZÀ-ÿ\u00f1\u00d1 ]+$", ErrorMessage = "Solo se permiten letras")]
        public string Nombre { get; set; }

        //Solo números enteros positivos
        [Required]
        [Display(Name = "Número de Iglesia")]
        [Range(1, 68, ErrorMessage = "Escoge un número de iglesia válido entre 1 y 68")]
        public int NumeroIglesia { get; set; }

        //Solo números enteros positivos
        [Required]
        [Display(Name = "Edad")]
        [Range(15, 60, ErrorMessage = "La edad debe ser entre 15 y 60")]
        public int Edad { get; set; }

        [Required(ErrorMessage = "Selecciona un género")]
        [Display(Name = "Género")]
        public string Genero { get; set; }

        [Required(ErrorMessage = "Selecciona si quieres o no una camiseta")]
        [Display(Name = "Camiseta")]
        public string Camiseta { get; set; }

        //[Required(ErrorMessage = "Selecciona la talla")]
        [Display(Name = "Talla")]
        public string Talla { get; set; }

        //Formato de correo
        [Required]
        [Display(Name = "Correo Electrónico")]
        [EmailAddress(ErrorMessage = "Ingresa un correo electrónico válido")]
        public string CorreoElectronico { get; set; }
    }
}