using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AapiPeliculas.Models.DTOS
{
    public class UsuarioAuthLogindto
    {
        [Required(ErrorMessage = "El usuario es requerido")]
        public string Usuario { get; set; }
        [Required(ErrorMessage = "El password es requerido")]
        public string Passwrod { get; set; }
    }
}
