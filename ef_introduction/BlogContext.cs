namespace ef_introduction;

using Microsoft.EntityFrameworkCore;
using System;

public class BlogContext : DbContext
{
    public DbSet<Blog> Blogs { get; set; }
    public DbSet<Post> Posts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        // Ersetzen Sie dies durch Ihren echten Connection String
        var connectionString = "server=localhost;user=root;password=insy;database=EFCoreDemo";
        
        // Pomelo benötigt die Server-Version für korrekte SQL-Generierung
        var serverVersion = ServerVersion.AutoDetect(connectionString);

        options.UseMySql(connectionString, serverVersion)
            // Optional: Zeigt SQL-Befehle in der Konsole an (gut zum Lernen)
            .LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information)
            .EnableSensitiveDataLogging()
            .EnableDetailedErrors();
    }
}