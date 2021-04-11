using Microsoft.AspNetCore.Mvc;
using PTM.PublicDataModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tesseract.Common;

namespace PTM.Services.TaskBoards
{
    [ApiController]
    [Route("taskboards")]
    public class TaskBoardController : ControllerBase
    {
        ITaskBoardRepository mRepository;

        /// <summary>
        /// Domyślny ctor.
        /// </summary>
        /// <param name="repository">Repozytorium</param>
        public TaskBoardController(ITaskBoardRepository repository)
        {
            Ensure.ParamNotNull(repository, nameof(repository));

            mRepository = repository;
        }

        /// <summary>
        /// Pobiera wszystkie taskboardy użytkownika
        /// </summary>
        /// <param name="ID">ID użytkownika</param>
        /// <returns>Lista taskboardów usera</returns>
        [HttpGet("GetAll/{ID}")]
        public IActionResult GetUserTaskBoards(int ID)
        {
            // TODO sprawdzenie, czy użytkownik istnieje - wymagany serwis userów
            try
            {
                if (ID <= 0)
                {
                    return base.Problem("Parameter has to be grater than 0");
                }

                IEnumerable<TaskBoardPublic> taskBoard = mRepository.GetUserTaskBoards(ID);

                if (taskBoard == null)
                {
                    return base.NotFound(taskBoard);
                }

                return base.Ok(taskBoard);
            }
            catch (Exception ex)
            {
                return base.Problem(ex.Message);
            }
        }

        /// <summary>
        /// Pobiera taskboard o podanym ID
        /// </summary>
        /// <param name="ID">ID taskboardu</param>
        /// <returns>Taskboard</returns>
        [HttpGet("{ID}")]
        public IActionResult GetTaskBoard(int ID)
        {
            try
            {
                if (ID <= 0)
                {
                    return base.Problem("Parameter has to be grater than 0");
                }

                TaskBoardPublic taskBoard = mRepository.GetTaskBoard(ID);

                if (taskBoard == null)
                {
                    return base.NotFound(taskBoard);
                }

                return base.Ok(taskBoard);
            }
            catch (Exception ex)
            {
                return base.Problem(ex.Message);
            }
        }

        /// <summary>
        /// Tworzy taskboard
        /// </summary>
        /// <param name="taskBoard">Obiekt taskboardu do stworzenia</param>
        [HttpPost]
        public virtual IActionResult CreateTaskBoard([FromBody]TaskBoardPublic taskBoard)
        {
            Ensure.ParamNotNull(taskBoard, nameof(taskBoard));

            TaskBoardPublic createdTaskBoard = mRepository.CreateTaskBoard(taskBoard);

            return base.Created($"/taskboards/{createdTaskBoard.ID}", createdTaskBoard);
        }

        /// <summary>
        /// Aktualizuje Taskboard
        /// </summary>
        /// <param name="taskBoard">Obiekt Kolekcji do zaktualizowania</param>
        [HttpPut]
        public virtual IActionResult UpdateTaskBoard([FromBody] TaskBoardPublic taskBoard)
        {
            Ensure.ParamNotNull(taskBoard, nameof(taskBoard));

            TaskBoardPublic updatedTaskBoard = mRepository.UpdateTaskBoard(taskBoard);

            if (updatedTaskBoard == null)
            {
                return base.NotFound();
            }

            return base.Created($"/taskboards/{updatedTaskBoard.ID}", updatedTaskBoard);
        }

        /// <summary>
        /// Kasuje TaskBoard
        /// </summary>
        /// <param name="ID">ID TaskBoardu do usunięcia</param>
        [HttpDelete("{ID}")]
        public async virtual Task<IActionResult> DeleteTaskBoard(int ID)
        {
            if (ID <= 0)
            {
                return base.Problem("Parameter has to be grater than 0");
            }

            await mRepository.DeleteTaskBoard(ID);

            return base.Ok($"/taskboards/{ID}");
        }
    }
}
