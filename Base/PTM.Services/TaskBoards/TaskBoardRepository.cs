using PTM.Entities;
using PTM.Logic;
using PTM.Logic.ModelConverters;
using PTM.PublicDataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Tesseract.Common;

namespace PTM.Services.TaskBoards
{
    public class TaskBoardRepository : ITaskBoardRepository
    {
        private IDatabaseContext mDBContext;

        /// <summary>
        /// Domyślny ctor.
        /// </summary>
        /// <param name="databaseContext">Kontekst bazy danych z którego ma korzystać serwis.</param>
        public TaskBoardRepository(IDatabaseContext databaseContext)
        {
            Ensure.ParamNotNull(databaseContext, nameof(databaseContext));

            mDBContext = databaseContext;
        }

        /// <inheritdoc/>
        public TaskBoardPublic CreateTaskBoard(TaskBoardPublic taskBoard)
        {
            Ensure.ParamNotNull(taskBoard, nameof(taskBoard));

            TaskBoardConverter converter = new TaskBoardConverter(mDBContext);
            TaskBoardLogic logic = new TaskBoardLogic(mDBContext);

            TaskBoard createdTaskBoard = logic.CreateTaskBoard(converter.Convert(taskBoard));
            mDBContext.SaveChanges();

            return converter.Convert(createdTaskBoard);
        }

        /// <inheritdoc/>
        public async Task DeleteTaskBoard(int ID)
        {
            TaskBoardLogic logic = new TaskBoardLogic(mDBContext);

            await logic.DeleteTaskBoard(ID);

            mDBContext.SaveChanges();
        }

        /// <inheritdoc/>
        public TaskBoardPublic GetTaskBoard(int ID)
        {
            TaskBoard taskBoard = mDBContext.TaskBoards.FirstOrDefault(tb => tb.ID == ID);

            // Taskboard o takim ID nie został znaleziony. 
            if (taskBoard == null)
            {
                return null;
            }

            TaskBoardConverter converter = new TaskBoardConverter(mDBContext);

            return converter.Convert(taskBoard);
        }

        /// <inheritdoc/>
        public IEnumerable<TaskBoardPublic> GetUserTaskBoards(int ID)
        {
            List<TaskBoard> taskBoards = mDBContext.TaskBoards.Where(tb => tb.User.ID == ID).ToList();
            TaskBoardConverter converter = new TaskBoardConverter(mDBContext);

            foreach (TaskBoard taskBoard in taskBoards)
            {
                yield return converter.Convert(taskBoard);
            }
        }

        /// <inheritdoc/>
        public TaskBoardPublic UpdateTaskBoard(TaskBoardPublic taskBoard)
        {
            Ensure.ParamNotNull(taskBoard, nameof(taskBoard));

            TaskBoardConverter converter = new TaskBoardConverter(mDBContext);
            TaskBoardLogic logic = new TaskBoardLogic(mDBContext);

            TaskBoard createdTaskBoard = logic.UpdateTaskBoard(converter.Convert(taskBoard));
            mDBContext.SaveChanges();

            return converter.Convert(createdTaskBoard);
        }
    }
}
