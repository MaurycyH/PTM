using PTM.PublicDataModel;
using PTM.Services.Client.TaskBoardClient;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Common;
using Tesseract.Common.MVVM;

namespace PTM.Terminal.TaskBoards
{
    public class TaskBoardContainer : BindableBase
    {
        private string mName;
        private ITerminalContext mContext;
        public TaskBoardPublic TaskBoard { get; set; }

        /// <summary>
        /// Prop obsługujący wyświetlanie i aktualizowanie nazwy taskboardu.
        /// </summary>
        public string Name
        {
            get
            {
                return mName;
            }
            set
            {
                mName = value;
                OnPropertyChanged(nameof(Name));

                if (value != TaskBoard.Name)
                {
                    if (string.IsNullOrEmpty(value))
                    {
                        Name = "Unnamed TaskBoard";
                    }
                    else
                    {
                        TaskBoard.Name = value;
                        Task.Run(() => UpdateTaskBoard());
                    }
                }
            }
        }

        public TaskBoardContainer(TaskBoardPublic taskBoard, ITerminalContext context)
        {
            Ensure.ParamNotNull(taskBoard, nameof(taskBoard));
            Ensure.ParamNotNull(context, nameof(context));
            this.TaskBoard = taskBoard;
            this.Name = taskBoard.Name;
            this.mContext = context;
        }

        private async Task UpdateTaskBoard()
        {
            try
            {
                HttpTaskBoardClient client = new HttpTaskBoardClient();

                await client.UpdateTaskBoard(TaskBoard).ConfigureAwait(false);
            }
            catch (Exception)
            {
                mContext.DialogBuilder.ErrorDialog("Could not update collection, due to server error.");
            }
        }
    }
}
