using AapiPeliculas.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AapiPeliculas.Repositorios.IRepository
{
    public interface IUsuarioRepositorio
    {
        ICollection<Usuario> GetUsuarios();
        Usuario GetUsuario(int IdUsuario);
        bool ExistUsuario(string usuario);
        Usuario Registro(Usuario usuario, string password);
        Usuario Login(string usuario, string password);
        bool Guardar();
    }
}
