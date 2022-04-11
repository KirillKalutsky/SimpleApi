using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using SimpleApi.Core;
using SimpleApi.DB;
using SimpleApi.Repositories;

namespace SimpleApi
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
            var server = Configuration["DBServer"] ?? "localhost";
            var port = Configuration["DBPort"] ?? "5432";
            var user = Configuration["DBUser"] ?? "postgres";
            var password = Configuration["DBPassword"] ?? "abrakadabra77";
            var db = Configuration["DB"] ?? "journalDB";
            var cS = $"host={server};port={port};database={db};username={user};password={password}";
            Console.WriteLine(cS);
            services.AddEntityFrameworkNpgsql().AddDbContext<JournalContext>(opt =>
                opt.UseNpgsql(cS));
            services.AddControllers()
                .AddNewtonsoftJson(x => 
                    x.SerializerSettings.ReferenceLoopHandling 
                        = Newtonsoft.Json.ReferenceLoopHandling.Ignore);




            services.AddDbContext<JournalContext>();

            services.AddControllers();

            services.AddScoped<ArticleRepository>();
            services.AddScoped<AuthorRepository>();

            services.AddAutoMapper(cfg =>
            {
                cfg.CreateMap<Article, ArticleToUpdateDto>();
                cfg.CreateMap<ArticleToCreateDto, Article>();
                cfg.CreateMap<ArticleToUpdateDto, Article>();

                cfg.CreateMap<Author, AuthorToUpdateDto>();
                cfg.CreateMap<AuthorToCreateDto, Author>();
                cfg.CreateMap<AuthorToUpdateDto, Author>();

                cfg.CreateMap<Article, ArticleDto>()
                    .ForMember(dto => dto.Creators,
                        opt => opt.MapFrom(
                            article => article.Creators.Select(cr => cr.Id)));

                cfg.CreateMap<Author, AuthorDto>()
                    .ForMember(dto => dto.Works,
                        opt => opt.MapFrom(
                            article => article.Works.Select(cr => cr.Id)));
            }, Array.Empty<Assembly>());

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "SimpleApi", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SimpleApi v1"));
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
