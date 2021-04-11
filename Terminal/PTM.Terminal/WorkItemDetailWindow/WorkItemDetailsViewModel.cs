using PTM.PublicDataModel;
using PTM.Services.Client.WorkItemClient;
using PTM.Terminal.WindowHelpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Tesseract.Common.MVVM;

namespace PTM.Terminal.WorkItemDetailWindow
{
    public class WorkItemDetailsViewModel : WindowBaseViewModel
    {
        private readonly string mIdentity;
        private WorkItemPublic mWorkItem;
        private ITerminalContext mContext;

        /// <summary>
        /// Identity okna
        /// </summary>
        public override string Identity
        {
            get
            {
                return mIdentity;
            }
        }

        /// <summary>
        /// Przycisk zapisu zmian
        /// </summary>
        public ICommand UpdateCommand { get; }

        /// <summary>
        /// WorkItem przypisany do okna
        /// </summary>
        public WorkItemPublic WorkItem
        {
            get
            {
                return mWorkItem;
            }
            set
            {
                mWorkItem = value;
                OnPropertyChanged(nameof(WorkItem));
            }
        }

        /// <summary>
        /// Tytuł widoczny na górnej krawędzi okna
        /// </summary>
        public string Title
        {
            get
            {
                return "WorkItem #" + WorkItem.ID + " - Details";
            }
        }

        /// <summary>
        /// Konstruktor przyjmujący WorkItemDisplayViewModel 
        /// </summary>
        public WorkItemDetailsViewModel(WorkItemPublic workItem, ITerminalContext context)
        {
            WorkItem = workItem;
            mContext = context;
            mIdentity = nameof(WorkItemDetailsViewModel) + WorkItem.ID;
            UpdateCommand = new AsyncCommand(SaveChanges);
        }

        /// <summary>
        /// Zapisuje zmiany
        /// </summary>
        public async Task SaveChanges()
        {
            if (string.IsNullOrEmpty(WorkItem.Name))
            {
                mContext.DialogBuilder.WarningDialog("Work Item's name is missing or incorrect");
                return;
            }
            if (WorkItem.WorkItemStart >= WorkItem.WorkItemEnd)
            {
                mContext.DialogBuilder.WarningDialog("Selected date and time are incorrect");
                return;
            }
            if (WorkItem.WorkItemEnd.Subtract(WorkItem.WorkItemStart).TotalHours > 24)
            {
                mContext.DialogBuilder.WarningDialog("Work Item cannot be longer than 24 hours");
                return;
            }

            WorkItemPublic updatedWorkItem = null;

            if (WorkItem.ID == 0)
            {
                updatedWorkItem = await CreateWorkItem().ConfigureAwait(false);
                if(updatedWorkItem == null)
                {
                    updatedWorkItem = WorkItem;
                }
            }
            else
            {
                updatedWorkItem = await UpdateWorkItem().ConfigureAwait(false);
            }

            await Application.Current.Dispatcher.BeginInvoke(() =>
            {
                if (!AreWorkItemsEqualNoID(WorkItem, updatedWorkItem) || updatedWorkItem.ID == 0)
                {
                    mContext.DialogBuilder.WarningDialog("Work Item conflicts with an existing Work Item");
                    WorkItem = updatedWorkItem;
                }
                else
                {
                    WorkItem = updatedWorkItem;
                    mContext.WorkItemMediator.UpdateWorkItem(WorkItem);
                    mContext.WorkItemMediator.AddItemToCollection(WorkItem);
                }
            });
        } 

        /// <summary>
        /// Aktualizuje WorkItem w bazie danych
        /// </summary>
        /// <returns></returns>
        public async Task<WorkItemPublic> UpdateWorkItem()
        {
            try
            {
                HttpWorkItemClient client = new HttpWorkItemClient();

                WorkItemPublic updatedWorkItem = await client.UpdateWorkItem(WorkItem).ConfigureAwait(false);

                return updatedWorkItem;
            }
            catch (Exception)
            {
                mContext.DialogBuilder.ErrorDialog("Work Item couldn't be updated, due to server error.");
                return null;
            }
        }

        /// <summary>
        /// Tworzy WorkItem w bazie danych
        /// </summary>
        /// <returns></returns>
        public async Task<WorkItemPublic> CreateWorkItem()
        {
            try
            {
                HttpWorkItemClient client = new HttpWorkItemClient();

                WorkItemPublic updatedWorkItem = await client.CreateWorkItem(WorkItem).ConfigureAwait(false);

                return updatedWorkItem;
            }
            catch (Exception)
            {
                mContext.DialogBuilder.ErrorDialog("Work Item couldn't be updated, due to server error.");
                return null;
            }
        }

        /// <summary>
        /// sprawdza czy 2 workItemy przechowują te same dane
        /// </summary>
        private bool AreWorkItemsEqualNoID(WorkItemPublic one, WorkItemPublic two)
        {
            if (one.Name == two.Name && one.Description == two.Description &&
                one.Color == two.Color && one.WorkItemStart == two.WorkItemStart && one.WorkItemEnd == two.WorkItemEnd
                && one.WorkItemCollectionID == two.WorkItemCollectionID)
            {
                return true;
            }
            return false;
        }
    }
}
