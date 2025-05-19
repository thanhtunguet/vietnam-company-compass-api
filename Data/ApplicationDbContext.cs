using Microsoft.EntityFrameworkCore;
using VietnamBusiness.Models;
using System;

namespace VietnamBusiness.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Business> Businesses { get; set; }
        public DbSet<CompanyStatus> CompanyStatuses { get; set; }
        public DbSet<CrawlerJob> CrawlerJobs { get; set; }
        public DbSet<CrawlingStatus> CrawlingStatuses { get; set; }
        public DbSet<Province> Provinces { get; set; }
        public DbSet<District> Districts { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<ApiKey> ApiKeys { get; set; }
        public DbSet<ApiUsageTracking> ApiUsageTrackings { get; set; }
        public DbSet<Ward> Wards { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<CompanyBusinessMapping> CompanyBusinessMappings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // Seed data for testing
            SeedData(modelBuilder);

            // Configure Business entity
            modelBuilder.Entity<Business>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Code).IsUnique();
            });

            // Configure CompanyStatus entity
            modelBuilder.Entity<CompanyStatus>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Code).IsUnique();
            });

            // Configure CrawlerJob entity
            modelBuilder.Entity<CrawlerJob>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Progress).HasDefaultValue(0.0f);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            // Configure CrawlingStatus entity
            modelBuilder.Entity<CrawlingStatus>(entity =>
            {
                entity.HasKey(e => e.Id);
            });

            // Configure Province entity
            modelBuilder.Entity<Province>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Code).IsUnique();
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("getdate()");
                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("getdate()");
            });

            // Configure District entity
            modelBuilder.Entity<District>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Code).IsUnique();
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("getdate()");
                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("getdate()");
                
                entity.HasOne(d => d.Province)
                    .WithMany(p => p.Districts)
                    .HasForeignKey(d => d.ProvinceId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            // Configure User entity
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.IsActive).HasDefaultValue(true);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("getdate()");
                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("getdate()");
            });

            // Configure ApiKey entity
            modelBuilder.Entity<ApiKey>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Key).IsUnique();
                entity.Property(e => e.Plan).HasDefaultValue("free");
                entity.Property(e => e.RequestLimit).HasDefaultValue(500);
                entity.Property(e => e.RequestsUsed).HasDefaultValue(0);
                entity.Property(e => e.IsActive).HasDefaultValue(true);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("getdate()");
                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("getdate()");
                
                entity.HasOne(a => a.User)
                    .WithMany(u => u.ApiKeys)
                    .HasForeignKey(a => a.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            // Configure ApiUsageTracking entity
            modelBuilder.Entity<ApiUsageTracking>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.CalledAt).HasDefaultValueSql("getdate()");
                
                entity.HasOne(a => a.ApiKey)
                    .WithMany(k => k.ApiUsageTrackings)
                    .HasForeignKey(a => a.ApiKeyId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            // Configure Ward entity
            modelBuilder.Entity<Ward>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Code).IsUnique();
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("getdate()");
                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("getdate()");
                
                entity.HasOne(w => w.District)
                    .WithMany(d => d.Wards)
                    .HasForeignKey(w => w.DistrictId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
                
                entity.HasOne(w => w.Province)
                    .WithMany(p => p.Wards)
                    .HasForeignKey(w => w.ProvinceId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            // Configure Company entity
            modelBuilder.Entity<Company>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.TaxCode).IsUnique();
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("getdate()");
                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("getdate()");
                
                entity.HasOne(c => c.Province)
                    .WithMany(p => p.Companies)
                    .HasForeignKey(c => c.ProvinceId);
                
                entity.HasOne(c => c.District)
                    .WithMany(d => d.Companies)
                    .HasForeignKey(c => c.DistrictId);
                
                entity.HasOne(c => c.Ward)
                    .WithMany(w => w.Companies)
                    .HasForeignKey(c => c.WardId);
                
                entity.HasOne(c => c.Status)
                    .WithMany(s => s.Companies)
                    .HasForeignKey(c => c.StatusId);
            });

            // Configure CompanyBusinessMapping entity
            modelBuilder.Entity<CompanyBusinessMapping>(entity =>
            {
                entity.HasKey(e => new { e.CompanyId, e.BusinessId });
                
                entity.HasOne(cbm => cbm.Company)
                    .WithMany(c => c.CompanyBusinessMappings)
                    .HasForeignKey(cbm => cbm.CompanyId)
                    .OnDelete(DeleteBehavior.Cascade);
                
                entity.HasOne(cbm => cbm.Business)
                    .WithMany(b => b.CompanyBusinessMappings)
                    .HasForeignKey(cbm => cbm.BusinessId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }

        public override int SaveChanges()
        {
            UpdateTimestamps();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateTimestamps();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateTimestamps()
        {
            var entities = ChangeTracker.Entries()
                .Where(x => x.Entity is Company || x.Entity is Business || x.Entity is Province || 
                           x.Entity is District || x.Entity is Ward || x.Entity is User || 
                           x.Entity is ApiKey || x.Entity is CrawlerJob)
                .Where(x => x.State == EntityState.Added || x.State == EntityState.Modified);

            foreach (var entity in entities)
            {
                if (entity.State == EntityState.Added)
                {
                    // Handle CreatedAt for different entity types
                    if (entity.Entity is Company company && company.CreatedAt == null)
                        company.CreatedAt = DateTime.UtcNow;
                    else if (entity.Entity is Business business && business.CreatedAt == null)
                        business.CreatedAt = DateTime.UtcNow;
                    else if (entity.Entity is Province province && province.CreatedAt == null)
                        province.CreatedAt = DateTime.UtcNow;
                    else if (entity.Entity is District district && district.CreatedAt == null)
                        district.CreatedAt = DateTime.UtcNow;
                    else if (entity.Entity is Ward ward && ward.CreatedAt == null)
                        ward.CreatedAt = DateTime.UtcNow;
                    else if (entity.Entity is CrawlerJob job && job.CreatedAt == null)
                        job.CreatedAt = DateTime.UtcNow;
                }

                // Handle UpdatedAt for different entity types
                if (entity.Entity is Company c)
                    c.UpdatedAt = DateTime.UtcNow;
                else if (entity.Entity is Business b)
                    b.UpdatedAt = DateTime.UtcNow;
                else if (entity.Entity is Province p)
                    p.UpdatedAt = DateTime.UtcNow;
                else if (entity.Entity is District d)
                    d.UpdatedAt = DateTime.UtcNow;
                else if (entity.Entity is Ward w)
                    w.UpdatedAt = DateTime.UtcNow;
                else if (entity.Entity is CrawlerJob j)
                    j.UpdatedAt = DateTime.UtcNow;
                else if (entity.Entity is User u)
                    u.UpdatedAt = DateTime.UtcNow;
                else if (entity.Entity is ApiKey a)
                    a.UpdatedAt = DateTime.UtcNow;
            }
        }
        
        private void SeedData(ModelBuilder modelBuilder)
        {
            // Seed Provinces
            modelBuilder.Entity<Province>().HasData(
                new Province { Id = 1, Name = "Hà Nội", Code = "HN", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new Province { Id = 2, Name = "Hồ Chí Minh", Code = "HCM", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new Province { Id = 3, Name = "Đà Nẵng", Code = "DN", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }
            );

            // Seed Districts
            modelBuilder.Entity<District>().HasData(
                new District { Id = 1, ProvinceId = 1, Name = "Ba Đình", Code = "BD", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new District { Id = 2, ProvinceId = 1, Name = "Hoàn Kiếm", Code = "HK", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new District { Id = 3, ProvinceId = 2, Name = "Quận 1", Code = "Q1", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new District { Id = 4, ProvinceId = 2, Name = "Quận 2", Code = "Q2", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new District { Id = 5, ProvinceId = 3, Name = "Hải Châu", Code = "HC", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }
            );

            // Seed Wards
            modelBuilder.Entity<Ward>().HasData(
                new Ward { Id = 1, ProvinceId = 1, DistrictId = 1, Name = "Phúc Xá", Code = "PX", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new Ward { Id = 2, ProvinceId = 1, DistrictId = 1, Name = "Trúc Bạch", Code = "TB", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new Ward { Id = 3, ProvinceId = 2, DistrictId = 3, Name = "Bến Nghé", Code = "BN", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new Ward { Id = 4, ProvinceId = 3, DistrictId = 5, Name = "Hải Châu 1", Code = "HC1", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }
            );

            // Seed Company Statuses
            modelBuilder.Entity<CompanyStatus>().HasData(
                new CompanyStatus { Id = 1, Name = "Đang hoạt động", Code = "ACTIVE" },
                new CompanyStatus { Id = 2, Name = "Tạm ngừng hoạt động", Code = "SUSPENDED" },
                new CompanyStatus { Id = 3, Name = "Đã giải thể", Code = "DISSOLVED" }
            );

            // Seed Business Categories
            modelBuilder.Entity<Business>().HasData(
                new Business { 
                    Id = 1, 
                    Name = "Bán lẻ", 
                    Code = "RETAIL", 
                    RootCode = "RETAIL", 
                    Description = "Kinh doanh bán lẻ", 
                    Vsic2007Code = "47", 
                    Vsic2007RootCode = "47", 
                    Vsic2007Name = "Bán buôn và bán lẻ; sửa chữa ô tô, mô tô, xe máy và xe có động cơ khác", 
                    CreatedAt = DateTime.UtcNow, 
                    UpdatedAt = DateTime.UtcNow 
                },
                new Business { 
                    Id = 2, 
                    Name = "Công nghệ thông tin", 
                    Code = "IT", 
                    RootCode = "IT", 
                    Description = "Dịch vụ CNTT", 
                    Vsic2007Code = "62", 
                    Vsic2007RootCode = "62", 
                    Vsic2007Name = "Lập trình máy vi tính, tư vấn và các hoạt động liên quan", 
                    CreatedAt = DateTime.UtcNow, 
                    UpdatedAt = DateTime.UtcNow 
                },
                new Business { 
                    Id = 3, 
                    Name = "Vận tải", 
                    Code = "TRANSPORT", 
                    RootCode = "TRANSPORT", 
                    Description = "Dịch vụ vận tải", 
                    Vsic2007Code = "49", 
                    Vsic2007RootCode = "49", 
                    Vsic2007Name = "Vận tải đường bộ và vận tải đường ống", 
                    CreatedAt = DateTime.UtcNow, 
                    UpdatedAt = DateTime.UtcNow 
                }
            );

            // Seed Users
            modelBuilder.Entity<User>().HasData(
                new User 
                { 
                    Id = 1, 
                    Email = "admin@example.com", 
                    PasswordHash = "AQAAAAEAACcQAAAAEGUQUy3d6YXsHi/xDYjYUPQHbVXx3dMa6OIwfR4vTXfkM2FBMeXmCkJvk1RgtS7eoQ==", // hashed "Admin@123"
                    Name = "Admin User", 
                    IsActive = true, 
                    CreatedAt = DateTime.UtcNow, 
                    UpdatedAt = DateTime.UtcNow 
                },
                new User 
                { 
                    Id = 2, 
                    Email = "user@example.com", 
                    PasswordHash = "AQAAAAEAACcQAAAAEI/8k8nWCpbLtmBnhB8/h1x+a8ZXgMK7JUu9sIgaQfssgz1FKzFIQQCW/VnCh9+gTw==", // hashed "User@123" 
                    Name = "Regular User", 
                    IsActive = true, 
                    CreatedAt = DateTime.UtcNow, 
                    UpdatedAt = DateTime.UtcNow 
                }
            );

            // Seed Companies
            modelBuilder.Entity<Company>().HasData(
                new Company 
                { 
                    Id = 1, 
                    Name = "Công ty TNHH ABC", 
                    AlternateName = "ABC Company Limited",
                    TaxCode = "0123456789", 
                    Address = "123 Đường A, Phường Phúc Xá", 
                    ProvinceId = 1, 
                    DistrictId = 1, 
                    WardId = 1, 
                    StatusId = 1, 
                    PhoneNumber = "0987654321", 
                    Email = "contact@abc.com", 
                    Website = "https://abc.com", 
                    LegalRepresentative = "Nguyễn Văn A", 
                    BusinessType = "Công ty TNHH", 
                    FoundedYear = 2015, 
                    EmployeeCount = 50, 
                    Capital = 5000000000, 
                    CreatedAt = DateTime.UtcNow, 
                    UpdatedAt = DateTime.UtcNow 
                },
                new Company 
                { 
                    Id = 2, 
                    Name = "Công ty Cổ phần XYZ", 
                    AlternateName = "XYZ Joint Stock Company",
                    TaxCode = "9876543210", 
                    Address = "456 Đường B, Phường Bến Nghé", 
                    ProvinceId = 2, 
                    DistrictId = 3, 
                    WardId = 3, 
                    StatusId = 1, 
                    PhoneNumber = "0123456789", 
                    Email = "contact@xyz.com", 
                    Website = "https://xyz.com", 
                    LegalRepresentative = "Trần Thị B", 
                    BusinessType = "Công ty Cổ phần", 
                    FoundedYear = 2010, 
                    EmployeeCount = 120, 
                    Capital = 20000000000, 
                    CreatedAt = DateTime.UtcNow, 
                    UpdatedAt = DateTime.UtcNow 
                }
            );

            // Seed Company-Business Mappings
            modelBuilder.Entity<CompanyBusinessMapping>().HasData(
                new CompanyBusinessMapping { CompanyId = 1, BusinessId = 1 },
                new CompanyBusinessMapping { CompanyId = 1, BusinessId = 2 },
                new CompanyBusinessMapping { CompanyId = 2, BusinessId = 2 },
                new CompanyBusinessMapping { CompanyId = 2, BusinessId = 3 }
            );

            // Seed API Keys for testing
            modelBuilder.Entity<ApiKey>().HasData(
                new ApiKey 
                { 
                    Id = 1, 
                    UserId = 1, 
                    Key = "api_key_admin_" + Guid.NewGuid().ToString("N"), 
                    Plan = "premium", 
                    RequestLimit = 10000, 
                    RequestsUsed = 0, 
                    IsActive = true, 
                    CreatedAt = DateTime.UtcNow, 
                    UpdatedAt = DateTime.UtcNow 
                },
                new ApiKey 
                { 
                    Id = 2, 
                    UserId = 2, 
                    Key = "api_key_user_" + Guid.NewGuid().ToString("N"), 
                    Plan = "free", 
                    RequestLimit = 500, 
                    RequestsUsed = 0, 
                    IsActive = true, 
                    CreatedAt = DateTime.UtcNow, 
                    UpdatedAt = DateTime.UtcNow 
                }
            );
        }
    }
}
