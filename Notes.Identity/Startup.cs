using System.IO;
using IdentityServer4;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Notes.Identity.Data;
using Notes.Identity.Models;

namespace Notes.Identity
{
    public class Startup
    {
       
        public IConfiguration AppConfiguration { get; }

        public Startup(IConfiguration configuration) => AppConfiguration = configuration;
        

        /// <summary>
        /// Добавление всех сервисов, которые планируется использовать в приложении
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = AppConfiguration.GetValue<string>("DbConnection");
             
            //добавляем контекст бд
            services.AddDbContext<AuthDbContext>(options =>
            {
                options.UseSqlite(connectionString);
            });

            //требования к паролю
            services.AddIdentity<AppUser, IdentityRole>(config =>
            {
                config.Password.RequiredLength = 4;
                config.Password.RequireDigit = false;
                config.Password.RequireNonAlphanumeric = false;
                config.Password.RequireUppercase = false;
            })
                .AddEntityFrameworkStores<AuthDbContext>() //добавляем контекст как хранилище к identity
                .AddDefaultTokenProviders();               //добавляем дефолтный провайдер для получения и обновления токенов доступа


            services.AddIdentityServer()
                .AddAspNetIdentity<AppUser>() //добавляем appUser как AspNetIdentity для IdentityServer
                .AddInMemoryApiResources(Configuration.ApiResources)
                .AddInMemoryIdentityResources(Configuration.IdentityResources)
                .AddInMemoryApiScopes(Configuration.ApiScopes)
                .AddInMemoryClients(Configuration.Clients)
                .AddDeveloperSigningCredential();

            //настраиваем куки для хранения токена
            services.ConfigureApplicationCookie(config =>
            {
                config.Cookie.Name = "Notes.Identity.Cookie";
                config.LoginPath = "/Auth/Login";
                config.LogoutPath = "/Auth/Logout";
            });

            services.AddControllersWithViews(); //добавляем возможность использовать контроллеры и представления
        }

        /// <summary>
        /// Здесь настраивается конвейер обработки запроса. Применяются все middleware
        /// выполняются в том порядке, в ктором добавляются в конвейер
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if(env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles(new StaticFileOptions   //подключаем статические файлы
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(env.ContentRootPath, "Styles")),
                RequestPath = "/styles"
            });
            app.UseRouting();
            app.UseIdentityServer();  //используем identityserver
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });

        }
    }
}
