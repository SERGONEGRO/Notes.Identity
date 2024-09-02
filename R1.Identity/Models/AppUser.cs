using Microsoft.AspNetCore.Identity;

namespace R1.Identity.Models
{
    /// <summary>
    /// Делаем свою реализацию identityuser со своими полями
    /// </summary>
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
    }
}
