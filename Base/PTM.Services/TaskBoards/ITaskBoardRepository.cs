using PTM.PublicDataModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PTM.Services.TaskBoards
{
    /// <summary>
    /// Repozytorium Taskboardów
    /// </summary>
    public interface ITaskBoardRepository
    {
        /// <summary>
        /// Pobiera wszystkie taskboardy usera
        /// </summary>
        /// <returns>Zwraca kolekcje taskboardów usera</returns>
        IEnumerable<TaskBoardPublic> GetUserTaskBoards(int ID);

        /// <summary>
        /// Pobiera taskboard o wskazanym ID
        /// </summary>
        /// <param name="ID">ID taskboardu do pobrania</param>
        /// <returns>Taskboard</returns>
        TaskBoardPublic GetTaskBoard(int ID);

        /// <summary>
        /// Tworzy taskboard
        /// </summary>
        /// <param name="taskBoard">Dane taskboardu do utworzenia</param>
        /// <returns>Taskboard</returns>
        TaskBoardPublic CreateTaskBoard(TaskBoardPublic taskBoard);

        /// <summary>
        /// Aktualizuje taskBoard
        /// </summary>
        /// <param name="taskBoard">Dane taskboardu do utworzenia</param>
        /// <returns>Taskboard</returns>
        TaskBoardPublic UpdateTaskBoard(TaskBoardPublic taskBoard);

        /// <summary>
        /// Kasuje taskBoard
        /// </summary>
        /// <param name="ID">ID taskboardu do skasowania</param>
        Task DeleteTaskBoard(int ID);
    }
}
