using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AapiPeliculas.Models;
using AapiPeliculas.Models.DTOS;
using AapiPeliculas.Repositorios.IRepository;
using AutoMapper;
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
        public PeliculasController(IPeliculaRepositorio _PeliculaRepositorio, IMapper mapper)
        {
            this._PeliculaRepositorio = _PeliculaRepositorio;
            this.mapper = mapper;
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

        [HttpPost]
        public IActionResult CrearCtaegoria([FromBody] Peliculadto peliculadto)
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
            var pelicula = mapper.Map<Pelicula>(peliculadto);

            if (!_PeliculaRepositorio.CrearPelicula(pelicula))
            {
                ModelState.AddModelError("", $"Algo salio mal al crear el registro {pelicula.Nombre}");

                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetPelicula", new { IdPelicula = pelicula.Id }, pelicula);

        }

        [HttpPatch("{IdPelicula:int}", Name = "ActualizarPelicula")]
        public IActionResult ActualizarPelicula(int IdPelicula, [FromBody] Peliculadto peliculadto)
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
