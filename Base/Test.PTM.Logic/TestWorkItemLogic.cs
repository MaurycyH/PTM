using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PTM.Entities;
using PTM.Logic;
using PTM.TestCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test.PTM.Logic
{
    [TestClass]
    public class TestWorkItemLogic
    {
        /// <summary>
        /// Na poprawnym itemie zostaje utworzone entry w DB
        /// </summary>
        [TestMethod]
        public void CreateWorkItem_OnWalidWorkItem_CreatesEntryInDB()
        {
            // ARRANGE
            DbContextOptions<TestDatabaseContext> options = new DbContextOptionsBuilder<TestDatabaseContext>()
               .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
               .Options;
            IDatabaseContext context = new TestDatabaseContext(options);
            WorkItemLogic logic = new WorkItemLogic(context);
            WorkItem request = new WorkItem()
            {
                Name = Guid.NewGuid().ToString(),
                Color = Guid.NewGuid().ToString(),
                Description = Guid.NewGuid().ToString(),
                WorkItemCollectionId = 1,
                WorkItemStart = DateTime.Now.AddDays(-1),
                WorkItemEnd = DateTime.Now
            };

            context.WorkItemCollections.Add(new WorkItemCollection()
            {
                ID = 1,
                TaskBoardId = 1
            });

            context.TaskBoards.Add(new TaskBoard()
            {
                ID = 1,
                UserID = 1
            });

            context.Users.Add(new User()
            {
                ID = 1
            });

            context.SaveChanges();

            // ACT
            WorkItem result = logic.CreateWorkItem(request);
            context.SaveChanges();

            request.ID = result.ID;

            // ASSERT
            result.Should().BeEquivalentTo<WorkItem>(request);
        }

        /// <summary>
        /// Dla nieprawidłowego ID taskboardu rzucamy wyjątkiem
        /// </summary>
        [TestMethod]
        public void CreateWorkItem_OnNonExistingCollection_ThrowsArgumentException()
        {
            // ARRANGE
            DbContextOptions<TestDatabaseContext> options = new DbContextOptionsBuilder<TestDatabaseContext>()
               .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
               .Options;
            IDatabaseContext context = new TestDatabaseContext(options);
            WorkItemLogic logic = new WorkItemLogic(context);
            WorkItem request = new WorkItem()
            {
                WorkItemCollectionId = 1
            };

            context.SaveChanges();

            // ACT
            Action act = () => logic.CreateWorkItem(request);

            // ASSERT
            act.Should().Throw<ArgumentException>().Where(e => e.Message.Contains("doesn't exist"));
        }

        /// <summary>
        /// Dla błędnego ID rzucamy wyjątkiem
        /// </summary>
        [TestMethod]
        public void CreateWorkItem_OnInvalidID_ThrowsArgumentException()
        {
            // ARRANGE
            DbContextOptions<TestDatabaseContext> options = new DbContextOptionsBuilder<TestDatabaseContext>()
               .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
               .Options;
            IDatabaseContext context = new TestDatabaseContext(options);
            WorkItemLogic logic = new WorkItemLogic(context);
            WorkItem request = new WorkItem()
            {
                ID = 123
            };

            context.SaveChanges();

            // ACT
            Action act = () => logic.CreateWorkItem(request);

            // ASSERT
            act.Should().Throw<ArgumentException>().Where(e => e.Message.Contains("ID can't be set on entity"));
        }

        /// <summary>
        /// Dla błędnego ID rzucamy wyjątkiem
        /// </summary>
        [TestMethod]
        public void GetWorkItem_OnInvalidID_ThrowsArgumentException()
        {
            // ARRANGE
            DbContextOptions<TestDatabaseContext> options = new DbContextOptionsBuilder<TestDatabaseContext>()
               .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
               .Options;
            IDatabaseContext context = new TestDatabaseContext(options);
            WorkItemLogic logic = new WorkItemLogic(context);
      
            // ACT
            Action act = () => logic.GetWorkItem(0);

            // ASSERT
            act.Should().Throw<ArgumentException>().Where(e => e.Message.Contains("grater than 0"));
        }

        /// <summary>
        /// Dla błędnego ID rzucamy wyjątkiem
        /// </summary>
        [TestMethod]
        public void GetWorkItem_OnValidID_ReturnsEntity()
        {
            // ARRANGE
            DbContextOptions<TestDatabaseContext> options = new DbContextOptionsBuilder<TestDatabaseContext>()
               .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
               .Options;
            IDatabaseContext context = new TestDatabaseContext(options);
            WorkItemLogic logic = new WorkItemLogic(context);
            context.WorkItems.Add(new WorkItem()
            {
                ID = 1
            });

            // ACT
            WorkItem result = logic.GetWorkItem(1);

            // ASSERT
            result.Should().BeEquivalentTo<WorkItem>(result);
        }

        /// <summary>
        /// Dla błędnego ID rzucamy wyjątkiem
        /// </summary>
        [TestMethod]
        public void UpdateWorkItem_OnValidWorkItem_UpdatesEntity()
        {
            // ARRANGE
            DbContextOptions<TestDatabaseContext> options = new DbContextOptionsBuilder<TestDatabaseContext>()
               .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
               .Options;
            IDatabaseContext context = new TestDatabaseContext(options);
            WorkItemLogic logic = new WorkItemLogic(context);

            WorkItemCollection wic = new WorkItemCollection()
            {
                ID = 1,
                TaskBoardId = 1
            };

            context.WorkItems.Add(new WorkItem()
            {
                ID = 1,
                WorkItemCollectionId = 1
            });

            context.WorkItemCollections.Add(wic);

            context.TaskBoards.Add(new TaskBoard()
            {
                ID = 1,
                UserID = 1
            });

            context.Users.Add(new User()
            {
                ID = 1
            });

            context.SaveChanges();

            WorkItem request = new WorkItem()
            {
                ID = 1,
                Name = Guid.NewGuid().ToString(),
                Color = Guid.NewGuid().ToString(),
                Description = Guid.NewGuid().ToString(),
                WorkItemCollectionId = 1,
                WorkItemCollection = wic,
                WorkItemStart = DateTime.Now.AddDays(-1),
                WorkItemEnd = DateTime.Now
            };

            // ACT
            logic.UpdateWorkItem(request);
            WorkItem result = logic.GetWorkItem(1);

            // ASSERT
            result.Should().BeEquivalentTo<WorkItem>(request);
        }

        /// <summary>
        /// Dla błędnego ID rzucamy wyjątkiem
        /// </summary>
        [TestMethod]
        public void UpdateWorkItem_OnInValidWorkItemID_ThrowsArgumentException()
        {
            // ARRANGE
            DbContextOptions<TestDatabaseContext> options = new DbContextOptionsBuilder<TestDatabaseContext>()
               .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
               .Options;
            IDatabaseContext context = new TestDatabaseContext(options);
            WorkItemLogic logic = new WorkItemLogic(context);

            WorkItem request = new WorkItem()
            {
                ID = 0,
            };

            // ACT
            Action act = () => logic.UpdateWorkItem(request);

            // ASSERT
            act.Should().Throw<ArgumentException>().Where(e => e.Message.Contains("grater than 0"));
        }

        /// <summary>
        /// Dla nieistniejącego WI rzucamy wyjątek
        /// </summary>
        [TestMethod]
        public void UpdateWorkItem_OnNonExistingWorkItem_ThrowsArgumentException()
        {
            // ARRANGE
            DbContextOptions<TestDatabaseContext> options = new DbContextOptionsBuilder<TestDatabaseContext>()
               .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
               .Options;
            IDatabaseContext context = new TestDatabaseContext(options);
            WorkItemLogic logic = new WorkItemLogic(context);

            WorkItem request = new WorkItem()
            {
                ID = 1,
            };

            // ACT
            Action act = () => logic.UpdateWorkItem(request);

            // ASSERT
            act.Should().Throw<ArgumentException>().Where(e => e.Message.Contains("does not exist"));
        }

        /// <summary>
        /// Dla błędnego ID rzucamy wyjątkiem
        /// </summary>
        [TestMethod]
        public void DeleteWorkItem_OnInValidWorkItemID_ThrowsArgumentException()
        {
            // ARRANGE
            DbContextOptions<TestDatabaseContext> options = new DbContextOptionsBuilder<TestDatabaseContext>()
               .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
               .Options;
            IDatabaseContext context = new TestDatabaseContext(options);
            WorkItemLogic logic = new WorkItemLogic(context);

            WorkItem request = new WorkItem()
            {
                ID = 0,
            };

            // ACT
            Action act = () => logic.UpdateWorkItem(request);

            // ASSERT
            act.Should().Throw<ArgumentException>().Where(e => e.Message.Contains("grater than 0"));
        }

        /// <summary>
        /// Sprawdza, czy WI został usunięty.
        /// </summary>
        [TestMethod]
        public void DeleteWorkItem_OnValidRequest_DeletesEntry()
        {
            // ARRANGE
            DbContextOptions<TestDatabaseContext> options = new DbContextOptionsBuilder<TestDatabaseContext>()
               .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
               .Options;
            IDatabaseContext context = new TestDatabaseContext(options);
            WorkItemLogic logic = new WorkItemLogic(context);

            WorkItem request = new WorkItem()
            {
                ID = 1,
            };

            context.WorkItems.Add(request);
            context.SaveChanges();

            // ACT
            logic.DeleteWorkItem(1);
            context.SaveChanges();

            // ASSERT
            context.WorkItems.Any(wi => wi.ID == 1).Should().BeFalse();
        }
    }
}
