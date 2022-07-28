using Microsoft.AspNetCore.Identity;

namespace Notes.Identity.Models
{
    /// <summary>
    /// Делаем свою реализацию identityuser со своими полями
    /// </summary>
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
