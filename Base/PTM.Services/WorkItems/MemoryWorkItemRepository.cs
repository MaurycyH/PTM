using PTM.PublicDataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tesseract.Common;

namespace PTM.Services.WorkItems
{
    public class MemoryWorkItemRepository : IWorkItemRepository
    {
        protected ICollection<WorkItemPublic> mWorkItems;

        /// <summary>
        /// Domysłny ctor. Inicjalizuje kolekcję workitemów.
        /// </summary>
        public MemoryWorkItemRepository()
        {
            if (mWorkItems == null)
            {
                mWorkItems = new List<WorkItemPublic>();
            }
        }

        /// <summary>
        /// Ctor pozwalający przekazać konkretną listę obiektów.
        /// </summary>
        /// <param name="workItems">Lista obietków</param>
        public MemoryWorkItemRepository(ICollection<WorkItemPublic> workItems)
        {
            Ensure.ParamNotNull(workItems, nameof(workItems));

            mWorkItems = workItems;
        }

        /// <inheritdoc/>
        public WorkItemPublic CreateWorkItem(WorkItemPublic workItem)
        {
            mWorkItems.Add(workItem);
            return workItem;
        }

        /// <inheritdoc/>
        public void DeleteWorkItem(int ID)
        {
            WorkItemPublic workItem = this.GetWorkItem(ID);

            mWorkItems.Remove(workItem);
        }

        public async Task<IEnumerable<WorkItemPublic>> GetAllWorkItemFromUserFromDay(int ID, DateTime date)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<WorkItemPublic> GetAllWorkItems(int ID)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public WorkItemPublic GetWorkItem(int ID)
        {
            return mWorkItems.FirstOrDefault(wi => wi.ID == ID);
        }

        /// <inheritdoc/>
        public WorkItemPublic UpdateWorkItem(WorkItemPublic workItem)
        {
            return workItem;
        }
    }
}
