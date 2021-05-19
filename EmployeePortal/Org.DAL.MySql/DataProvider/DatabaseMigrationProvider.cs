using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Org.Common.DataProvider;

namespace Org.DAL.MySql.DataProvider
{
    internal class DatabaseMigrationProvider : IDatabaseMigrationProvider
    {
        private readonly EmployeContext _context;

        public DatabaseMigrationProvider(EmployeContext context)
        {
            _context = context;
        }

        public async Task MigrateDb()
        {
            //     await _context.Database.MigrateAsync();
            await _context.Database.EnsureCreatedAsync();
        }
    }
}