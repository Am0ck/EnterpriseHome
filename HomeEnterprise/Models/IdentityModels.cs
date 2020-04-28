using System;
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
            //test
            //Seed Roles
            context.Roles.Add(new IdentityRole() { Id = "1", Name = "Admin" });
            context.Roles.Add(new IdentityRole() { Id = "2", Name = "Registered User" });
            context.SaveChanges();

            
            if (!(context.Users.Any(u => u.UserName == "admin@admin.com")))
            {
                var userStore = new UserStore<ApplicationUser>(context);
                var userManager = new UserManager<ApplicationUser>(userStore);
                var userToInsert = new ApplicationUser { UserName = "admin@admin.com", Email = "admin@admin.com" };
                userManager.Create(userToInsert, "A@dmin1234");
                userManager.AddToRole(userToInsert.Id, "admin");
            }
            
            //Seed Registered Users
            for (int i = 1; i < 6; i++)
            {
                string username = "user" + i;
                var userStore = new UserStore<ApplicationUser>(context);
                var userManager = new UserManager<ApplicationUser>(userStore);
                var userToInsert = new ApplicationUser { UserName = username, Email = username+"@gmail.com" };
                userManager.Create(userToInsert, username+"A@");
                userManager.AddToRole(userToInsert.Id, "Registered User");
                context.SaveChanges();
            }

            //Seed Categories
            List<Category> categories = new List<Category>();
            categories.Add(new Category() { Id = 1, CategoryName = "Electronics" });
            categories.Add(new Category() { Id = 2, CategoryName = "Appliances" });
            categories.Add(new Category() { Id = 3, CategoryName = "Vehicles" });
            categories.Add(new Category() { Id = 4, CategoryName = "Gaming" });
            categories.Add(new Category() { Id = 5, CategoryName = "Accessories" });
            context.Categories.AddRange(categories);
            context.SaveChanges();
            foreach(Category c in categories)
            {
                for(int i = 1; i <= 20; i++)
                {
                    context.ItemTypes.Add(new ItemType() { TypeName = "type" + i, Image = "13c-BpIZvgWH7yQB3CbdP0_q5pHzTalIC", CategoryId = c.Id });
                }
            }
            //Seed Qualities
            List<Quality> qualities = new List<Quality>();
            qualities.Add(new Quality() { Id = 1, QualityName = "Excellent" });
            qualities.Add(new Quality() { Id = 2, QualityName = "Good" });
            qualities.Add(new Quality() { Id = 3, QualityName = "Poor" });
            qualities.Add(new Quality() { Id = 4, QualityName = "Bad" });
            context.Qualities.AddRange(qualities);

            var users = context.Users;
            List<string> uids = new List<string>();
            foreach (ApplicationUser u in users)
            {
                uids.Add(u.Id);
            }
            string s = uids[0];
            Random rnd = new Random();
            long qid = 0;
            for (int i = 1; i < 101; i++)
            {
                qid++;
                if(qid > 5)
                {
                    qid = 1;
                }
                context.Items.Add(new Item() { ItemTypeId = i, OwnerId = uids[rnd.Next(0, 6)], Price = i, QualityId = (long)rnd.Next(1, 5), Quantity = i });
            }
            
            //context.Users.All();
            
            
            //seed Admin User
            /*
            if (!(context.Users.Any(u => u.UserName == "admin@admin.com")))
            {
                //var userStore = new UserStore<ApplicationUser>(context);
                //var userManager = new UserManager<ApplicationUser>(userStore);
                //var userToInsert = new ApplicationUser { UserName = "admin@admin.com", Email = "admin@admin.com" };
                //userManager.Create(userToInsert, "A@dmin1234");
                //userManager.AddToRole(userToInsert.Id, "admin");
            }
            */
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

        public System.Data.Entity.DbSet<HomeEnterprise.Models.Item> Items { get; set; }

        public System.Data.Entity.DbSet<HomeEnterprise.Models.Quality> Qualities { get; set; }
    }
}