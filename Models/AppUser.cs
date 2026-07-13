using Microsoft.AspNetCore.Identity;

namespace UNI_ASSETS.Models
{
    public class AppUser:IdentityUser
    {
        public string Surname { get; set; }
       public string Name { get; set; }
    }
}
