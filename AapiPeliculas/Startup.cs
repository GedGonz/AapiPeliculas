using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AapiPeliculas.Data;
using AapiPeliculas.Maper;
using AapiPeliculas.Repositorios;
using AapiPeliculas.Repositorios.IRepository;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AapiPeliculas
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContextPool<AplicationDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DbConnection"))
            );

            services.AddScoped<ICategoriaRepositorio, CategoriaRepositorio>();
            services.AddScoped<IPeliculaRepositorio, PeliculaRepositorio>();
            services.AddAutoMapper(typeof(ProfileMappers));
            //services.AddDbContext<AplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DbConnection")));
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
