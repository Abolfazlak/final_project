using Microsoft.EntityFrameworkCore;

namespace RiskManagement.API.RiskManagement.DataProvide;

public class RiskManagementDbContext(DbContextOptions<RiskManagementDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Company> Companies { get; set; }
    public DbSet<Project> Projects { get; set; }

    public DbSet<Risk> Risks { get; set; }
    public DbSet<MainRiskCategory> MainRiskCategories { get; set; }
    public DbSet<SecondaryRiskCategory> SecondaryRiskCategories { get; set; }

    public DbSet<RiskDetails> RiskDetails { get; set; }
    public DbSet<Solution> Solutions { get; set; }


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
        
        modelBuilder.Entity<Risk>(entity =>
        {
            entity.HasKey(p => p.Id); 
            entity.HasOne(r => r.SecondaryRiskCategory)
                .WithMany(s => s.Risks)
                .HasForeignKey(r => r.SecondaryRiskCategoryId);
            
            entity.HasOne(r => r.Project)
                .WithMany(p => p.Risks)
                .HasForeignKey(r => r.ProjectId);
        });
        
        modelBuilder.Entity<SecondaryRiskCategory>(entity =>
        {
            entity.HasKey(p => p.Id); 
            entity.HasOne(src => src.MainRiskCategory)
                .WithMany(mrc => mrc.SecondaryRiskCategories)
                .HasForeignKey(src => src.MainRiskCategoryId);
        });
        
        modelBuilder.Entity<RiskDetails>(entity =>
        {
            entity.HasKey(p => p.Id); 
            entity.HasOne(rd => rd.Risk)
                .WithMany(r => r.RiskDetails)
                .HasForeignKey(rd => rd.RiskId);
        });
        
        modelBuilder.Entity<Solution>(entity =>
        {
            entity.HasKey(p => p.Id); 
            entity.HasOne(s => s.Risk)
                .WithMany(r => r.Solutions)
                .HasForeignKey(s => s.RiskId);
        });

    }
}