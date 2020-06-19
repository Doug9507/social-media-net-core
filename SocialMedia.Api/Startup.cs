using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SocialMedia.Core.Interfaces;
using SocialMedia.Core.Services;
using SocialMedia.Infrastructure.Data;
using SocialMedia.Infrastructure.Filters;
using SocialMedia.Infrastructure.Repositories;

namespace SocialMedia.Api
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
            //Deshabilitar validaciones implicitas, para el uso de Fluent Validation
            services.AddControllers();
            //.ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true));

            //Agregar dependencias
            //Automapper
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            //Repositories, Inyectar Dependencias
            services.AddTransient<IPostService, PostService>();
            services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));
            //services.AddTransient<IUserRepository, UserRepository>();
            //services.AddTransient<IPostRepository, PostRepository>();

            //Configurar DbContext
            services.AddDbContext<SocialMediaContext>(options => options.UseSqlServer(Configuration.GetConnectionString("SocialMedia")));

            //Configurar Validation Filter y Fluent Validation
            //services.AddMvc(options => options.Filters.Add<ValidationFilter>())
            services.AddMvc(options => options.Filters.Add<ValidationFilter>())
                .AddFluentValidation(options => options.RegisterValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));
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
