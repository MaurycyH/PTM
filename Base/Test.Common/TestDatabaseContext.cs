using Microsoft.EntityFrameworkCore;
using PTM.Entities;
using PTM.Logic;

namespace PTM.TestCommon
{
    /// <summary>
    /// Testowy kontekst bazy danych
    /// </summary>
    public class TestDatabaseContext : DbContext, IDatabaseContext
    {
        public DbSet<WorkItem> WorkItems { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<WorkItemCollection> WorkItemCollections { get; set; }
        public DbSet<TaskBoard> TaskBoards { get; set; }

        public TestDatabaseContext(DbContextOptions<TestDatabaseContext> options) : base(options)
        {

        }
    }

}
