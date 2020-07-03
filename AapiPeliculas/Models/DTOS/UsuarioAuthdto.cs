using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AapiPeliculas.Models.DTOS
{
    public class UsuarioAuthdto
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="El usuario es requerido")]
        public string Usuario { get; set; }
        [Required(ErrorMessage = "El password es requerido")]
        [StringLength(10, MinimumLength =4, ErrorMessage ="La contraseña debe estar entre 4 y 10 caracteres")]
        public string Passwrod { get; set; }

    }
}
