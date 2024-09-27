namespace Email.Models
{
    using Microsoft.AspNetCore.Identity.Data;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

        public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
        {
            public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
                : base(options)
            {

            }

            // You can define other DbSets for additional entities here if needed
            // public DbSet<SomeOtherEntity> SomeOtherEntities { get; set; }
        }
    

}
