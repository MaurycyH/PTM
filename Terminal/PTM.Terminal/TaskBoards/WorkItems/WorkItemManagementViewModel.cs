using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using PTM.PublicDataModel;
using PTM.Services.Client.WorkItemClient;
using PTM.Services.Client.WorkItemCollectionClient;
using PTM.Terminal.Mediator;
using Tesseract.Common;
using Tesseract.Common.MVVM;

namespace PTM.Terminal.TaskBoards.WorkItems
{
    public class WorkItemManagementViewModel : BindableBase
    {
        private ITerminalContext mContext;
        private TaskBoardPublic mTaskBoard;
        /// <summary>
        /// Tworzy obserwowalna kolekcje kontenerow
        /// </summary>
        public ObservableCollection<CtrlWorkItemContainer> ContainerCollection { get; }

        /// <summary>
        /// ICommand wywolywany podczas tworzenia klikniecia przycisku "Dodaj kontener".
        /// </summary>
        public ICommand AddContainerCommand { get; }

        /// <summary>
        /// Konstruktor 
        /// </summary>
        public WorkItemManagementViewModel(ITerminalContext context, TaskBoardPublic taskBoard)
        {
            Ensure.ParamNotNull(context, nameof(context));

            mContext = context;
            mContext.WorkItemMediator.CollectionUpdatedEvent += new WorkItemMediator.OnCollectionUpdated(DeleteContainer);
            mContext.WorkItemMediator.CollectionAddItemEvent += new WorkItemMediator.OnCollectionAddItem(AddWorkItemToContainer);

            mTaskBoard = taskBoard;
            AddContainerCommand = new AsyncCommand(this.AddContainer);
            ContainerCollection = new ObservableCollection<CtrlWorkItemContainer>();

            Task.Run(() => GetContainers());
        }

        /// <summary>
        /// Metoda dodajaca nowy kontener do obserwowalnej kolekcji kontenerow
        /// </summary>
        private async Task AddContainer()
        {
            try
            {
                HttpWorkItemCollectionClient client = new HttpWorkItemCollectionClient();

                WorkItemCollectionPublic workItemCollection = new WorkItemCollectionPublic()
                {
                    Name = "Unnamed Collection",
                    TaskBoardID = mTaskBoard.ID
                };

                WorkItemCollectionPublic response = await client.CreateWorkItemCollection(workItemCollection).ConfigureAwait(false);
                response.WorkItems = new List<WorkItemPublic>();

                await Application.Current.Dispatcher.BeginInvoke(() =>
                {
                    ContainerCollection.Add(new CtrlWorkItemContainer(mContext, response));
                });
            }
            catch (Exception ex)
            {
                mContext.DialogBuilder.ErrorDialog("Could not create a collection, due to server error.", ex);
            }
        }

        /// <summary>
        /// Pobiera kolekcje z bazy danych i pakuje je w kontenery
        /// </summary>
        private async Task GetContainers()
        {
            try
            {
                HttpWorkItemCollectionClient client = new HttpWorkItemCollectionClient();

                IEnumerable<WorkItemCollectionPublic> response = await client.GetAllWorkItemCollections(mTaskBoard.ID).ConfigureAwait(false);

                foreach (WorkItemCollectionPublic wic in response)
                {
                    wic.WorkItems = new List<WorkItemPublic>();
                    await GetWorkItems(wic).ConfigureAwait(false);
                }

                await Application.Current.Dispatcher.BeginInvoke(() =>
                {
                    foreach (WorkItemCollectionPublic wic in response)
                    {
                        ContainerCollection.Add(new CtrlWorkItemContainer(mContext, wic));
                    }
                });
            }
            catch (Exception ex)
            {
                mContext.DialogBuilder.ErrorDialog("Could not retrieve collections, due to server error.", ex);
            }
        }

        /// <summary>
        /// pobiera workItemy dla kolekcji
        /// </summary>
        private async Task GetWorkItems(WorkItemCollectionPublic collection)
        {
            try
            {
                HttpWorkItemClient client = new HttpWorkItemClient();

                IEnumerable<WorkItemPublic> response = await client.GetAllWorkItems(collection.ID).ConfigureAwait(false);

                await Application.Current.Dispatcher.BeginInvoke(() =>
                {
                    foreach(WorkItemPublic workItem in response)
                    {
                        collection.WorkItems.Add(workItem);
                    }
                });
            }
            catch (Exception ex)
            {
                mContext.DialogBuilder.ErrorDialog("Could not retrieve work items, due to server error.", ex);
            }
        }

        /// <summary>
        /// Usuwa kontener
        /// </summary>
        public void DeleteContainer(WorkItemMediator m, WorkItemCollectionEventArgs e)
        {
            ContainerCollection.Remove(ContainerCollection.FirstOrDefault(cc => ((WorkItemContainerViewModel)cc.DataContext).WorkItemCollection == e.Collection));
        }

        /// <summary>
        /// Dodaje workItem do kontenera
        /// </summary>
        public void AddWorkItemToContainer(WorkItemMediator m, WorkItemEventArgs e)
        {
            Ensure.ParamNotNull(e, nameof(e));

            WorkItemPublic workItem = e.WorkItem;

            CtrlWorkItemContainer container = ContainerCollection.FirstOrDefault(cc => ((WorkItemContainerViewModel)cc.DataContext).WorkItemCollection.ID == workItem.WorkItemCollectionID);
            WorkItemContainerViewModel wic = (WorkItemContainerViewModel)container.DataContext;
            WorkItemPublic wi = wic.WorkItems.FirstOrDefault(wi => wi.ID == workItem.ID);

            if (wi==null)
            {
                wic.WorkItems.Add(workItem);
            }
            else
            {
                int index = wic.WorkItems.IndexOf(wi);
                wic.WorkItems.Remove(wi);
                wic.WorkItems.Insert(index, workItem);
            }
        }
    }
}
