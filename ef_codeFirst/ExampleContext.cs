using Microsoft.EntityFrameworkCore;

namespace ef_codeFirst;

public partial class ExampleContext : DbContext
{
    public ExampleContext()
    {
    }

    public ExampleContext(DbContextOptions<ExampleContext> options)
        : base(options)
    {
    }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https: //go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySQL("server=127.0.0.1;uid=root;pwd=insy;database=scottnew");
    public virtual DbSet<Example> Examples { get; set; }
}