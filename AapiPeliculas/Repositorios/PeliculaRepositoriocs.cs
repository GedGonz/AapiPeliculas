using AapiPeliculas.Data;
using AapiPeliculas.Models;
using AapiPeliculas.Repositorios.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AapiPeliculas.Repositorios
{
    public class PeliculaRepositoriocs : IPeliculaRepositoriocs
    {
        private readonly AplicationDbContext _db;

        public PeliculaRepositoriocs(AplicationDbContext _db)
        {
            this._db = _db;
        }
        public bool ActualizarPelicula(Pelicula pelicula)
        {
            try
            {
                _db.Pelicula.Update(pelicula);
                return Guardar();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public IEnumerable<Pelicula> BuscarPelicula(string nombre)
        {
            IQueryable<Pelicula> query = _db.Pelicula;
            try
            {
                if (!string.IsNullOrEmpty(nombre)) 
                {
                    query = query.Where(e => e.Nombre.Contains(nombre) || e.Descripcion.Contains(nombre));
                }
            }
            catch (Exception)
            {

                throw;
            }

            return query.ToList();
        }

        public bool CrearPelicula(Pelicula pelicula)
        {
            try
            {
                _db.Pelicula.Add(pelicula);
                return Guardar();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool EliminarPelicula(Pelicula pelicula)
        {
            try
            {
                _db.Pelicula.Remove(pelicula);
                return Guardar();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool ExistPelicula(string nombre)
        {
            bool valor = false;
            try
            {
                valor = _db.Pelicula.Any(c => c.Nombre.ToLower().Trim() == nombre.ToLower().Trim());
            }
            catch (Exception)
            {

                throw;
            }

            return valor;
        }

        public bool ExistPelicula(int IdPelicula)
        {
            bool valor = false;
            try
            {
                valor = _db.Pelicula.Any(x => x.Id == IdPelicula);
            }
            catch (Exception)
            {

                throw;
            }

            return valor;
        }

        public Pelicula GetPelicula(int IdPelicula)
        {
            var pelicula = new Pelicula();
            try
            {
                pelicula = _db.Pelicula.FirstOrDefault(x => x.Id == IdPelicula);
            }
            catch (Exception)
            {

                throw;
            }

            return pelicula;
        }

        public ICollection<Pelicula> GetPeliculas()
        {
            var peliculas = new List<Pelicula>();
            try
            {
                peliculas = _db.Pelicula.OrderBy(x => x.Nombre).ToList();
            }
            catch (Exception)
            {

                throw;
            }

            return peliculas;
        }

        public ICollection<Pelicula> GetPeliculasEnCategoria(int IdCategoria)
        {
            var peliculas = new List<Pelicula>();
            try
            {
                peliculas = _db.Pelicula.Include(ca => ca.Categoria).Where(x => x.IdCategoria == IdCategoria).ToList();
            }
            catch (Exception)
            {

                throw; 
            }

            return peliculas;
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
    }
}
