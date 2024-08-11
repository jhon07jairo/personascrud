using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PersonasCrud.Models
{
    public partial class Persona
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string? Apellidos { get; set; }
        public int Documento { get; set; }

        [Display(Name= "Fecha de Nacimiento")]
        //[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FechaNacimiento { get; set; }
        public string Sexo { get; set; } = null!;

        [Display(Name = "Pais")]
        public string? CodigoPais { get; set; }

        [Display(Name = "Pais")]
        public virtual Pai? CodigoPaisNavigation { get; set; }
    }
}
