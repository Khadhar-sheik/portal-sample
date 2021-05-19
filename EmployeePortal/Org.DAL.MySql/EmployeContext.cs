using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Org.DAL.MySql.Entities;

namespace Org.DAL.MySql
{
    internal class EmployeContext : IdentityDbContext<EmployeePortalUser>
    {
        public EmployeContext(DbContextOptions options) : base(options)
        {
        }
        
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Common.Domain.Leave> Leaves { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Leave>()
                .HasOne(l => l.ApprovedBy).WithMany(eu => eu.ApprovedLeaves).IsRequired(false);
        }
    }
}