using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PTM.Entities;
using PTM.Logic;
using PTM.Logic.ModelConverters;
using PTM.PublicDataModel;
using System;
using PTM.TestCommon;

namespace Test.PTM.Logic.ModelConverters
{
    [TestClass]
    public class TestTaskBoardConverter
    {
        /// <summary>
        /// Sprawdza, czy konwersja między Entity a Serwisem zachodzi poprawnie
        /// </summary>
        [TestMethod]
        public void Convert_ConvertsEntityToServiceModel_ReturnsConvertedModel()
        {
            // ARRANGE
            DbContextOptions<TestDatabaseContext> options = new DbContextOptionsBuilder<TestDatabaseContext>()
               .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
               .Options;
            IDatabaseContext dbContext = new TestDatabaseContext(options);
            TaskBoardConverter converter = new TaskBoardConverter(dbContext);
            TaskBoard taskBoard = new TaskBoard()
            {
                ID = 1,
                Name = "Test",
                UserID = 1
            };

            // ACT
            TaskBoardPublic taskBoardPublic = converter.Convert(taskBoard);

            // ASSERT
            taskBoardPublic.Should().BeEquivalentTo<TaskBoardPublic>(new TaskBoardPublic() { ID = 1, Name = "Test", UserID = 1 });
        }

        /// <summary>
        /// Sprawdza, czy konwersja między serwisem a entity zachodzi poprawnie
        /// </summary>
        [TestMethod]
        public void Convert_ConvertsNonExistingServiceModelToEntity_ReturnsConvertedEntity()
        {
            // ARRANGE
            DbContextOptions<TestDatabaseContext> options = new DbContextOptionsBuilder<TestDatabaseContext>()
               .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
               .Options;
            IDatabaseContext dbContext = new TestDatabaseContext(options);
            TaskBoardConverter converter = new TaskBoardConverter(dbContext);
            TaskBoardPublic taskBoardPublic = new TaskBoardPublic()
            {
                ID = 1,
                Name = "Test",
                UserID = 1
            };

            // ACT
            TaskBoard taskBoard = converter.Convert(taskBoardPublic);

            // ASSERT
            taskBoard.Should().BeEquivalentTo<TaskBoard>(new TaskBoard() { ID = 0, Name = "Test", UserID = 1 });
            taskBoard.User.Should().BeNull();
        }

        /// <summary>
        /// Sprawdza, czy konwersja między serwisem a entity zachodzi poprawnie
        /// </summary>
        [TestMethod]
        public void Convert_ConvertsExistingServiceModelToEntity_ReturnsConvertedEntity()
        {
            // ARRANGE
            DbContextOptions<TestDatabaseContext> options = new DbContextOptionsBuilder<TestDatabaseContext>()
               .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
               .Options;
            IDatabaseContext dbContext = new TestDatabaseContext(options);

            User user = new User()
            {
                ID = 1,
                FirstName = Guid.NewGuid().ToString(),
                LastName = Guid.NewGuid().ToString()
            };

            dbContext.TaskBoards.Add(new TaskBoard() { ID = 1 });
            dbContext.Users.Add(user);
            dbContext.SaveChanges();

            TaskBoardConverter converter = new TaskBoardConverter(dbContext);
            TaskBoardPublic taskBoardPublic = new TaskBoardPublic()
            {
                ID = 1,
                Name = "Test",
                UserID = 1
            };

            // ACT
            TaskBoard taskBoard = converter.Convert(taskBoardPublic);

            // ASSERT
            taskBoard.Should().BeEquivalentTo<TaskBoard>(new TaskBoard() { ID = 1, Name = "Test", UserID = 1, User = user });
            taskBoard.User.Should().NotBeNull();
        }
    }
}
