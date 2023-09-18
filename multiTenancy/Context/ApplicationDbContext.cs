using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using multiTenancy.Models;
using multiTenancy.Services;

namespace multiTenancy.Context
{
    public class ApplicationDbContext :DbContext
    {
        private readonly string tenantid;
        private readonly ITenantService tenantService;

        public ApplicationDbContext(DbContextOptions options, ITenantService tenantService) : base(options) 
        {
            this.tenantService = tenantService;
            tenantid = tenantService.GetCurrentTenant()?.Id;
        }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // when i get the products from db i must determine the tenantid so instead making writing this condition with all get request i wirte it only one time using global query filter
            modelBuilder.Entity<Product>().HasQueryFilter(t => t.TenantId == tenantid);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
             var tenantConnectionString = tenantService.GetConnectionString();
              if(!string.IsNullOrEmpty(tenantConnectionString)) 
            {
                    var dbProvider = tenantService.GetDatabaseProvider();
                if (dbProvider.ToLower() == "mssql")
                {
                    optionsBuilder.UseSqlServer(tenantConnectionString);
                }
            }
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<IMustHaveTenant>().Where(e=>e.State==EntityState.Added)) // when i add any item which implement the imusthavetenant interface like product i will add the column tenantid dynamically
            {
                //EntityEntry<TEntity>: Represents an entry for an entity in the change tracker.
                //It provides access to information about the entity's state, properties, and can be used to perform operations like updating, deleting, or attaching entities.
                entry.Entity.TenantId = tenantid;        
              
            }
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
