using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CampIAFCJ.Models
{
    public class TablaUnida
    {
        public Usuarios nombre { get; set; }
        public Usuarios edad { get; set; }
        public Usuarios genero { get; set; }
        public Usuarios estadoCivil { get; set; }
        public Usuarios numIglesia { get; set; }
        public Usuarios correo { get; set; }
        public Usuarios camiseta { get; set; }
        public Usuarios talla { get; set; }
        public Campamentos descripcion { get; set; }
    }
}