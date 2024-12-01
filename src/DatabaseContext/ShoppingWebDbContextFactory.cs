using DatabaseContext.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DatabaseContext;

public class ShoppingWebDbContextFactory : IDesignTimeDbContextFactory<ShoppingWebDbContext>
{
    public ShoppingWebDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ShoppingWebDbContext>();
        var connectionString = args[0];
        optionsBuilder.UseSqlServer(connectionString);
        
        return new ShoppingWebDbContext(optionsBuilder.Options);
    }
}