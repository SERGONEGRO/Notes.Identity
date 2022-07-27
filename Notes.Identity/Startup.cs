using IdentityServer4;
using Microsoft.Extensions.Configuration;

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
            services.AddIdentityServer()
                .AddInMemoryApiResources(Configuration.ApiResources)
                .AddInMemoryIdentityResources(Configuration.IdentityResources)
                .AddInMemoryApiScopes(Configuration.ApiScopes)
                .AddInMemoryClients(Configuration.Clients)
                .AddDeveloperSigningCredential();
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

            app.UseRouting();
            app.UseIdentityServer();  //используем identityserver
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("huy");
                });
            });

        }
    }
}
