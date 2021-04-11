using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PTM.Entities;
using PTM.Logic;
using System.Collections.Generic;
using PTM.Logic.ModelConverters;
using PTM.PublicDataModel;
using PTM.Services.TaskBoards;
using System;
using PTM.TestCommon;

namespace Test.PTM.Services
{
    [TestClass]
    public class TestTaskBoardRepository
    {
        /// <summary>
        /// Dla istniejącego taskboardu spodziewamy się zwróconego obiektu
        /// </summary>
        [TestMethod]
        public void GetTaskBoard_OnExistingTaskBoard_ReturnsTaskBoard()
        {
            // ARRANGE 
            DbContextOptions<TestDatabaseContext> options = new DbContextOptionsBuilder<TestDatabaseContext>()
               .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
               .Options;
            IDatabaseContext dbContext = new TestDatabaseContext(options);
            TaskBoardRepository repository = new TaskBoardRepository(dbContext);
            TaskBoardConverter converter = new TaskBoardConverter(dbContext);
            TaskBoard taskBoard = new TaskBoard() 
            { 
                Name =  Guid.NewGuid().ToString(),
            };

            dbContext.TaskBoards.Add(taskBoard);

            dbContext.SaveChanges();

            // ACT
            TaskBoardPublic taskBoardPublic = repository.GetTaskBoard(1);

            // ASSERT
            taskBoardPublic.Should().BeEquivalentTo<TaskBoardPublic>(converter.Convert(taskBoard));
        }

        /// <summary>
        /// Dla nieistniejącego taskboardu spodziewamy się nulla
        /// </summary>
        [TestMethod]
        public void GetTaskBoard_OnNonExistingTaskBoard_ReturnsNull()
        {
            // ARRANGE 
            DbContextOptions<TestDatabaseContext> options = new DbContextOptionsBuilder<TestDatabaseContext>()
               .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
               .Options;
            IDatabaseContext dbContext = new TestDatabaseContext(options);
            TaskBoardRepository repository = new TaskBoardRepository(dbContext);

            // ACT
            TaskBoardPublic taskBoardPublic = repository.GetTaskBoard(1);

            // ASSERT
            taskBoardPublic.Should().BeNull();
        }

        /// <summary>
        /// Dla prawidłowego obiektu tworzy entity w DB
        /// </summary>
        [TestMethod]
        public void CreateTaskBoard_OnValidObject_CreatesEntityInDatabase()
        {
            // ARRANGE 
            DbContextOptions<TestDatabaseContext> options = new DbContextOptionsBuilder<TestDatabaseContext>()
               .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
               .Options;
            IDatabaseContext dbContext = new TestDatabaseContext(options);
            TaskBoardRepository repository = new TaskBoardRepository(dbContext);
            TaskBoardPublic taskBoardPublic = new TaskBoardPublic()
            {
                Name = Guid.NewGuid().ToString(),
                UserID = 1
            };

            // ACT
            TaskBoardPublic taskBoardCreated = repository.CreateTaskBoard(taskBoardPublic);

            // ID powinno się zwiększyć po tym jak entity framework stworzy encje w DB
            taskBoardPublic.ID = 1;

            // ASSERT
            taskBoardCreated.Should().BeEquivalentTo<TaskBoardPublic>(taskBoardPublic);
        }

        /// <summary>
        /// Sprawdza, czy są zwracane tylko taskboardy dla danego usera
        /// </summary>
        [TestMethod]
        public void GetUserTaskBoards_OnRequestingUsersTaskBoard_ReturnsOnlyUsersTaskBoards()
        {
            // ARRANGE 
            DbContextOptions<TestDatabaseContext> options = new DbContextOptionsBuilder<TestDatabaseContext>()
               .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
               .Options;
            IDatabaseContext dbContext = new TestDatabaseContext(options);
            TaskBoardRepository repository = new TaskBoardRepository(dbContext);

            User user1 = dbContext.Users.Add(new User()
            {
                FirstName = Guid.NewGuid().ToString(),
                LastName = Guid.NewGuid().ToString()
            }).Entity;

            User user2 = dbContext.Users.Add(new User()
            {
                FirstName = Guid.NewGuid().ToString(),
                LastName = Guid.NewGuid().ToString()
            }).Entity;

            dbContext.TaskBoards.Add(new TaskBoard()
            {
                Name = Guid.NewGuid().ToString(),
                User = user1
            });

            dbContext.TaskBoards.Add(new TaskBoard()
            {
                Name = Guid.NewGuid().ToString(),
                User = user1
            });

            dbContext.TaskBoards.Add(new TaskBoard()
            {
                Name = Guid.NewGuid().ToString(),
                User = user2
            });

            dbContext.SaveChanges();

            // ACT
            IEnumerable<TaskBoardPublic> boards = repository.GetUserTaskBoards(1);

            // ASSERT
            boards.Should().HaveCount(2);
        }
    }
}
