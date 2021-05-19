using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Org.DAL.MySql
{
    internal class EmployeeDesignContextFactory : IDesignTimeDbContextFactory<EmployeContext>
    {
        public EmployeContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<EmployeContext>();
            
            optionsBuilder.UseMySql("server=localhost;database=EmployePortalData;allowuservariables=True;user id=root;password=5656");
            
            return new EmployeContext(optionsBuilder.Options);
        }
    }
}