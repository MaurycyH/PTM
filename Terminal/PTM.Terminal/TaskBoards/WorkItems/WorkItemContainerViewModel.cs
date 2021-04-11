using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using PTM.PublicDataModel;
using PTM.Services.Client.WorkItemClient;
using PTM.Services.Client.WorkItemCollectionClient;
using PTM.Terminal.Mediator;
using PTM.Terminal.WorkItemDetailWindow;
using Tesseract.Common;
using Tesseract.Common.MVVM;

namespace PTM.Terminal.TaskBoards.WorkItems
{
    public class WorkItemContainerViewModel : BindableBase
    {
        private ITerminalContext mContext;
        private string mName;

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

                if(value != WorkItemCollection.Name)
                {
                    if (string.IsNullOrEmpty(value))
                    {
                        switch(mContext.DialogBuilder.ChoiceDialog("Do You want to delete this collection and all its items?"))
                        {
                            case MessageBoxResult.Yes: Task.Run(() => DeleteWorkItemCollection()); break;
                            default: Name = "Unnamed Collection"; break;
                        }
                    }
                    else
                    {
                        WorkItemCollection.Name = value;
                        Task.Run(() => UpdateWorkItemCollection());
                    }
                }
            }
        }

        public WorkItemCollectionPublic WorkItemCollection { get; set; }

        /// <summary>
        /// Tworzy obserwowalna kolekcje taskow
        /// </summary>
        public ObservableCollection<WorkItemPublic> WorkItems { get; }

        /// <summary>
        /// ICommand wywolywany podczas klikniecie przycisku "Dodaj zadanie"
        /// </summary>
        public ICommand AddWorkItemCommand { get; }

        /// <summary>
        /// Konstruktor
        /// </summary>
        public WorkItemContainerViewModel(ITerminalContext context, WorkItemCollectionPublic wic)
        {
            Ensure.ParamNotNull(wic, nameof(wic));

            mContext = context;

            WorkItemCollection = wic;
            Name = wic.Name;

            AddWorkItemCommand = new AsyncCommand(AddWorkItem);
            ShowEditDialogCommand = new BasicCommand<WorkItemPublic>(ShowEditDialog);
            WorkItems = new ObservableCollection<WorkItemPublic>(wic.WorkItems);
        }
        

        /// <summary>
        /// Odpala komendę wyświetlenia okienka edycji workItema
        /// </summary>
        public ICommand ShowEditDialogCommand { get; }

        /// <summary>
        /// Pokazuje okno edycji Work Itema z odniesieniem do niego
        /// </summary>
        private void ShowEditDialog(WorkItemPublic workItem)
        {
            mContext.WindowManager.OpenWindow(new DlgWorkItemDetails(workItem, mContext));
        }

        /// <summary>
        /// Metoda dodajaca nowe zadanie do obserswowalnej kolekcji zadan
        /// </summary>
        private async Task AddWorkItem()
        {
            try
            {
                HttpWorkItemClient client = new HttpWorkItemClient();

                WorkItemPublic workItem = new WorkItemPublic()
                {
                    Name = "WorkItem",
                    WorkItemCollectionID = WorkItemCollection.ID,
                    WorkItemStart = DateTime.Now.Date.AddHours(12),
                    WorkItemEnd = DateTime.Now.Date.AddHours(13)
                };

                await Application.Current.Dispatcher.BeginInvoke(() =>
                {
                    mContext.WindowManager.OpenWindow(new DlgWorkItemDetails(workItem, mContext));
                });
            }
            catch (Exception)
            {
                mContext.DialogBuilder.ErrorDialog("Could not create the Work Item, due to server error.");
            }
        }

        private async Task UpdateWorkItemCollection()
        {
            try
            {
                HttpWorkItemCollectionClient client = new HttpWorkItemCollectionClient();

                await client.UpdateWorkItemCollection(WorkItemCollection).ConfigureAwait(false);
            }
            catch (Exception)
            {
                mContext.DialogBuilder.ErrorDialog("Could not update collection, due to server error.");
            }
        }

        private async Task DeleteWorkItemCollection()
        {
            try
            {
                HttpWorkItemCollectionClient client = new HttpWorkItemCollectionClient();

                await client.DeleteWorkItemCollection(WorkItemCollection.ID).ConfigureAwait(false);

                await Application.Current.Dispatcher.BeginInvoke(() =>
                {
                    mContext.WorkItemMediator.UpdateCollection(WorkItemCollection);
                    mContext.WorkItemMediator.UpdateWorkItems();
                    mContext.DialogBuilder.SuccessDialog("Collection was successfully deleted");
                });
            }
            catch (Exception)
            {
                mContext.DialogBuilder.ErrorDialog("Could not delete collection, due to server error.");
            }
        }
    }
}
