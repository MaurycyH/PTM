using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PTM.Entities;
using PTM.Logic;
using PTM.Logic.ModelConverters;
using PTM.PublicDataModel;
using PTM.TestCommon;
using System;
using System.Collections.Generic;
using System.Text;

namespace Test.PTM.Logic.ModelConverters
{
    [TestClass]
    public class TestUserConverter
    {/// <summary>
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
            UserConverter converter = new UserConverter(dbContext);
            User user = new User()
            {
                ID = 1,
                FirstName = "Johhny",
                LastName = "Test",
                OAuthID = "12345"
            };

            // ACT
            UserPublic userPublic = converter.Convert(user);

            // ASSERT
            userPublic.Should().BeEquivalentTo<UserPublic>(new UserPublic() {
                ID = 1,
                FirstName = "Johhny",
                LastName = "Test",
                OAuthID = "12345"
            });
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
            UserConverter converter = new UserConverter(dbContext);
            UserPublic userPublic = new UserPublic()
            {
                ID = 1,
                FirstName = "Johhny",
                LastName = "Test",
                OAuthID = "12345"
            };

            // ACT
            User taskBoard = converter.Convert(userPublic);

            // ASSERT
            taskBoard.Should().BeEquivalentTo<User>(new User()
            {
                ID = 0,
                FirstName = "Johhny",
                LastName = "Test",
                OAuthID = "12345"
            });
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
                FirstName = "Johhny",
                LastName = "Test",
                OAuthID = "12345"
            };

            dbContext.Users.Add(user);
            dbContext.SaveChanges();

            UserConverter converter = new UserConverter(dbContext);
            UserPublic userPublic = new UserPublic()
            {
                ID = 1,
                FirstName = "Johhny",
                LastName = "Test",
                OAuthID = "12345"
            };

            // ACT
            User user2 = converter.Convert(userPublic);

            // ASSERT
            user2.Should().BeEquivalentTo<User>(user);
        }
    }
}
