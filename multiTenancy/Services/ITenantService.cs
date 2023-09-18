namespace multiTenancy.Services
{
    public interface ITenantService
    {
        string? GetDatabaseProvider();
        string? GetConnectionString();

        Tenant? GetCurrentTenant();


    }
}
