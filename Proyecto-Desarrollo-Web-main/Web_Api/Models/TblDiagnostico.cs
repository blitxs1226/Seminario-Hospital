using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Web_Api.Models
{
    public partial class TblDiagnostico
    {
        [Key]
        public int IdDiagnostico { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }

    }
}
