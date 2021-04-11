using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PTM.PublicDataModel;
using PTM.Services.TaskBoards;
using System;
using System.Collections.Generic;

namespace Test.PTM.Services
{
    [TestClass]
    public class TestTaskBoardController
    {
        /// <summary>
        /// Sprawdza, czy serwis taskboardów zwróci kolekcję dla usera
        /// </summary>
        [TestMethod]
        public void GetUserTaskBoards_RequestingTaskBoardsForExistingUser_ReturnsTaskboards()
        {
            // ARRANGE
            List<TaskBoardPublic> taskBoards = new List<TaskBoardPublic>
            {
                new TaskBoardPublic() { ID = 1, Name = Guid.NewGuid().ToString(), UserID = 1 },
                new TaskBoardPublic() { ID = 2, Name = Guid.NewGuid().ToString(), UserID = 1 },
                new TaskBoardPublic() { ID = 3, Name = Guid.NewGuid().ToString(), UserID = 2 }
            };

            ITaskBoardRepository taskBoardRepository = new MemoryTaskBoardRepository(taskBoards);
            TaskBoardController controller = new TaskBoardController(taskBoardRepository);

            // ACT
            IEnumerable<TaskBoardPublic> result = (controller.GetUserTaskBoards(1) as ObjectResult).Value as IEnumerable<TaskBoardPublic>;

            // ASSERT
            result.Should().HaveCount(2);
        }

        /// <summary>
        /// Sprawdza, czy serwis taskboardów zwraca HTTP 200 dla istniejącego usera
        /// </summary>
        [TestMethod]
        public void GetUserTaskBoards_RequestingTaskBoardsForExistingUser_ReturnsOK()
        {
            // ARRANGE
            List<TaskBoardPublic> taskBoards = new List<TaskBoardPublic>
            {
                new TaskBoardPublic() { ID = 1, Name = Guid.NewGuid().ToString(), UserID = 1 },
                new TaskBoardPublic() { ID = 2, Name = Guid.NewGuid().ToString(), UserID = 1 },
                new TaskBoardPublic() { ID = 3, Name = Guid.NewGuid().ToString(), UserID = 2 }
            };

            ITaskBoardRepository taskBoardRepository = new MemoryTaskBoardRepository(taskBoards);
            TaskBoardController controller = new TaskBoardController(taskBoardRepository);

            // ACT
            OkObjectResult result = controller.GetUserTaskBoards(1) as OkObjectResult;

            // ASSERT
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(200);
        }

        /// <summary>
        /// Dla nowego usera lista taskboardów powinna być pusta, a w rezultacie powinno przyjść HTTP 200
        /// </summary>
        [TestMethod]
        public void GetUserTaskBoards_ForNewUser_ReturnsOK()
        {
            // ARRANGE
            List<TaskBoardPublic> taskBoards = new List<TaskBoardPublic>();

            taskBoards.Add(new TaskBoardPublic() { ID = 1, Name = Guid.NewGuid().ToString(), UserID = 1 });
            taskBoards.Add(new TaskBoardPublic() { ID = 2, Name = Guid.NewGuid().ToString(), UserID = 1 });

            ITaskBoardRepository taskBoardRepository = new MemoryTaskBoardRepository(taskBoards);
            TaskBoardController controller = new TaskBoardController(taskBoardRepository);

            // ACT
            OkObjectResult result = controller.GetUserTaskBoards(2) as OkObjectResult;

            // ASSERT
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(200);
        }

        /// <summary>
        /// W przypadku nieistniejącego taskboardu spodziewamy się HTTP 404
        /// </summary>
        [TestMethod]
        public void GetTaskBoard_RequestingNonExistingTaskboard_ReturnsNotFound()
        {
            // ARRANGE
            ITaskBoardRepository taskBoardRepository = new MemoryTaskBoardRepository();
            TaskBoardController controller = new TaskBoardController(taskBoardRepository);

            // ACT
            NotFoundObjectResult result = (NotFoundObjectResult)controller.GetTaskBoard(1);

            // ASSERT
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(404);
        }

        /// <summary>
        /// Dla requesta ID mniejszego niż 1 spodziewamy się błędu HTTP 500
        /// </summary>
        [TestMethod]
        public void GetTaskBoard_OnInvalidID_ReportsProblem()
        {
            // ARRANGE
            ITaskBoardRepository taskBoardRepository = new MemoryTaskBoardRepository();
            TaskBoardController controller = new TaskBoardController(taskBoardRepository);
            controller.ProblemDetailsFactory = new MockProblemDetailsFactory();

            // ACT
            ObjectResult result = controller.GetTaskBoard(0) as ObjectResult;

            // ASSERT
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(500);
        }

        /// <summary>
        /// Dla poprawnego requesta spodziewamy się HTTP 200
        /// </summary>
        [TestMethod]
        public void GetTaskBoard_OnValidRequest_ReturnOK()
        {
            // ARRANGE
            ITaskBoardRepository taskBoardRepository = new MemoryTaskBoardRepository();
            taskBoardRepository.CreateTaskBoard(new TaskBoardPublic() { ID = 1 });
            TaskBoardController controller = new TaskBoardController(taskBoardRepository);
            controller.ProblemDetailsFactory = new MockProblemDetailsFactory();

            // ACT
            OkObjectResult result = controller.GetTaskBoard(1) as OkObjectResult;

            // ASSERT
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(200);
        }

        /// <summary>
        /// Dla poprawnego requesta spodziewamy się znalezionego taskboardu
        /// </summary>
        [TestMethod]
        public void GetTaskBoard_OnValidRequest_ReturnTaskBoardPublic()
        {
            // ARRANGE
            ITaskBoardRepository taskBoardRepository = new MemoryTaskBoardRepository();
            taskBoardRepository.CreateTaskBoard(new TaskBoardPublic() { ID = 1, Name = "Test", UserID = 1 });
            TaskBoardController controller = new TaskBoardController(taskBoardRepository);
            controller.ProblemDetailsFactory = new MockProblemDetailsFactory();

            // ACT
            TaskBoardPublic result = (controller.GetTaskBoard(1) as OkObjectResult).Value as TaskBoardPublic;

            // ASSERT
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo<TaskBoardPublic>(new TaskBoardPublic() { ID = 1, Name = "Test", UserID = 1 });
        }

        /// <summary>
        /// Dla poprawnego requesta spodziewamy się HTTP 201
        /// </summary>
        [TestMethod]
        public void CreateTaskBoard_OnProperBody_ReturnsCreated()
        {
            // ARRANGE
            ITaskBoardRepository taskBoardRepository = new MemoryTaskBoardRepository();
            TaskBoardController controller = new TaskBoardController(taskBoardRepository);

            // ACT
            CreatedResult result = controller.CreateTaskBoard(new TaskBoardPublic() { ID = 1, Name = Guid.NewGuid().ToString(), UserID = 1}) as CreatedResult;

            // ASSERT
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(201);
        }

        /// <summary>
        /// Dla poprawnego requesta spodziewamy się HTTP 201
        /// </summary>
        [TestMethod]
        public void CreateTaskBoard_OnProperBody_ReturnsCreatedTaskboard()
        {
            // ARRANGE
            ITaskBoardRepository taskBoardRepository = new MemoryTaskBoardRepository();
            TaskBoardController controller = new TaskBoardController(taskBoardRepository);
            TaskBoardPublic taskBoard = new TaskBoardPublic() { ID = 1, Name = Guid.NewGuid().ToString(), UserID = 1 };

            // ACT
            TaskBoardPublic result = (controller.CreateTaskBoard(taskBoard) as CreatedResult).Value as TaskBoardPublic;

            // ASSERT
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo<TaskBoardPublic>(taskBoard);
        }
    }
}
