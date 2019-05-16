using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace CampIAFCJ.Models
{
    public class CampamentoContext : DbContext
    {
        public CampamentoContext()
            : base("name=DefaultConnection")
        {
        }
        public virtual DbSet<Campamentos> Campamento { get; set; }
        public virtual DbSet<Usuarios> Usuario { get; set; }
    }
}