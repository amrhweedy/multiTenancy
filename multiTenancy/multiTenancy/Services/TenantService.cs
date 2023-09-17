namespace multiTenancy.Services
{
    public class TenantService : ITenantService
    {
        public string? GetConnectionString()
        {
            throw new NotImplementedException();
        }

        public Tenant? GetCurrentTenant()
        {
            throw new NotImplementedException();
        }

        public string? GetDatabaseProvider()
        {
            throw new NotImplementedException();
        }
    }
}
