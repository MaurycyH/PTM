using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PTM.Entities;
using PTM.Logic;
using System;
using System.Linq;
using PTM.TestCommon;

namespace Test.PTM.Logic
{
    [TestClass]
    public class TestUserLogic
    {
        /// <summary>
        /// Sprawdza czy użytkownik jest tworzony
        /// </summary>
        [TestMethod]
        public void Create_CreatesUser_UserIsCreated()
        {
            //Arrange
            DbContextOptions<TestDatabaseContext> options = new DbContextOptionsBuilder<TestDatabaseContext>()
               .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
               .Options;
            TestDatabaseContext context = new TestDatabaseContext(options);

            UserLogic userLogic = new UserLogic(context);

            User user1 = new User()
            {
                FirstName = "Jan",
                LastName = "Kowalski",
            };
            User user2 = new User()
            {
                FirstName = "John",
                LastName = "Smith",
            };

            //Act
            userLogic.Create(user1);
            userLogic.Create(user2);
            context.SaveChanges();

            //Assert
            context.Users.Where(user => user.ID == 1).First().Should().Be(user1);
            context.Users.Where(user => user.ID == 2).First().Should().Be(user2);
        }

        /// <summary>
        /// Sprawdza czy użytkownik odczytywany
        /// </summary>
        [TestMethod]
        public void Read_ReadsUser_UserIsLoaded()
        {
            //Arrange
            DbContextOptions<TestDatabaseContext> options = new DbContextOptionsBuilder<TestDatabaseContext>()
               .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
               .Options;
            TestDatabaseContext context = new TestDatabaseContext(options);

            UserLogic userLogic = new UserLogic(context);

            User user1 = new User()
            {
                FirstName = "Jan",
                LastName = "Kowalski",
            };
            User user2 = new User()
            {
                FirstName = "John",
                LastName = "Smith",
            };

            context.Users.AddRange(user1, user2);
            context.SaveChanges();

            //Act, Assert
            userLogic.Read(1).Should().Be(user1);
            userLogic.Read(2).Should().Be(user2);
        }

        /// <summary>
        /// Sprawdza czy użytkownik jest modyfikowany
        /// </summary>
        [TestMethod]
        public void Update_UpdatesUser_UserIsUpdated()
        {
            //Arrange
            DbContextOptions<TestDatabaseContext> options = new DbContextOptionsBuilder<TestDatabaseContext>()
               .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
               .Options;
            TestDatabaseContext context = new TestDatabaseContext(options);
            context.Users.Add(new User()
            {
                FirstName = "Jan",
                LastName = "Kowalski",
            });
            context.SaveChanges();

            UserLogic userLogic = new UserLogic(context);

            //Act
            User user = userLogic.Read(1);
            user.FirstName = "Janusz";
            userLogic.Update(user);

            //Assert
            userLogic.Read(1).FirstName.Should().Be("Janusz");
        }

        /// <summary>
        /// Sprawdza czy użytkownik jest modyfikowany
        /// </summary>
        [TestMethod]
        public void Delete_DeletesUser_UserIsDeleted()
        {
            //Arrange
            DbContextOptions<TestDatabaseContext> options = new DbContextOptionsBuilder<TestDatabaseContext>()
               .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
               .Options;
            TestDatabaseContext context = new TestDatabaseContext(options);
            context.Users.Add(new User()
            {
                FirstName = "Jan",
                LastName = "Kowalski",
            });
            context.SaveChanges();

            UserLogic userLogic = new UserLogic(context);

            //Act
            userLogic.Delete(1);
            context.SaveChanges();

            //Assert
            userLogic.Read(1).Should().BeNull();
        }
    }
}
