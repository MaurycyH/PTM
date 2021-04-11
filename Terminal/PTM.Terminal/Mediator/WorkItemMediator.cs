using PTM.PublicDataModel;
using System;
using System.Collections.Generic;
using System.Text;
using Tesseract.Common;

namespace PTM.Terminal.Mediator
{
    public class WorkItemMediator
    {
        public event OnWorkItemUpdated WorkItemUpdatedEvent;
        public event OnWorkItemsUpdated WorkItemsUpdatedEvent;
        public event OnCollectionUpdated CollectionUpdatedEvent;
        public event OnCollectionAddItem CollectionAddItemEvent;
        public delegate void OnWorkItemUpdated(WorkItemMediator m, WorkItemEventArgs e);
        public delegate void OnWorkItemsUpdated(WorkItemMediator m);
        public delegate void OnCollectionUpdated(WorkItemMediator m, WorkItemCollectionEventArgs e);
        public delegate void OnCollectionAddItem(WorkItemMediator m, WorkItemEventArgs e);

        /// <summary>
        /// Wysyła wiadomość o aktualizacji workitema
        /// </summary>
        public void UpdateWorkItem(WorkItemPublic workItem)
        {
            if (WorkItemUpdatedEvent == null) return;

            Ensure.ParamNotNull(workItem, nameof(workItem));

            WorkItemEventArgs wia = new WorkItemEventArgs() { WorkItem = workItem };
            WorkItemUpdatedEvent(this, wia);
        }

        /// <summary>
        /// Wysyła wiadomość do harmonogramu o dużych zmianach
        /// </summary>
        public void UpdateWorkItems()
        {
            if (WorkItemsUpdatedEvent == null) return;

            WorkItemsUpdatedEvent(this);
        }

        /// <summary>
        /// Wysyła wiadomość o aktualizacji kolekcji
        /// </summary>
        public void UpdateCollection(WorkItemCollectionPublic collection)
        {
            if (CollectionUpdatedEvent == null) return;

            Ensure.ParamNotNull(collection, nameof(collection));

            WorkItemCollectionEventArgs wice = new WorkItemCollectionEventArgs() { Collection = collection };
            CollectionUpdatedEvent(this, wice);
        }

        /// <summary>
        /// Wysyła wiadomość o aktualizacji workitema
        /// </summary>
        public void AddItemToCollection(WorkItemPublic workItem)
        {
            if (CollectionAddItemEvent == null) return;

            Ensure.ParamNotNull(workItem, nameof(workItem));

            WorkItemEventArgs wia = new WorkItemEventArgs() { WorkItem = workItem };
            CollectionAddItemEvent(this, wia);
        }

        /// <summary>
        /// Kasuje handlery przypisane do OnCollectionUpdated
        /// </summary>
        public void ClearCollectionEvents()
        {
            CollectionUpdatedEvent = null;
            CollectionAddItemEvent = null;
        }
    }

    public class WorkItemEventArgs : EventArgs
    {
        private WorkItemPublic mWorkItem;
        public WorkItemPublic WorkItem
        {
            set
            {
                mWorkItem = value;
            }
            get
            {
                return this.mWorkItem;
            }
        }
    }
    public class WorkItemCollectionEventArgs : EventArgs
    {
        private WorkItemCollectionPublic mCollection;
        public WorkItemCollectionPublic Collection
        {
            set
            {
                mCollection = value;
            }
            get
            {
                return this.mCollection;
            }
        }
    }
}
