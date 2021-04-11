using PTM.Entities;
using PTM.PublicDataModel;
using Tesseract.Common;

namespace PTM.Logic.ModelConverters
{
    /// <summary>
    /// Konwerter między <see cref="TaskBoard"/> i <see cref="TaskBoardPublic"/>
    /// </summary>
    public class TaskBoardConverter : IModelConverter<TaskBoard, TaskBoardPublic>
    {
        private IDatabaseContext mDBContext;

        public TaskBoardConverter(IDatabaseContext context)
        {
            Ensure.ParamNotNull(context, nameof(context));

            mDBContext = context;
        }

        /// <inheritdoc/>
        public TaskBoard Convert(TaskBoardPublic source)
        {
            Ensure.ParamNotNull(source, nameof(source));

            TaskBoard destination = mDBContext.TaskBoards.Find(source.ID);

            if (destination == null)
            {
                destination = new TaskBoard();
            }

            destination.Name = source.Name;
            destination.User = mDBContext.Users.Find(source.UserID);
            destination.UserID = source.UserID;

            return destination;
        }

        /// <inheritdoc/>
        public TaskBoardPublic Convert(TaskBoard source)
        {
            Ensure.ParamNotNull(source, nameof(source));

            TaskBoardPublic destination = new TaskBoardPublic()
            {
                ID = source.ID,
                Name = source.Name,
                UserID = source.UserID
            };

            return destination;
        }
    }
}
