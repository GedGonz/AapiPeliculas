using AapiPeliculas.Data;
using AapiPeliculas.Models;
using AapiPeliculas.Repositorios.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AapiPeliculas.Repositorios
{
    public class UsuarioRepositorio: IUsuarioRepositorio
    {
        private readonly AplicationDbContext _db;
        public UsuarioRepositorio(AplicationDbContext _db)
        {
            this._db = _db;
        }

        public bool ExistUsuario(string usuario)
        {
            bool valor = false;
            try
            {
                valor = _db.Usuario.Any(c => c.UsuarioA.ToLower().Trim() == usuario.ToLower().Trim());
            }
            catch (Exception)
            {

                throw;
            }

            return valor;
        }

        public Usuario GetUsuario(int IdUsuario)
        {
            var usuario = new Usuario();
            try
            {
                usuario = _db.Usuario.FirstOrDefault(x=>x.Id==IdUsuario);
            }
            catch (Exception)
            {

                throw;
            }

            return usuario;
        }

        public ICollection<Usuario> GetUsuarios()
        {
            var usuarios = new List<Usuario>();
            try
            {
                usuarios = _db.Usuario.OrderBy(c=>c.UsuarioA).ToList();
            }
            catch (Exception)
            {

                throw;
            }

            return usuarios;
        }

        public bool Guardar()
        {
            bool retorno;
            try
            {
                retorno = _db.SaveChanges() >= 0;
            }
            catch (Exception)
            {

                throw;
            }

            return retorno;
        }

        public Usuario Login(string usuario, string password)
        {
            throw new NotImplementedException();
        }

        public Usuario Registro(Usuario usuario, string password)
        {
            throw new NotImplementedException();
        }
    }
}
