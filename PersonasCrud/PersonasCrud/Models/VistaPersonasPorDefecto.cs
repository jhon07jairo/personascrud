using System;
using System.Collections.Generic;

namespace PersonasCrud.Models
{
    public partial class VistaPersonasPorDefecto
    {
        public string Nombre { get; set; } = null!;
        public string? Apellidos { get; set; }
        public int Documento { get; set; }
        public string Sexo { get; set; } = null!;
        public string? FechaNacimiento { get; set; }
    }
}
