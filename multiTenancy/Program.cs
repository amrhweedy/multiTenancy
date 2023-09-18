

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using multiTenancy.Context;
using multiTenancy.Services;

namespace multiTenancy
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddScoped<ITenantService, TenantService>();
            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            builder.Services.AddScoped<IProductService, ProductService>();

            builder.Services.Configure<TenantSettings>(builder.Configuration.GetSection("TenantSettings"));

            TenantSettings options = new TenantSettings();
            builder.Configuration.GetSection(nameof(TenantSettings)).Bind(options);


            var defaultProvider = options.Defaults.DBProvider;
            if (defaultProvider.ToLower() == "mssql")
            {
                builder.Services.AddDbContext<ApplicationDbContext>(m => m.UseSqlServer());
            }
            
            foreach(var tenant in options.Tenants)
            {
                var connectionString = tenant.ConnectionString ?? options.Defaults.ConnectionString;    

                // i want to inject the applicationDbContext here but there is no constructor for program.cs so i make scope to can deal with applicationdbcontext here 
                // i want applicationdbcontext to make automatic updatedatabase for any pending migrations

                using var scope  = builder.Services.BuildServiceProvider().CreateScope();
                var dbcontext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                
                dbcontext.Database.SetConnectionString(connectionString);

                if (dbcontext.Database.GetPendingMigrations().Any())
                {
                    dbcontext.Database.Migrate();
                }
            }



            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}