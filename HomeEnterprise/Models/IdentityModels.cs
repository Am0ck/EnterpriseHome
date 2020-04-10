using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace HomeEnterprise.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        //[Required(AllowEmptyStrings = false, ErrorMessage = "Registered users must supply a first name.")]
        //[MinLength(2, ErrorMessage = "A name must have a length of at least 2")]
        //public string Email { get; set; }

        //[Required(AllowEmptyStrings = false, ErrorMessage = "Registered users must supply a last name.")]
        //[MinLength(2, ErrorMessage = "A name must have a length of at least 2")]
        //public string LastName { get; set; }
        public virtual ICollection<Item> Items { get; set; }

    }
    public class DbContextInitializer : DropCreateDatabaseIfModelChanges<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            base.Seed(context);
            //List<IdentityRole> roles = new List<IdentityRole>();
            //roles.Add(new IdentityRole() { Name = "Admin" });
            //roles.Add(new IdentityRole() { Name = "Registered User" });
            //roles.Add(new IdentityRole() { Name = "Anon" });
            context.Roles.Add(new IdentityRole() { Id = "1", Name = "Admin" });
            context.Roles.Add(new IdentityRole() { Id = "2", Name = "Registered User" });
            if (!(context.Users.Any(u => u.UserName == "admin@admin.com")))
            {
                var userStore = new UserStore<ApplicationUser>(context);
                var userManager = new UserManager<ApplicationUser>(userStore);
                var userToInsert = new ApplicationUser { UserName = "admin@admin.com", Email = "admin@admin.com" };
                userManager.Create(userToInsert, "A@dmin1234");
                userManager.AddToRole(userToInsert.Id, "admin");
            }
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            Database.SetInitializer(new DbContextInitializer());
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public System.Data.Entity.DbSet<HomeEnterprise.Models.Category> Categories { get; set; }

        public System.Data.Entity.DbSet<HomeEnterprise.Models.ItemType> ItemTypes { get; set; }
    }
}