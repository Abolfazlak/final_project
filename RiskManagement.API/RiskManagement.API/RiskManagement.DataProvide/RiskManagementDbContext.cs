using Microsoft.EntityFrameworkCore;

namespace RiskManagement.API.RiskManagement.DataProvide;

public class RiskManagementDbContext(DbContextOptions<RiskManagementDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Company> Companies { get; set; }
    public DbSet<Project> Projects { get; set; }



    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder){}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id); 
            entity.HasOne(u => u.Company)
                .WithMany(c => c.Users);
        });
        
        modelBuilder.Entity<Project>(entity =>
        {
            entity.HasKey(p => p.Id); 
            entity.HasOne(p => p.Company)
                .WithMany(c => c.Projects)
                .HasForeignKey(p => p.CompanyId);
        });
        
        modelBuilder.Entity<Company>(entity =>
        {
            entity.HasKey(c => c.Id); 
        });

    }
}