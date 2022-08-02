using System.Collections.Generic;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;

namespace Notes.Identity
{
    /// <summary>
    /// содержит информацию о клиентах, ресурсах и т.д
    /// </summary>
    public static class Configuration
    {
        /// <summary>
        /// Области, которые клиентскому приложению можно использовать(Идентификатор, который отправляется во время
        /// аутентификации в процессе запроса токена
        /// </summary>
        public static IEnumerable<ApiScope> ApiScopes =>
            new List<ApiScope>
            {
                new ApiScope("NotesWebApi", "Web API")
            };
        /// <summary>
        /// identityResource позволяет смоделировать область, которое позволит клиентскому приложению просматривать
        /// множество утверждений о пользователе
        /// </summary>
        public static IEnumerable<IdentityResource> IdentityResources =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };

        /// <summary>
        /// Позволяет смоделировать доступ ко всему защищаемому ресурсу
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
        /// список клиентских приложений, которые могут использовать наш api
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
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "NotesWebApi"
                    },
                    AllowAccessTokensViaBrowser = true  //управляет передачей токена через браузер
                }
            };
    }
}
