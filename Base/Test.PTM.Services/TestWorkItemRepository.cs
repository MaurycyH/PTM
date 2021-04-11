using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PTM.Entities;
using PTM.Logic;
using PTM.Logic.ModelConverters;
using PTM.PublicDataModel;
using PTM.Services.WorkItems;
using PTM.TestCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test.PTM.Services
{
    [TestClass]
    public class TestWorkItemRepository
    {
        /// <summary>
        /// Sprawdza, czy item zostaje stworzony poprawnie
        /// </summary>
        [TestMethod]
        public void CreateWorkItem_OnValidRequest_CreatesWorkItem()
        {
            // ARRANGE
            DbContextOptions<TestDatabaseContext> options = new DbContextOptionsBuilder<TestDatabaseContext>()
             .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
             .Options;
            IDatabaseContext dbContext = new TestDatabaseContext(options);
            WorkItemRepository repository = new WorkItemRepository(dbContext);

            WorkItemPublic workItem = new WorkItemPublic()
            {
                Color = Guid.NewGuid().ToString(),
                WorkItemCollectionID = 1,
                Description = Guid.NewGuid().ToString(),
                Name = Guid.NewGuid().ToString(),
                WorkItemEnd = DateTime.Now,
                WorkItemStart = DateTime.Now
            };

            dbContext.WorkItemCollections.Add(new WorkItemCollection()
            {
                ID = 1,
                TaskBoardId = 1
            });

            dbContext.TaskBoards.Add(new TaskBoard()
            {
                ID = 1,
                UserID = 1
            });

            dbContext.Users.Add(new User()
            {
                ID = 1
            });

            dbContext.SaveChanges();

            // ACT
            WorkItemPublic result = repository.CreateWorkItem(workItem);
            workItem.ID = result.ID;

            // ASSERT
            result.Should().BeEquivalentTo<WorkItemPublic>(workItem);
        }

        /// <summary>
        /// Sprawdza czy poprawnie zwraca item
        /// </summary>
        [TestMethod]
        public void GetWorkItem_OnValidRequest_ReturnsWorkItem()
        {
            // ARRANGE
            DbContextOptions<TestDatabaseContext> options = new DbContextOptionsBuilder<TestDatabaseContext>()
             .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
             .Options;
            IDatabaseContext dbContext = new TestDatabaseContext(options);
            WorkItemRepository repository = new WorkItemRepository(dbContext);
            WorkItemConverter converter = new WorkItemConverter(dbContext);

            WorkItem workItem = new WorkItem()
            {
                Description = Guid.NewGuid().ToString(),
                Name = Guid.NewGuid().ToString(),
                WorkItemEnd = DateTime.Now,
                WorkItemStart = DateTime.Now,
                WorkItemCollectionId = 1
            };

            dbContext.WorkItems.Add(workItem);

            dbContext.SaveChanges();

            // ACT
            WorkItemPublic result = repository.GetWorkItem(1);

            // ASSERT
            result.Should().BeEquivalentTo<WorkItemPublic>(converter.Convert(workItem));
        }

        [TestMethod]
        public void UpdateWorkItem_OnValidRequest_UpdatesWorkItem()
        {
            // ARRANGE
            DbContextOptions<TestDatabaseContext> options = new DbContextOptionsBuilder<TestDatabaseContext>()
             .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
             .Options;
            IDatabaseContext dbContext = new TestDatabaseContext(options);
            WorkItemRepository repository = new WorkItemRepository(dbContext);
            WorkItemPublic workItem = new WorkItemPublic()
            {
                ID = 1,
                Description = Guid.NewGuid().ToString(),
                Name = Guid.NewGuid().ToString(),
                WorkItemEnd = DateTime.Now,
                WorkItemStart = DateTime.Now,
                WorkItemCollectionID = 1
            };

            dbContext.WorkItems.Add(new WorkItem()
            { 
                ID = 1
            });

            dbContext.WorkItemCollections.Add(new WorkItemCollection()
            {
                ID = 1,
                TaskBoardId = 1
            });

            dbContext.TaskBoards.Add(new TaskBoard()
            {
                ID = 1,
                UserID = 1
            });

            dbContext.Users.Add(new User()
            {
                ID = 1
            });

            dbContext.SaveChanges();

            // ACT
            WorkItemPublic result = repository.UpdateWorkItem(workItem);

            // ASSERT
            result.Should().BeEquivalentTo<WorkItemPublic>(workItem);
        }

        [TestMethod]
        public void DeleteWorkItem_OnValidRequest_DeletesWorkItem()
        {
            // ARRANGE
            DbContextOptions<TestDatabaseContext> options = new DbContextOptionsBuilder<TestDatabaseContext>()
             .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
             .Options;
            IDatabaseContext dbContext = new TestDatabaseContext(options);
            WorkItemRepository repository = new WorkItemRepository(dbContext);
            dbContext.WorkItems.Add(new WorkItem()
            {
                ID = 1
            });

            dbContext.SaveChanges();

            // ACT
            repository.DeleteWorkItem(1);

            // ASSERT
            dbContext.WorkItems.Any(wi => wi.ID == 1).Should().BeFalse();
        }
    }
}
