using System;
using System.Collections.Generic;
using System.Text;
using PTM.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Threading;

namespace PTM.Logic
{
    /// <summary>
    /// Interfejs kontekstu bazy danych
    /// </summary>
    public interface IDatabaseContext : IDisposable
    {
        /// <summary>
        /// WorkItemy
        /// </summary>
        public DbSet<WorkItem> WorkItems { get; set; }

        /// <summary>
        /// Użytkownicy
        /// </summary>
        public DbSet<User> Users { get; set; }

        /// <summary>
        /// Zbór zadań w ramach jednej kolekcji
        /// </summary>
        public DbSet<WorkItemCollection> WorkItemCollections { get; set; }

        /// <summary>
        /// Zbór taskboardów
        /// </summary>
        public DbSet<TaskBoard> TaskBoards { get; set; }

        /// <summary>
        /// Zapisuje asynchronicznie zmiany
        /// </summary>
        /// <returns></returns>
        public Task<int> SaveChangesAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Zapisuje zmiany
        /// </summary>
        public int SaveChanges();
    }
}
