using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PTM.PublicDataModel;
using PTM.Services.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace Test.PTM.Services
{
    [TestClass]
    public class TestUserController
    {
        [TestMethod]
        public void CreateUser_CreatesUser_ReturnsUserAndCreated()
        {
            // ARRANGE
            List<UserPublic> users = new List<UserPublic>
            {
                new UserPublic() { ID = 1, FirstName = "Adam", LastName = "A", OAuthID = "12345" },
                new UserPublic() { ID = 2, FirstName = "Ewa", LastName = "E", OAuthID = "23456" }
            };

            IUserRepository userRepository = new MemoryUserRepository(users);
            UserController controller = new UserController(userRepository);

            // ACT
            ObjectResult result = controller.CreateUser(new UserPublic() { ID = 3, FirstName = "Daniel", LastName = "D", OAuthID = "34567" }) as ObjectResult;

            // ASSERT
            result.Value.Should().BeEquivalentTo(new UserPublic() { ID = 3, FirstName = "Daniel", LastName = "D", OAuthID = "34567" });
            result.StatusCode.Should().Be(201);
        }

        [TestMethod]
        public void GetUser_GetsUser_ReturnsUserAndOK()
        {
            // ARRANGE
            List<UserPublic> users = new List<UserPublic>
            {
                new UserPublic() { ID = 1, FirstName = "Adam", LastName = "A", OAuthID = "12345" },
                new UserPublic() { ID = 2, FirstName = "Ewa", LastName = "E", OAuthID = "23456" },
                new UserPublic() { ID = 3, FirstName = "Daniel", LastName = "D", OAuthID = "34567"  }
            };
            IUserRepository userRepository = new MemoryUserRepository(users);
            UserController controller = new UserController(userRepository);

            // ACT
            ObjectResult result = (controller.GetUser(2) as ObjectResult);

            // ASSERT
            result.Value.Should().BeEquivalentTo(new UserPublic() { ID = 2, FirstName = "Ewa", LastName = "E", OAuthID = "23456" });
            result.StatusCode.Should().Be(200);
        }

        [TestMethod]
        public void GetUser_IDNotExist_Returns404()
        {
            // ARRANGE
            List<UserPublic> users = new List<UserPublic>
            {
                new UserPublic() { ID = 1, FirstName = "Adam", LastName = "A", OAuthID = "12345" },
                new UserPublic() { ID = 2, FirstName = "Ewa", LastName = "E", OAuthID = "23456" },
                new UserPublic() { ID = 3, FirstName = "Daniel", LastName = "D", OAuthID = "34567"  }
            };
            IUserRepository userRepository = new MemoryUserRepository(users);
            UserController controller = new UserController(userRepository);

            // ACT, Assert
            (controller.GetUser(4) as StatusCodeResult).StatusCode.Should().Be(404);
        }

        [TestMethod]
        public void GetUserOAuth_GetsUser_ReturnsUserAndOk()
        {
            // ARRANGE
            List<UserPublic> users = new List<UserPublic>
            {
                new UserPublic() { ID = 1, FirstName = "Adam", LastName = "A", OAuthID = "12345" },
                new UserPublic() { ID = 2, FirstName = "Ewa", LastName = "E", OAuthID = "23456" },
                new UserPublic() { ID = 3, FirstName = "Daniel", LastName = "D", OAuthID = "34567"  }
            };
            IUserRepository userRepository = new MemoryUserRepository(users);
            UserController controller = new UserController(userRepository);

            // ACT
            ObjectResult result = (controller.GetUserOAuth("12345") as ObjectResult);

            // ASSERT
            result.Value.Should().BeEquivalentTo(new UserPublic() { ID = 1, FirstName = "Adam", LastName = "A", OAuthID = "12345" });
            result.StatusCode.Should().Be(200);
        }

        [TestMethod]
        public void UpdateUser_UpdatesUser_ReturnsUserAndOk()
        {
            // ARRANGE
            List<UserPublic> users = new List<UserPublic>
            {
                new UserPublic() { ID = 1, FirstName = "Adam", LastName = "A", OAuthID = "12345" },
                new UserPublic() { ID = 2, FirstName = "Ewa", LastName = "E", OAuthID = "23456" },
                new UserPublic() { ID = 3, FirstName = "Daniel", LastName = "D", OAuthID = "34567"  }
            };
            IUserRepository userRepository = new MemoryUserRepository(users);
            UserController controller = new UserController(userRepository);

            // ACT
            UserPublic user = (controller.GetUser(1) as ObjectResult).Value as UserPublic;
            user.FirstName = "Karol";
            controller.UpdateUser(user);
            ObjectResult result = controller.GetUser(1) as ObjectResult;

            // ASSERT
            result.Value.Should().BeEquivalentTo(new UserPublic() { ID = 1, FirstName = "Karol", LastName = "A", OAuthID = "12345" });
            result.StatusCode.Should().Be(200);
        }

        [TestMethod]
        public void UpdateUser_IDNotExist_Returns404()
        {
            // ARRANGE
            List<UserPublic> users = new List<UserPublic>
            {
                new UserPublic() { ID = 1, FirstName = "Adam", LastName = "A", OAuthID = "12345" },
                new UserPublic() { ID = 2, FirstName = "Ewa", LastName = "E", OAuthID = "23456" },
                new UserPublic() { ID = 3, FirstName = "Daniel", LastName = "D", OAuthID = "34567"  }
            };
            IUserRepository userRepository = new MemoryUserRepository(users);
            UserController controller = new UserController(userRepository);

            // ACT
            UserPublic user = new UserPublic() { ID = 4, FirstName = "Janusz", LastName = "J", OAuthID = "45678" };
            user.FirstName = "Karol";
            StatusCodeResult result = controller.UpdateUser(user) as StatusCodeResult;

            // ASSERT
            result.StatusCode.Should().Be(404);
        }

        [TestMethod]
        public void DeleteUser_DeletesUser_CountIsLower()
        {
            // ARRANGE
            List<UserPublic> users = new List<UserPublic>
            {
                new UserPublic() { ID = 1, FirstName = "Adam", LastName = "A", OAuthID = "12345" },
                new UserPublic() { ID = 2, FirstName = "Ewa", LastName = "E", OAuthID = "23456" },
                new UserPublic() { ID = 3, FirstName = "Daniel", LastName = "D", OAuthID = "34567"  }
            };
            IUserRepository userRepository = new MemoryUserRepository(users);
            UserController controller = new UserController(userRepository);

            // ACT
            controller.DeleteUser(2);

            // ASSERT
            users.Count.Should().Be(2);
        }
    }
}
