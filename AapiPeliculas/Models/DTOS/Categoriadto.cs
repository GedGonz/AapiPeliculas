using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AapiPeliculas.Models.DTOS
{
    public class Categoriadto
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="El Nombre es requerido")]
        public String Nombre { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}
