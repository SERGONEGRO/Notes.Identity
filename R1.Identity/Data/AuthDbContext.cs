using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using R1.Identity.Models;

namespace R1.Identity.Data
{
    /// <summary>
    /// Контекст базы данных для управления пользователями и ролями с использованием ASP.NET Core Identity.
    /// </summary>
    public class AuthDbContext : IdentityDbContext<AppUser>
    {
        // <summary>
        /// Инициализирует новый экземпляр класса <see cref="AuthDbContext"/>.
        /// </summary>
        /// <param name="options">Параметры конфигурации для контекста базы данных.</param>
        public AuthDbContext(DbContextOptions<AuthDbContext> options)
            : base(options) { }

        /// <summary>Переопределяет метод для настройки модели базы данных.</summary>
        /// <param name="builder">Строитель модели, используемый для настройки модели базы данных.</param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Настраиваем таблицы для сущностей ASP.NET Core Identity
            builder.Entity<AppUser>(entity => entity.ToTable(name: "Users"));
            builder.Entity<IdentityRole>(entity => entity.ToTable(name: "Roles"));
            builder.Entity<IdentityUserRole<string>>(entity => entity.ToTable(name: "UserRoles"));
            builder.Entity<IdentityUserClaim<string>>(entity => entity.ToTable(name: "UserClaim"));
            builder.Entity<IdentityUserLogin<string>>(entity => entity.ToTable(name: "UserLogins"));
            builder.Entity<IdentityUserToken<string>>(entity => entity.ToTable(name: "UserTokens"));
            builder.Entity<IdentityRoleClaim<string>>(entity => entity.ToTable(name: "RoleClaims"));

            builder.ApplyConfiguration(new AppUserConfiguration());
        }
    }
}
