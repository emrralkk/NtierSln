using Microsoft.AspNetCore.Identity;

namespace Ntier.Data.Models.Identity
{
    public class AppUser : IdentityUser<int>
    {
        public AppUser()
        {
        }

        public AppUser(string userName) : base(userName)
        {
        }
        public string Name { get; set; }
        public string Surname { get; set; }
    }
}
