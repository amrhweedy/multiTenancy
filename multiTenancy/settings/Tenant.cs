namespace multiTenancy.settings
{
    public class Tenant
    {
        public string Id { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string? ConnectionString { get; set; } // if i determine this connetionstring i will use it if i dont determine it i will use the connectionstring in configuration class
    }
}
