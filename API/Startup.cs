using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.Extensions;
using API.Interfaces;
using API.Middleware;
using API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace API
{
    public class Startup
    {
        //se inyecta configuracion a la startup class
        private readonly IConfiguration _config;

        public Startup(IConfiguration config)
        {
            _config = config;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //aca se inyectan las dependencias .net core se encargara de crear y destruir las clases
            
            services.AddAplicationServices(_config); //traigo lo que esta en ApplicationServiceExtensions
            services.AddControllers();
            services.AddCors();
            services.AddIdentityServices(_config); //traigo lo que esta en IdentityServiceExtensions
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            //Exception handdler por defecto, lo comento ya que se hizo uno personalizado
            /*if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage(); //middleware para exceptions
            }*/

            app.UseMiddleware<ExceptionMiddleware>();

            app.UseHttpsRedirection(); //redirige al endpoint

            app.UseRouting(); //lleva al controller

            app.UseCors(policy => policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200"));

            app.UseAuthentication();            
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers(); //busca los controller para ver los endpoints disponibles
            });
        }
    }
}
