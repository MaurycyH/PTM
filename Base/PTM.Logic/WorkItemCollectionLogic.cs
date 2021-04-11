using PTM.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tesseract.Common;

namespace PTM.Logic
{
    /// <summary>
    /// Logika interkacji dla WorkItemCollection
    /// </summary>
    public class WorkItemCollectionLogic
    {
        private IDatabaseContext mDBContext;

        /// <summary>
        /// Domyślny ctor.
        /// </summary>
        /// <param name="dbContext">Kontekst</param>
        public WorkItemCollectionLogic(IDatabaseContext dbContext)
        {
            Ensure.ParamNotNull(dbContext, nameof(dbContext));

            mDBContext = dbContext;
        }

        public WorkItemCollection CreateWorkItemCollection(WorkItemCollection workItemCollection)
        {
            Ensure.ParamNotNull(workItemCollection, nameof(workItemCollection));

            if (workItemCollection.ID != 0)
            {
                throw new ArgumentException($"ID can't be set on entity. The provided ID is {workItemCollection.ID}");
            }

            if (!mDBContext.TaskBoards.Any(tb => tb.ID == workItemCollection.TaskBoardId))
            {
                throw new ArgumentException($"The specified TaskBoard doesn't exist - {workItemCollection.TaskBoardId}");
            }

            return mDBContext.WorkItemCollections.Add(workItemCollection).Entity;
        }

        public WorkItemCollection GetWorkItemCollection(int ID)
        {
            if (ID <= 0)
            {
                throw new AggregateException($"Parameter have to be grater than 0. The provided value is {ID}.");
            }

            return mDBContext.WorkItemCollections.FirstOrDefault(wi => wi.ID == ID);
        }

        public WorkItemCollection UpdateWorkItemCollection(WorkItemCollection workItemCollection)
        {
            Ensure.ParamNotNull(workItemCollection, nameof(workItemCollection));

            if (workItemCollection.ID <= 0)
            {
                throw new AggregateException($"Parameter {nameof(workItemCollection)} ID's have to be grater than 0. The provided value is {workItemCollection.ID}.");
            }

            WorkItemCollection result = mDBContext.WorkItemCollections.FirstOrDefault(wi => wi.ID == workItemCollection.ID);

            if (result == null)
            {
                throw new AggregateException($"WorkItemCollection with ID {workItemCollection.ID} does not exist.");
            }

            result.Name = workItemCollection.Name;
            result.TaskBoardId = workItemCollection.TaskBoardId;

            return result;
        }

        public void DeleteWorkItemCollection(int ID)
        {
            WorkItemCollection collection = mDBContext.WorkItemCollections.Find(ID);

            if (collection == null)
            {
                return;
            }

            mDBContext.WorkItems.RemoveRange(collection.WorkItems);
            mDBContext.WorkItemCollections.Remove(collection);
        }
    }
}
