using PTM.PublicDataModel;
using PTM.Services.Client.TaskBoardClient;
using PTM.Terminal.TaskBoards.WorkItems;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Tesseract.Common;
using Tesseract.Common.MVVM;

namespace PTM.Terminal.TaskBoards
{
    public class TaskBoardMainViewModel : BindableBase
    {
        private TaskBoardView mCurrentView;
        private TaskBoardPublic mSelectedTaskBoard;
        public ITerminalContext mContext { get; }
        public CtrlWorkItemManagement ManagementView { get; set; }

        /// <summary>
        /// Tworzenie nowego TaskBoardu
        /// </summary>
        public ICommand CreateTaskBoardCommand { get; }

        /// <summary>
        /// Przełącza pomiędzy widokiem Listy i TaskBoardu
        /// </summary>
        public ICommand SwitchViewCommand { get; }

        /// <summary>
        /// Wybranie TaskBoardu z poziomu widoku Listy
        /// </summary>
        public ICommand SelectTaskBoardCommand { get; }


        /// <summary>
        /// Komenda usuwania taskboardu
        /// </summary>
        public ICommand DeleteTaskBoardCommand { get; }

        /// <summary>
        /// Lista TaskBoardów użytkownika
        /// </summary>
        public ObservableCollection<TaskBoardContainer> TaskBoards { get; }

        /// <summary>
        /// Enum z możliwymi pod-widokami widoku TaskBoardów
        /// </summary>
        public enum TaskBoardView
        {
            ListView,
            ManagementView
        }

        /// <summary>
        /// Aktualnie przypisany widok
        /// </summary>
        public TaskBoardView CurrentView
        {
            get
            {
                return mCurrentView;
            }
            set
            {
                mCurrentView = value;
                OnPropertyChanged(nameof(CurrentView));
            }
        }

        /// <summary>
        /// TaskBoard wybrany do edycji
        /// </summary>
        public TaskBoardPublic SelectedTaskBoard
        {
            get
            {
                return mSelectedTaskBoard;
            }
            set
            {
                mSelectedTaskBoard = value;
                OnPropertyChanged(nameof(SelectedTaskBoard));
            }
        }

        /// <summary>
        /// Domyślny ctor.
        /// </summary>
        public TaskBoardMainViewModel(ITerminalContext context)
        {
            Ensure.ParamNotNull(context, nameof(context));
            mContext = context;

            CreateTaskBoardCommand = new AsyncCommand(CreateTaskBoard);
            SwitchViewCommand = new BasicCommand(SwapCurrentView);
            SelectTaskBoardCommand = new BasicCommand<TaskBoardPublic>(SelectTaskBoard);
            DeleteTaskBoardCommand = new AsyncCommand<TaskBoardContainer>(DeleteTaskBoard);

            TaskBoards = new ObservableCollection<TaskBoardContainer>();

            Task.Run(() => GetTaskBoards());

            CurrentView = TaskBoardView.ListView;
        }

        /// <summary>
        /// Przełącza pomiędzy widokiem Listy i TaskBoardu
        /// </summary>
        public void SwapCurrentView()
        {
            if (CurrentView == TaskBoardView.ListView)
            {
                ManagementView = new CtrlWorkItemManagement(mContext, SelectedTaskBoard);
                CurrentView = TaskBoardView.ManagementView;
            }
            else
            {
                mContext.WorkItemMediator.ClearCollectionEvents();
                CurrentView = TaskBoardView.ListView;
            }
        }

        /// <summary>
        /// Przekazuje taskboard z widoku do zmiennej
        /// </summary>
        /// <param name="taskBoard">kliknięty taskboard</param>
        public void SelectTaskBoard(TaskBoardPublic taskBoard)
        {
            SelectedTaskBoard = taskBoard;
            SwapCurrentView();
        }

        /// <summary>
        /// Tworzy nowy TaskBoard
        /// </summary>
        private async Task CreateTaskBoard()
        {
            try
            {
                HttpTaskBoardClient client = new HttpTaskBoardClient();

                TaskBoardPublic taskBoard = new TaskBoardPublic()
                {
                    Name = "Unnamed TaskBoard",
                    UserID = mContext.UserAccount.ID
                };

                TaskBoardPublic response = await client.CreateTaskBoard(taskBoard).ConfigureAwait(false);

                await Application.Current.Dispatcher.BeginInvoke(() =>
                {
                    TaskBoards.Add(new TaskBoardContainer(response, mContext));
                });
            }
            catch (Exception)
            {
                mContext.DialogBuilder.ErrorDialog("Taskboard couldn't be created, due to server error.");
            }
        }

        /// <summary>
        /// Pobiera wszystkie taskboardy
        /// </summary>
        private async Task GetTaskBoards()
        {
            try
            {
                HttpTaskBoardClient client = new HttpTaskBoardClient();

                ICollection<TaskBoardPublic> response = await client.GetAllTaskboards(mContext.UserAccount.ID).ConfigureAwait(false);

                await Application.Current.Dispatcher.BeginInvoke(() =>
                {
                    foreach (TaskBoardPublic tb in response)
                    {
                        TaskBoards.Add(new TaskBoardContainer(tb, mContext));
                    }
                });
            }
            catch (Exception)
            {
                mContext.DialogBuilder.ErrorDialog("Taskboards couldn't be retrieved, due to server error.");
            }
        }

        /// <summary>
        /// Kasuje Taskboard z widoku i z bazy danych
        /// </summary>
        private async Task DeleteTaskBoard(TaskBoardContainer taskBoard)
        {
            switch (mContext.DialogBuilder.ChoiceDialog("Do you want to delete this taskboard? All associated work items will be lost."))
            {
                case MessageBoxResult.Yes: break;
                case MessageBoxResult.No: return;
                default: return;
            }

            try
            {
                HttpTaskBoardClient client = new HttpTaskBoardClient();

                await client.DeleteTaskBoard(taskBoard.TaskBoard.ID).ConfigureAwait(false);

                await Application.Current.Dispatcher.BeginInvoke(() =>
                {
                    mContext.WorkItemMediator.UpdateWorkItems();
                    mContext.DialogBuilder.SuccessDialog("Taskboard was successfully deleted");
                    TaskBoards.Remove(taskBoard);
                });
            }
            catch (Exception)
            {
                mContext.DialogBuilder.ErrorDialog("Taskboard couldn't be deleted, due to server error.");
            }
        }
    }
}
