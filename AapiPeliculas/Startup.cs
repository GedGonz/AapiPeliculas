using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AapiPeliculas.Data;
using AapiPeliculas.Helpers;
using AapiPeliculas.Maper;
using AapiPeliculas.Repositorios;
using AapiPeliculas.Repositorios.IRepository;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

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
            services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();


            /*Configuración Token*/
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(option =>
                {
                    option.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration.GetSection("AppSettings:Token").Value)),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });

            services.AddAutoMapper(typeof(ProfileMappers));

            //Configuración de la documentacion con Swagger

            services.AddSwaggerGen(option=> 
            {
                option.SwaggerDoc("ApiPeliculasCategorias", new Microsoft.OpenApi.Models.OpenApiInfo()
                {
                    Title = "API Categorías Películas",
                    Version = "1",
                    Description = "Backend películas",
                    Contact = new Microsoft.OpenApi.Models.OpenApiContact() 
                    {
                        Email ="gedgonz7@gmail.com",
                        Name = "Gerald González",
                        Url = new Uri("https://gedgonz.github.io/CV")
                    },
                    License = new Microsoft.OpenApi.Models.OpenApiLicense() 
                    {
                        Name = "MIT License",
                        Url = new Uri("https://en.wikipedia.org/wiki/MIT_License")
                    }

                });

                option.SwaggerDoc("ApiPeliculas", new Microsoft.OpenApi.Models.OpenApiInfo()
                {
                    Title = "API Películas",
                    Version = "1",
                    Description = "Backend películas",
                    Contact = new Microsoft.OpenApi.Models.OpenApiContact()
                    {
                        Email = "gedgonz7@gmail.com",
                        Name = "Gerald González",
                        Url = new Uri("https://gedgonz.github.io/CV")
                    },
                    License = new Microsoft.OpenApi.Models.OpenApiLicense()
                    {
                        Name = "MIT License",
                        Url = new Uri("https://en.wikipedia.org/wiki/MIT_License")
                    }

                });

                option.SwaggerDoc("ApiPeliculasUsuarios", new Microsoft.OpenApi.Models.OpenApiInfo()
                {
                    Title = "API Usuarios Películas ",
                    Version = "1",
                    Description = "Backend películas",
                    Contact = new Microsoft.OpenApi.Models.OpenApiContact()
                    {
                        Email = "gedgonz7@gmail.com",
                        Name = "Gerald González",
                        Url = new Uri("https://gedgonz.github.io/CV")
                    },
                    License = new Microsoft.OpenApi.Models.OpenApiLicense()
                    {
                        Name = "MIT License",
                        Url = new Uri("https://en.wikipedia.org/wiki/MIT_License")
                    }

                });


                var archivoXMLComentarios = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var rutaApiComentario = Path.Combine(AppContext.BaseDirectory, archivoXMLComentarios);

                option.IncludeXmlComments(rutaApiComentario);

                //Definición de esquema de seguridad en swagger

                option.AddSecurityDefinition("Bearer",
                    new OpenApiSecurityScheme
                    {
                        Description = "Autenticación JWT (Bearer)",
                        Type = SecuritySchemeType.Http,
                        Scheme = "bearer"
                    });

                option.AddSecurityRequirement(new OpenApiSecurityRequirement
                { 
                    {
                        new OpenApiSecurityScheme()
                        {
                            Reference = new OpenApiReference
                            {
                                Id = "Bearer",
                                Type= ReferenceType.SecurityScheme
                            }
                        }, new List<string>()
                    }
                });
            });

            services.AddControllers();

            //habilitacion de CORS
            services.AddCors();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else 
            {
                app.UseExceptionHandler(builder =>
                {
                    builder.Run(async context=>{
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        var error = context.Features.Get<IExceptionHandlerFeature>();
                        if (error !=null) 
                        {
                            context.Response.AddApplicationError(error.Error.Message);
                            await context.Response.WriteAsync(error.Error.Message);
                        } 
                });

                });
            }

            app.UseHttpsRedirection();

            //Middleware swagger
            app.UseSwagger();
            app.UseSwaggerUI(options=> 
            {
                options.SwaggerEndpoint("/swagger/ApiPeliculasCategorias/swagger.json", "Api Peliculas Categorías");
                options.SwaggerEndpoint("/swagger/ApiPeliculas/swagger.json", "Api Peliculas");
                options.SwaggerEndpoint("/swagger/ApiPeliculasUsuarios/swagger.json", "Api Peliculas Usuarios");
                options.RoutePrefix = "";
            });

            app.UseRouting();

            //MiddleWare para la autenticacion y autorizacion

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            //Middleware Cors
            app.UseCors(x=> 
            {
                x.AllowAnyOrigin();
                x.AllowAnyMethod();
                x.AllowAnyHeader();
            });
        }
    }
}
