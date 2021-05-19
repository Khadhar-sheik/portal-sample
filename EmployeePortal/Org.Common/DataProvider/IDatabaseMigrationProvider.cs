using System.Threading.Tasks;

namespace Org.Common.DataProvider
{
    public interface IDatabaseMigrationProvider
    {
        Task MigrateDb();
    }
}