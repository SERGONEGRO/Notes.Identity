using Duende.IdentityServer.Test;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using R1.Identity.Data;
using R1.Identity.Models;

namespace R1.Identity
{
    public class Startup
    {
        public IConfiguration AppConfiguration { get; }

        public Startup(IConfiguration configuration) => AppConfiguration = configuration;

        /// <summary>Добавление всех сервисов, которые планируется использовать в приложении</summary>
        /// <param name="services">Коллекция сервисов для конфигурации</param>
        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = AppConfiguration.GetValue<string>("DbConnection");
             
            services.AddDbContext<AuthDbContext>(options =>
            {
                options.UseSqlite(connectionString);
            });

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
                .AddTestUsers(TestUsers.Users)

                //.AddAspNetIdentity<AppUser>() //добавляем appUser как AspNetIdentity для IdentityServer
                .AddInMemoryApiResources(Configuration.ApiResources)
                .AddInMemoryIdentityResources(Configuration.IdentityResources)
                .AddInMemoryApiScopes(Configuration.ApiScopes)
                .AddInMemoryClients(Configuration.Clients)
                .AddDeveloperSigningCredential();

            
            //настраиваем куки для хранения токена
            services.ConfigureApplicationCookie(config =>
            {
                config.Cookie.Name = "R1.Identity.Cookie";
                config.LoginPath = "/Auth/Login";
                config.LogoutPath = "/Auth/Logout";
            });

            services.AddControllersWithViews(); //добавляем возможность использовать контроллеры и представления
        }

        /// <summary>
        /// Здесь настраивается конвейер обработки запроса. Применяются все middleware
        /// выполняются в том порядке, в ктором добавляются в конвейер
        /// </summary>
        /// <param name="app">Приложение</param>
        /// <param name="env">Среда хостинга</param>
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

    public static class TestUsers
    {
        public static List<TestUser> Users
        {
            get
            {
                return new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = "1",
                    Username = "alice",
                    Password = "password"
                },
                new TestUser
                {
                    SubjectId = "2",
                    Username = "bob",
                    Password = "password"
                }
            };
            }
        }
    }
}
