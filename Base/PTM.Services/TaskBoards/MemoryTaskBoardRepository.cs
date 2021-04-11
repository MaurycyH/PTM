using PTM.Entities;
using PTM.PublicDataModel;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tesseract.Common;

namespace PTM.Services.TaskBoards
{
    public class MemoryTaskBoardRepository : ITaskBoardRepository
    {
        protected ICollection<TaskBoardPublic> mTaskBoards;

        /// <summary>
        /// Domysłny ctor. Inicjalizuje kolekcję taskboardów.
        /// </summary>
        public MemoryTaskBoardRepository()
        {
            if (mTaskBoards == null)
            {
                mTaskBoards = new List<TaskBoardPublic>();
            }
        }

        /// <summary>
        /// Ctor pozwalający przekazać konkretną listę obiektów.
        /// </summary>
        /// <param name="taskBoards">Lista obietków</param>
        public MemoryTaskBoardRepository(ICollection<TaskBoardPublic> taskBoards)
        {
            Ensure.ParamNotNull(taskBoards, nameof(taskBoards));

            mTaskBoards = taskBoards;
        }

        /// <inheritdoc/>
        public TaskBoardPublic CreateTaskBoard(TaskBoardPublic taskBoard)
        {
            Ensure.ParamNotNull(taskBoard, nameof(taskBoard));

            mTaskBoards.Add(taskBoard);

            return taskBoard;
        }

        public Task DeleteTaskBoard(int ID)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc/>
        public TaskBoardPublic GetTaskBoard(int ID)
        {
            TaskBoardPublic taskBoard = mTaskBoards.FirstOrDefault(tb => tb.ID == ID);

            return taskBoard;
        }

        /// <inheritdoc/>
        public IEnumerable<TaskBoardPublic> GetUserTaskBoards(int ID)
        {
            return mTaskBoards.Where(tb => tb.UserID == ID);
        }

        /// <inheritdoc/>
        public TaskBoardPublic UpdateTaskBoard(TaskBoardPublic taskBoard)
        {
            throw new System.NotImplementedException();
        }
    }
}
