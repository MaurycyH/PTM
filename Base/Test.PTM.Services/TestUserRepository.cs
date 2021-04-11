using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PTM.Entities;
using PTM.Logic;
using PTM.Logic.ModelConverters;
using PTM.PublicDataModel;
using PTM.Services.Users;
using PTM.TestCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test.PTM.Services
{
    [TestClass]
    public class TestUserRepository
    {
        /// <summary>
        /// Dla nieistniejącego usera spodziewamy się usera z przyznanym automatycznie ID
        /// </summary>
        [TestMethod]
        public void CreateUser_OnNonExistingUser_ReturnsUserWithID()
        {
            // ARRANGE 
            DbContextOptions<TestDatabaseContext> options = new DbContextOptionsBuilder<TestDatabaseContext>()
               .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
               .Options;
            IDatabaseContext dbContext = new TestDatabaseContext(options);
            UserRepository repository = new UserRepository(dbContext);

            // ACT
            UserPublic userPublic = repository.CreateUser(new UserPublic { ID=99, FirstName = "Jan", LastName = "Kowalski", OAuthID = "12345" });

            // ASSERT
            userPublic.Should().BeEquivalentTo(new UserPublic { ID = 1, FirstName = "Jan", LastName = "Kowalski", OAuthID = "12345" });
        }

        /// <summary>
        /// Dla istniejącego usera spodziewamy się zwróconego obiektu
        /// </summary>
        [TestMethod]
        public void GetUser_OnExistingUser_ReturnsUser()
        {
            // ARRANGE 
            DbContextOptions<TestDatabaseContext> options = new DbContextOptionsBuilder<TestDatabaseContext>()
               .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
               .Options;
            IDatabaseContext dbContext = new TestDatabaseContext(options);
            UserRepository repository = new UserRepository(dbContext);
            UserConverter converter = new UserConverter(dbContext);
            User user = new User()
            {
                FirstName = "Jan",
                LastName = "Kowalski",
            };

            dbContext.Users.Add(user);

            dbContext.SaveChanges();

            // ACT
            UserPublic userPublic = repository.GetUser(1);

            // ASSERT
            userPublic.Should().BeEquivalentTo<UserPublic>(converter.Convert(user));
        }

        /// <summary>
        /// Dla nieistniejącego usera spodziewamy się nulla
        /// </summary>
        [TestMethod]
        public void GetUser_OnNonExistingUser_ReturnsNull()
        {
            // ARRANGE 
            DbContextOptions<TestDatabaseContext> options = new DbContextOptionsBuilder<TestDatabaseContext>()
               .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
               .Options;
            IDatabaseContext dbContext = new TestDatabaseContext(options);
            UserRepository repository = new UserRepository(dbContext);

            // ACT
            UserPublic userPublic = repository.GetUser(1);

            // ASSERT
            userPublic.Should().BeNull();
        }

        /// <summary>
        /// Dla istniejącego usera spodziewamy się zwróconego obiektu
        /// </summary>
        [TestMethod]
        public void GetUserOAuth_OnExistingUser_ReturnsUser()
        {
            // ARRANGE 
            DbContextOptions<TestDatabaseContext> options = new DbContextOptionsBuilder<TestDatabaseContext>()
               .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
               .Options;
            IDatabaseContext dbContext = new TestDatabaseContext(options);
            UserRepository repository = new UserRepository(dbContext);
            User user = new User()
            {
                FirstName = "Jan",
                LastName = "Kowalski",
                OAuthID = "12345"
            };

            dbContext.Users.Add(user);

            dbContext.SaveChanges();

            // ACT
            UserPublic userPublic = repository.GetUserOAuth("12345");

            // ASSERT
            userPublic.Should().BeEquivalentTo(new UserPublic()
            {
                ID=1,
                FirstName = "Jan",
                LastName = "Kowalski",
                OAuthID = "12345"
            });
        }

        /// <summary>
        /// Dla nieistniejącego usera spodziewamy się nulla
        /// </summary>
        [TestMethod]
        public void GetUserOAuth_OnNonExistingUser_ReturnsNull()
        {
            // ARRANGE 
            DbContextOptions<TestDatabaseContext> options = new DbContextOptionsBuilder<TestDatabaseContext>()
               .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
               .Options;
            IDatabaseContext dbContext = new TestDatabaseContext(options);
            UserRepository repository = new UserRepository(dbContext);

            // ACT
            UserPublic userPublic = repository.GetUserOAuth("12345");

            // ASSERT
            userPublic.Should().BeNull();
        }

        /// <summary>
        /// Dla istniejącego usera spodziewamy się zwróconego obiektu
        /// </summary>
        [TestMethod]
        public void UpdateUser_OnExistingUser_UpdatesAndReturnsUser()
        {
            // ARRANGE 
            DbContextOptions<TestDatabaseContext> options = new DbContextOptionsBuilder<TestDatabaseContext>()
               .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
               .Options;
            IDatabaseContext dbContext = new TestDatabaseContext(options);
            UserRepository repository = new UserRepository(dbContext);
            User user = new User()
            {
                FirstName = "Jan",
                LastName = "Kowalski",
                OAuthID = "12345"
            };

            dbContext.Users.Add(user);

            dbContext.SaveChanges();

            // ACT
            UserPublic userPublic = new UserPublic()
            {
                ID = 1,
                FirstName = "Janusz",
                LastName = "Kowalewski",
                OAuthID = "12345"
            };
            repository.UpdateUser(userPublic);
            userPublic = repository.GetUser(1);

            // ASSERT
            userPublic.Should().BeEquivalentTo(new UserPublic()
            {
                ID = 1,
                FirstName = "Janusz",
                LastName = "Kowalewski",
                OAuthID = "12345"
            });
        }

        /// <summary>
        /// Dla nieistniejącego usera spodziewamy się nulla
        /// </summary>
        [TestMethod]
        public void UpdateUser_OnNonExistingUser_ReturnsNull()
        {
            // ARRANGE 
            DbContextOptions<TestDatabaseContext> options = new DbContextOptionsBuilder<TestDatabaseContext>()
               .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
               .Options;
            IDatabaseContext dbContext = new TestDatabaseContext(options);
            UserRepository repository = new UserRepository(dbContext);

            // ACT
            UserPublic userPublic = new UserPublic()
            {
                ID = 1,
                FirstName = "Janusz",
                LastName = "Kowalewski",
                OAuthID = "12345"
            };
            userPublic = repository.UpdateUser(userPublic);

            // ASSERT
            userPublic.Should().BeNull();
        }

        /// <summary>
        /// Dla istniejącego Usera spodziewamy się usunięcia
        /// </summary>
        [TestMethod]
        public void DeleteUser_OnExistingUser_DeletesUser()
        {
            // ARRANGE 
            DbContextOptions<TestDatabaseContext> options = new DbContextOptionsBuilder<TestDatabaseContext>()
               .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
               .Options;
            IDatabaseContext dbContext = new TestDatabaseContext(options);
            UserRepository repository = new UserRepository(dbContext);
            User user = new User()
            {
                FirstName = "Jan",
                LastName = "Kowalski",
                OAuthID = "12345"
            };

            dbContext.Users.Add(user);

            dbContext.SaveChanges();

            // ACT
            repository.DeleteUser(1);

            // ASSERT
            dbContext.Users.Count().Should().Be(0);
        }

        /// <summary>
        /// Dla nieistniejącego usera spodziewamy się niczego
        /// </summary>
        [TestMethod]
        public void DeleteUser_OnNonExistingUser_DoesNothing()
        {
            // ARRANGE 
            DbContextOptions<TestDatabaseContext> options = new DbContextOptionsBuilder<TestDatabaseContext>()
               .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
               .Options;
            IDatabaseContext dbContext = new TestDatabaseContext(options);
            UserRepository repository = new UserRepository(dbContext);
            User user = new User()
            {
                FirstName = "Jan",
                LastName = "Kowalski",
                OAuthID = "12345"
            };

            dbContext.Users.Add(user);

            dbContext.SaveChanges();

            // ACT
            repository.DeleteUser(22);

            // ASSERT
            dbContext.Users.Count().Should().Be(1);
        }
    }
}
