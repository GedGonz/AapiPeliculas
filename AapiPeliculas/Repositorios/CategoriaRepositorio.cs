using AapiPeliculas.Data;
using AapiPeliculas.Models;
using AapiPeliculas.Repositorios.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AapiPeliculas.Repositorios
{
    public class CategoriaRepositorio : ICategoriaRepositorio
    {
        private readonly AplicationDbContext _db;
        public CategoriaRepositorio(AplicationDbContext _db)
        {
            this._db = _db;
        }
        public bool ActualizarCategoria(Categoria categoria)
        {
            try
            {
                _db.Categoria.Update(categoria);
                return Guardar();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool CrearCategoria(Categoria categoria)
        {
            try
            {
                _db.Categoria.Add(categoria);
                return Guardar();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool EliminarCategoria(Categoria categoria)
        {
            try
            {
                _db.Categoria.Remove(categoria);
                return Guardar();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool ExistCategoria(string nombre)
        {
            bool valor = false;
            try
            {
                valor = _db.Categoria.Any(c => c.Nombre.ToLower().Trim() == nombre.ToLower().Trim());
            }
            catch (Exception)
            {

                throw;
            }
           
            return valor;
        }

        public bool ExistCategoria(int IdCategoria)
        {
            bool valor = false;
            try
            {
                valor = _db.Categoria.Any(x => x.Id == IdCategoria);
            }
            catch (Exception)
            {

                throw;
            }

            return valor;
        }

        public Categoria GetCategoria(int IdCategoria)
        {
            var categoria = new Categoria();
            try
            {
                categoria = _db.Categoria.FirstOrDefault(x => x.Id == IdCategoria);
            }
            catch (Exception)
            {

                throw;
            }

            return categoria;
        }

        public ICollection<Categoria> GetCategorias()
        {
            var categorias = new List<Categoria>();
            try
            {
                categorias = _db.Categoria.OrderBy(x=>x.Nombre).ToList();
            }
            catch (Exception)
            {

                throw;
            }

            return categorias;
        }

        public bool Guardar()
        {
            var retorno = false;
            try
            {
                retorno=_db.SaveChanges()>=0;
            }
            catch (Exception)
            {

                throw;
            }
            
            return retorno;
        }
    }
}
