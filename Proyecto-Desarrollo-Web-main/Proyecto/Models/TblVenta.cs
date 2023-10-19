using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class TblVenta
    {     

        public int IdVentas { get; set; }
        [Required]
        public string Serie { get; set; }
        [Required]
        public int? Numero { get; set; }
        public DateTime? Fecha { get; set; }
        [Required(ErrorMessage = "Se Requiere Seleccionar a un paciente Imbecil")]
        public int? IdPaciente { get; set; }
    }
    public class VentaViewModel
    {

        public int IdVentas { get; set; }
        public string Serie { get; set; }
        public int? Numero { get; set; }
        public DateTime? Fecha { get; set; }
        public int? IdPaciente { get; set; }
        public string Paciente { get; set; }
    }
}
