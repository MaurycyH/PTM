using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using PTM.Entities;
using System.Runtime.CompilerServices;
using System.Configuration;
using Microsoft.Extensions.Configuration;
using Tesseract.Common;
using Microsoft.EntityFrameworkCore.Metadata;
using Castle.DynamicProxy.Generators.Emitters;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Threading;

namespace PTM.Logic
{

    /// <summary>
    /// Model bazy danych PTM
    /// </summary>
    public class PtmModel : DbContext, IDatabaseContext
    {
        /// <inheritdoc/>
        public virtual DbSet<WorkItem> WorkItems { get; set; }

        /// <inheritdoc/>
        public virtual DbSet<User> Users { get; set; }

        /// <inheritdoc/>
        public virtual DbSet<TaskBoard> TaskBoards { get; set; }

        /// <inheritdoc/>
        public virtual DbSet<WorkItemCollection> WorkItemCollections { get; set; }

        /// <inheritdoc/>
        public async override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Zapisuje zmiany w kontekście
        /// </summary>
        public override int SaveChanges() => base.SaveChanges();

        /// <summary>
        /// Właczenie LazyLoading oraz tworzenie połączenia z baza danych
        /// </summary>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            ConfigurationReader configurationReader = new ConfigurationReader();
            string connectionValue = configurationReader.Setting<string>("PtmConnection");
            optionsBuilder.UseSqlServer(connectionValue);
            optionsBuilder.UseLazyLoadingProxies();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            Ensure.ParamNotNull(modelBuilder, nameof(modelBuilder));

            modelBuilder.RemovePluralizingTableNameConvention();
        }
    }
}