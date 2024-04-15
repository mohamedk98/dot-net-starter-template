using Microsoft.EntityFrameworkCore;

namespace starter_template.Data;

public class DatabaseContext : DbContext
{
    private readonly IConfiguration _configuration;

    public DatabaseContext(DbContextOptions<DatabaseContext> options, IConfiguration configuration) : base(options)
    {
        _configuration = configuration;
    }
    
    //Here you can handle entities keys using fluentApi
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
    }
}