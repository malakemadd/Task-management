using Microsoft.AspNetCore.Identity;

namespace TaskManagment.Models
{
    public class AppUser:IdentityUser
    {
        public ICollection<Tasks> Tasks { get; set; }


    }
}
