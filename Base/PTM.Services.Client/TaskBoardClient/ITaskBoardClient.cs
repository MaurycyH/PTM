using PTM.Entities;
using PTM.PublicDataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PTM.Services.Client.TaskBoardClient
{
    /// <summary>
    /// Interfejs klienta taskboardu
    /// </summary>
    public interface ITaskBoardClient
    {
        /// <summary>
        /// Pobiera Taskboard o wskazanym ID
        /// </summary>
        /// <param name="ID">ID taskboardu</param>
        /// <returns>Taskboard</returns>
        Task<TaskBoardPublic> GetTaskboard(int ID);

        /// <summary>
        /// Pobiera wszystkie taskboardy użytkownika
        /// </summary>
        /// <param name="userID">ID użytkownika</param>
        /// <returns>Taskboard</returns>
        Task<ICollection<TaskBoardPublic>> GetAllTaskboards(int userID);

        /// <summary>
        /// Tworzy taskboard
        /// </summary>
        /// <param name="taskBoard">Dane taskboardu do stworzenia</param>
        /// <returns>Stworzony obiekt taskboardu</returns>
        Task<TaskBoardPublic> CreateTaskBoard(TaskBoardPublic taskBoard);

        /// <summary>
        /// Aktualizuje taskboard
        /// </summary>
        /// <param name="taskBoard">Dane taskboardu do zaktualizowania</param>
        /// <returns>Stworzony obiekt taskboardu</returns>
        Task<TaskBoardPublic> UpdateTaskBoard(TaskBoardPublic taskBoard);

        /// <summary>
        /// Kasuje taskboard
        /// </summary>
        /// <param name="ID">ID taskboardu do skasowania</param>
        /// <returns>Stworzony obiekt taskboardu</returns>
        Task DeleteTaskBoard(int ID);
    }
}
