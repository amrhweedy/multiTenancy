using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using multiTenancy.settings;
using System;
using System.IO;

namespace multiTenancy.Services
{
  //  IOptions and IConfiguration are both part of the configuration system in ASP.NET Core and are used to access configuration settings, but they serve slightly different purposes and have different use cases:
    //Purpose: IOptions<T> is used for strongly-typed access to configuration settings.
    //It allows you to bind configuration values to strongly-typed .NET objects.    
    // Typical Usage: It is commonly used in services, controllers, and components where you want to work with configuration settings in a strongly-typed manner.

    //Purpose: IConfiguration is a more general interface for accessing configuration settings, and it provides a key-value representation of the configuration.It's not type-safe by default, and you access values by specifying keys.
    //Typical Usage: It is used when you need to access configuration settings in a more dynamic or untyped way, or when you want to retrieve configuration values using keys directly from configuration providers.

    //IOptions<T> is preferred when you want to work with strongly-typed configuration settings, making your code more readable, maintainable, and type-safe.
   //IConfiguration is more general and provides a way to access configuration settings without type checking.It is useful when dealing with more complex or dynamic configuration scenarios.
    public class TenantService : ITenantService
    {   
        private readonly TenantSettings _tenantSettings; 
        private Tenant? currentTenant;
        private HttpContext? context;
        public TenantService(IHttpContextAccessor httpContextAccessor , IOptions<TenantSettings> tenantSettings) // i want to map configuration which its key is tenantsettings in appsettings to class tenantsettings
        {
            context = httpContextAccessor.HttpContext;
            _tenantSettings = tenantSettings.Value; // tenantSettings.value is the value of the configuration which its key is tenantsettings in appsettings

            if(context is not null)
            {
                if(context.Request.Headers.TryGetValue("tenant",out var tenantid))
                {
                    setCurrentTenant(tenantid!);
                }
                else
                {
                    throw new Exception("tenant is not found");
                }
            }
            
        }
        public string? GetConnectionString()
        {
            var currentconnectionString = currentTenant is null ?  _tenantSettings.Defaults.ConnectionString 
                                                                                    : currentTenant.ConnectionString;
            return currentconnectionString; 
        }

        public Tenant? GetCurrentTenant()
        {
            return currentTenant;
        }

        public string? GetDatabaseProvider()
        {
            return _tenantSettings.Defaults.DBProvider;
        }

        public void setCurrentTenant(string tenantid)
        {
            currentTenant = _tenantSettings.Tenants.FirstOrDefault(t => t.Id == tenantid);

            if (currentTenant is null)
            {
                throw new Exception("this tenant is not found");
            }

            if (string.IsNullOrWhiteSpace(currentTenant.ConnectionString))
            {
                currentTenant.ConnectionString = _tenantSettings.Defaults.ConnectionString;
            }
        }
    }
}
