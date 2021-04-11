using Microsoft.EntityFrameworkCore;
using PTM.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tesseract.Common;

namespace PTM.Logic
{
    /// <summary>
    /// Logika dla TaskBoardów
    /// </summary>
    public class TaskBoardLogic : BaseLogic
    {
        private IDatabaseContext mDBContext;

        /// <summary>
        /// Domyślny ctor
        /// </summary>
        /// <param name="dbContext">Konteks db</param>
        public TaskBoardLogic(IDatabaseContext dbContext)
        {
            Ensure.ParamNotNull(dbContext, nameof(dbContext));

            mDBContext = dbContext;
        }

        /// <summary>
        /// Tworzy taskboard w bazie danych
        /// </summary>
        /// <param name="taskBoard">Taskboard</param>
        /// <returns></returns>
        public TaskBoard CreateTaskBoard(TaskBoard taskBoard)
        {
            Ensure.ParamNotNull(taskBoard, nameof(taskBoard));

            return mDBContext.TaskBoards.Add(taskBoard).Entity;
        }

        /// <summary>
        /// Aktualizuje taskboard w bazie danych
        /// </summary>
        /// <param name="taskBoard">Taskboard</param>
        /// <returns></returns>
        public TaskBoard UpdateTaskBoard(TaskBoard taskBoard)
        {
            Ensure.ParamNotNull(taskBoard, nameof(taskBoard));

            if (taskBoard.ID <= 0)
            {
                throw new ArgumentException($"Parameter {nameof(taskBoard)} ID's has to be grater than 0. The provided value is {taskBoard.ID}.");
            }

            TaskBoard result = mDBContext.TaskBoards.FirstOrDefault(wi => wi.ID == taskBoard.ID);

            if (result == null)
            {
                throw new ArgumentException($"TaskBoard with ID {taskBoard.ID} does not exist.");
            }

            result.Name = taskBoard.Name;

            return result;
        }

        /// <summary>
        /// Usuwa taskboard z bazy danych
        /// </summary>
        /// <param name="ID">ID taskboardu</param>
        /// <returns></returns>
        public async Task DeleteTaskBoard(int ID)
        {
            TaskBoard taskBoard = mDBContext.TaskBoards.Find(ID);

            if (taskBoard == null)
            {
                return;
            }

            List<WorkItem> workItems = await mDBContext.WorkItems.Where(wi => wi.WorkItemCollection.TaskBoardId == ID).ToListAsync().ConfigureAwait(false);

            mDBContext.WorkItems.RemoveRange(workItems);
            mDBContext.WorkItemCollections.RemoveRange(taskBoard.WorkItemCollections);
            mDBContext.TaskBoards.Remove(taskBoard);
        }
    }
}
