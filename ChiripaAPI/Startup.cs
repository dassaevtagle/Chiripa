using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using AutoMapper;
using ChiripaAPI.Data;
using ChiripaAPI.Models;
using ChiripaAPI.Services.Infrastructure;
using ChiripaAPI.Services.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;

namespace ChiripaAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        
        public void ConfigureServices(IServiceCollection services)
        {
            // Database Configuration
            services.AddDbContext<ChiripaDbContext>(
                options => options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"),
                opt => opt.SetPostgresVersion(new Version(12, 11))));
            
            // Register the Identity services
             services.AddIdentity<ApplicationUser, IdentityRole>(cfg =>
                {
                    cfg.User.RequireUniqueEmail = true;
                }).AddEntityFrameworkStores<ChiripaDbContext>();
            
            services.AddAuthentication()
            .AddJwtBearer(cfg =>
            {
                cfg.TokenValidationParameters = new TokenValidationParameters()
                {
                ValidIssuer = Configuration["Tokens:Issuer"],
                ValidAudience = Configuration["Tokens:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Tokens:Key"]))
                };
            });
            
            // Login options
            services.Configure<IdentityOptions>(options =>
            {
                // Default Password settings.
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 5;
                options.Password.RequiredUniqueChars = 0;
            });


            services.AddScoped<IHielito, HielitoRepo>();
            services.AddScoped<IUsuario, UsuarioRepo>();
            services.AddScoped<IDbInitialize, DbInitializeRepo>();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            // Cors Access Policy
            services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader();
            }));
            
            
            services.AddHttpClient();
            
            services.AddControllers().AddNewtonsoftJson(options => 
                    {
                        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                        options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    })
                    .AddJsonOptions(options => 
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter())
            );
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IDbInitialize dbInitializer)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("MyPolicy");  

            dbInitializer.Initialize();

            app.UseHttpsRedirection();

            app.UseRouting();

            // who are u? quien eres?
            app.UseAuthentication();

            // reviso la lista para ver si puedes entrar
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "api/{controller=Hielitos}/{action=Obtener}/{id?}");

                endpoints.MapControllers();
            });
        }
    }
}
