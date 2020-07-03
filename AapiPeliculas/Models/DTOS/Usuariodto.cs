using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AapiPeliculas.Models.DTOS
{
    public class Usuariodto
    {
        public string UsuarioA { get; set; }
        public byte[] PasswordHash { get; set; }

    }
}
