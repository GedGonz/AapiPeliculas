using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static AapiPeliculas.Models.Pelicula;

namespace AapiPeliculas.Models.DTOS
{
    public class PeliculaUpdatedto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El nombre es requerido")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "La Rutaimagen es requerido")]
        public string RutaImagen { get; set; }
        [Required(ErrorMessage = "La Descripcion es requerido")]
        public string Descripcion { get; set; }
        [Required(ErrorMessage = "La Duracion es requerido")]
        public string Duracion { get; set; }
        public TipoClasificacion Clasificacion { get; set; }
        public int IdCategoria { get; set; }
    }
}
