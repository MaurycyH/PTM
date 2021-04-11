using PTM.Entities;
using PTM.PublicDataModel;
using System;
using System.Collections.Generic;
using System.Text;
using Tesseract.Common;

namespace PTM.Logic.ModelConverters
{
    public class WorkItemCollectionConverter : IModelConverter<WorkItemCollection, WorkItemCollectionPublic>
    {
        private IDatabaseContext mDBContext;

        public WorkItemCollectionConverter(IDatabaseContext context)
        {
            Ensure.ParamNotNull(context, nameof(context));

            mDBContext = context;
        }

        /// <inheritdoc/>
        public WorkItemCollection Convert(WorkItemCollectionPublic source)
        {
            Ensure.ParamNotNull(source, nameof(source));

            WorkItemCollection destination = mDBContext.WorkItemCollections.Find(source.ID);

            if (destination == null)
            {
                destination = new WorkItemCollection();
            }

            destination.Name = source.Name;
            destination.TaskBoard = mDBContext.TaskBoards.Find(source.TaskBoardID);
            destination.TaskBoardId = source.TaskBoardID;

            return destination;
        }

        /// <inheritdoc/>
        public WorkItemCollectionPublic Convert(WorkItemCollection source)
        {
            Ensure.ParamNotNull(source, nameof(source));

            WorkItemCollectionPublic destination = new WorkItemCollectionPublic()
            {
                ID = source.ID,
                Name = source.Name,
                TaskBoardID = source.TaskBoardId
            };

            return destination;
        }
    }
}