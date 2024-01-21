using InkersCore.Models.AuthEntityModels;
using InkersCore.Models.EntityModels;
using Microsoft.EntityFrameworkCore;

namespace InkersCore.Infrastructure.Configurations
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options)
            : base(options)
        {
            
        }

        public DbSet<UserAccount> UserAccounts { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<AuditUpdationLog> AuditUpdationLogs { get; set; }
        public DbSet<AuditComment> AuditComments { get; set; }
        public DbSet<UserGroup> UserGroups { get; set; }
        public DbSet<UserGroupMapping> UserGroupMappings { get; set; }
        public DbSet<UserGroupPermissionMapping> UserGroupPermissionMappings { get; set; }
        public DbSet<UserPermission> UserPermissions { get; set; }
        public DbSet<UserPermissionMapping> UserPermissionMappings { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Company> Companys { get; set; }
        public DbSet<CompanyServiceMapping> CompanyServiceMappings { get; set; }
        public DbSet<Customer> Customer { get; set; }
    }
}
