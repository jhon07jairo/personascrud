using System;
using System.Collections.Generic;

namespace PersonasCrud.Models
{
    public partial class Pai
    {
        public Pai()
        {
            Personas = new HashSet<Persona>();
        }

        public string CodigoPais { get; set; } = null!;
        public string NombrePais { get; set; } = null!;
        public bool PorDefecto { get; set; }

        public virtual ICollection<Persona> Personas { get; set; }
    }
}
