using AapiPeliculas.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AapiPeliculas.Data
{
    public class AplicationDbContext:DbContext
    {
        public AplicationDbContext(DbContextOptions<AplicationDbContext> contextOptions):base(contextOptions)
        {

        }


        public DbSet<Categoria> Categoria { get; set; }

    }

    //public class AplicationFactory : IDesignTimeDbContextFactory<AplicationDbContext>
    //{

    //    public AplicationDbContext CreateDbContext(string[] args)
    //    {
    //        var optionsBuilder = new DbContextOptionsBuilder<AplicationDbContext>();
    //        optionsBuilder.UseSqlServer("Server=GEDGONZ\\SQLEXPRESS;Database=PeliculasApiv1; Integrated Security=True");

    //        return new AplicationDbContext(optionsBuilder.Options);
    //    }


    //}
}
