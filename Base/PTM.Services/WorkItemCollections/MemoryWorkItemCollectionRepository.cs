using PTM.PublicDataModel;
using PTM.Services.WorkItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tesseract.Common;

namespace PTM.Services.WorkItemCollections
{
    public class MemoryWorkItemCollectionRepository : IWorkItemCollectionsRepository
    {
        protected ICollection<WorkItemCollectionPublic> mWorkItemCollections;
        
        /// <summary>
        /// Domysłny ctor. Inicjalizuje kolekcję workitemów.
        /// </summary>
        public MemoryWorkItemCollectionRepository()
        {
            mWorkItemCollections = new List<WorkItemCollectionPublic>();
        }

        /// <summary>
        /// Ctor pozwalający przekazać konkretną listę obiektów.
        /// </summary>
        /// <param name="workItemCollection">Lista obietków</param>
        public MemoryWorkItemCollectionRepository(ICollection<WorkItemCollectionPublic> workItemCollection)
        {
            Ensure.ParamNotNull(workItemCollection, nameof(workItemCollection));

            mWorkItemCollections = workItemCollection;
        }

        /// <inheritdoc/>
        public WorkItemCollectionPublic CreateWorkItemCollection(WorkItemCollectionPublic workItemCollection)
        {
            Ensure.ParamNotNull(workItemCollection, nameof(workItemCollection));

            mWorkItemCollections.Add(workItemCollection);

            return workItemCollection;
        }

        /// <inheritdoc/>
        public void DeleteWorkItemCollection(int ID)
        {
            mWorkItemCollections.Remove(this.GetWorkItemCollection(ID));
        }

        public IEnumerable<WorkItemCollectionPublic> GetAllWorkItemCollections(int ID)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public WorkItemCollectionPublic GetWorkItemCollection(int ID)
        {
            return mWorkItemCollections.FirstOrDefault(wic => wic.ID == ID);
        }

        /// <inheritdoc/>
        public WorkItemCollectionPublic UpdateWorkItemCollection(WorkItemCollectionPublic workItemCollection)
        {
            return workItemCollection;
        }
    }
}
