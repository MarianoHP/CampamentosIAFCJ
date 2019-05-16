using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//hacer anotaciones a las variables
using System.ComponentModel.DataAnnotations;

namespace CampIAFCJ.Models
{
    public class Login
    {
        [Required(ErrorMessage ="Por favor llena el campo de Usuario")]
        [Display(Name="Usuario:")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Por favor llena el campo de Contraseña")]
        [Display(Name = "Contraseña:")]
        [DataType(DataType.Password)]
        public string UserPassword { get; set; }

    }
}