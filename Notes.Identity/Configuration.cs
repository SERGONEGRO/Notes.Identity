using Duende.IdentityServer.Models;
using IdentityModel;

namespace Notes.Identity
{
    /// <summary>
    /// Статический класс для конфигурации IdentityServer.
    /// содержит информацию о клиентах, ресурсах и т.д
    /// </summary>
    public static class Configuration
    {
        /// <summary>
        /// Возвращает коллекцию API-областей (API scopes), которые будут использоваться в IdentityServer,
        /// которые можно использовать клиентскому приложению (Идентификатор, который отправляется во время
        /// аутентификации в процессе запроса токена)
        /// </summary>
        public static IEnumerable<ApiScope> ApiScopes =>
            new List<ApiScope>
            {
                new ApiScope("NotesWebApi", "Web API")
            };

        /// <summary>
        /// Возвращает коллекцию ресурсов идентификации (identity resources), которые будут использоваться в IdentityServer.
        /// Эта коллекция позволит клиентскому приложению просматривать множество утверждений о пользователе
        /// </summary>
        public static IEnumerable<IdentityResource> IdentityResources =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };

        /// <summary>
        /// Возвращает коллекцию API-ресурсов (API resources), которые будут использоваться в IdentityServer.
        /// Эта коллекция позволяет смоделировать доступ ко всему защищаемому ресурсу
        /// </summary>
        public static IEnumerable<ApiResource> ApiResources =>
            new List<ApiResource>
            {
                new ApiResource("NotesWebAPI", "Web API", new []{JwtClaimTypes.Name})
                {
                    Scopes = { "NotesWebAPI" }
                }
            };

        /// <summary>
        /// Возвращает коллекцию клиентов (clients), которые будут использоваться в IdentityServer.
        /// Эти клиенты могут использовать наш api
        /// </summary>
        public static IEnumerable<Client> Clients =>
            new List<Client>
            {
                new Client
                {
                    ClientId = "notes-web-api", //клиент id на клиенте должен быть = id клиента на сервере
                    ClientName = "Notes Web",
                    AllowedGrantTypes = GrantTypes.Code,  //используем autorization code
                    RequireClientSecret = false,//не исп. secret клиента (обычно это sha256 строка)
                    RequirePkce = true,         //нужен ключ подтверждения для autorization code
                    RedirectUris =              //набор адресов, куда может происходить перенаправление после аутентификации
                    {                           //клиентского приложения
                        "http://localhost:3000/signin-oidc"
                    },
                    AllowedCorsOrigins =        //набор uri адресов, кому позволено использовать identityserver
                    {
                         "http://localhost:3000"
                    },                          
                    PostLogoutRedirectUris =    //набор uri адресов, на которые переадресовывает после выхода клиентского
                    {                           //приложения
                        "http://localhost:3000/signout-oidc"
                    },
                    AllowedScopes =             //области (scopes) доступные клиенту
                    {
                        "NotesWebApi"
                    },
                    AllowAccessTokensViaBrowser = true  //управляет передачей токена через браузер
                }
            };
    }
}
