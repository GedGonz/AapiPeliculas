using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AapiPeliculas.Models;
using AapiPeliculas.Models.DTOS;
using AapiPeliculas.Repositorios.IRepository;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AapiPeliculas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PeliculasController : ControllerBase
    {

        private readonly IPeliculaRepositorio _PeliculaRepositorio;
        private readonly IMapper mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public PeliculasController(IPeliculaRepositorio _PeliculaRepositorio, IMapper mapper, IWebHostEnvironment _webHostEnvironment)
        {
            this._PeliculaRepositorio = _PeliculaRepositorio;
            this.mapper = mapper;
            this._webHostEnvironment = _webHostEnvironment;
        }

        [HttpGet]
        public IActionResult GetPeliculas()
        {
            var listaPeliculas = _PeliculaRepositorio.GetPeliculas();

            var listaPeliculasdto = mapper.Map<List<Peliculadto>>(listaPeliculas);

            return Ok(listaPeliculasdto);
        }

        [HttpGet("{IdPelicula:int}", Name = "GetPelicula")]
        public IActionResult GetPelicula(int IdPelicula)
        {
            var pelicula = _PeliculaRepositorio.GetPelicula(IdPelicula);

            if (pelicula == null)
            {
                return NotFound();
            }

            var peliculadto = mapper.Map<Peliculadto>(pelicula);

            return Ok(peliculadto);
        }

        [HttpGet("GetPeliculasEnCategorias/{IdCategoria:int}")]
        public IActionResult GetPeliculasEnCategorias(int IdCategoria)
        {
            var listaPeliculas = _PeliculaRepositorio.GetPeliculasEnCategoria(IdCategoria);

            if (listaPeliculas==null) 
            {
                return NotFound();
            }

            var listapeliculasdto = mapper.Map<List<Peliculadto>>(listaPeliculas);

            return Ok(listapeliculasdto);
        }

        [HttpGet("Buscar")]
        public IActionResult Buscar(string nombre) 
        {
            try
            {
                var peliculas = _PeliculaRepositorio.BuscarPelicula(nombre);
                
                if (peliculas.Any()) 
                {
                    var peliculasdto = mapper.Map<List<Peliculadto>>(peliculas);
                    return Ok(peliculasdto);
                }
                return NotFound();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,"Error recuperando datos de la aplicación");
            }
        }

        [HttpPost]
        public IActionResult CrearPelicula([FromForm] PeliculaCreatedto peliculadto)
        {
            if (peliculadto == null)
            {
                return BadRequest(ModelState);
            }

            if (_PeliculaRepositorio.ExistPelicula(peliculadto.Nombre))
            {
                ModelState.AddModelError("", "La Pelicula ya existe");

                return StatusCode(404, ModelState);
            }

            /*Subida de Archivos*/
            var archivo = peliculadto.Foto;
            var rutaPrincipal = _webHostEnvironment.WebRootPath;
            var archivos = HttpContext.Request.Form.Files;

            if (archivo.Length>0)
            {
                //Nueva Imagen
                var nombreFoto = Guid.NewGuid().ToString();
                var rutaCompleta = Path.Combine(rutaPrincipal, @"fotos");
                var extension = Path.GetExtension(archivos[0].FileName);


                using (var fileStrems = new FileStream(Path.Combine(rutaCompleta, nombreFoto + extension), FileMode.Create))
                {
                    archivos[0].CopyTo(fileStrems);
                }
                peliculadto.RutaImagen = @"\foto\"+nombreFoto+extension;
            }

            var pelicula = mapper.Map<Pelicula>(peliculadto);

            if (!_PeliculaRepositorio.CrearPelicula(pelicula))
            {
                ModelState.AddModelError("", $"Algo salio mal al crear el registro {pelicula.Nombre}");

                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetPelicula", new { IdPelicula = pelicula.Id }, pelicula);

        }

        [HttpPatch("{IdPelicula:int}", Name = "ActualizarPelicula")]
        public IActionResult ActualizarPelicula(int IdPelicula, [FromBody] PeliculaUpdatedto peliculadto)
        {

            if (peliculadto == null || IdPelicula != peliculadto.Id)
            {
                return BadRequest(ModelState);
            }

            var pelicula = mapper.Map<Pelicula>(peliculadto);

            if (!_PeliculaRepositorio.ActualizarPelicula(pelicula))
            {
                ModelState.AddModelError("", $"Algo salio mal al actualizar el registro {pelicula.Nombre}");

                return StatusCode(500, ModelState);
            }

            return NoContent();
        }


        [HttpDelete("{IdPelicula:int}", Name = "EliminarPelicula")]
        public IActionResult EliminarPelicula(int IdPelicula)
        {

            if (!_PeliculaRepositorio.ExistPelicula(IdPelicula))
            {
                return NotFound();
            }

            var pelicula = _PeliculaRepositorio.GetPelicula(IdPelicula);



            if (!_PeliculaRepositorio.EliminarPelicula(pelicula))
            {
                ModelState.AddModelError("", $"Algo salio mal al eliminar el registro {pelicula.Nombre}");

                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

    }
}
