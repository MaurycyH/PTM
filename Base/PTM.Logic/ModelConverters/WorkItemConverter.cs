using Microsoft.EntityFrameworkCore;
using PTM.Entities;
using PTM.PublicDataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tesseract.Common;

namespace PTM.Logic.ModelConverters
{
    public class WorkItemConverter : IModelConverter<WorkItem, WorkItemPublic>
    {
        private IDatabaseContext mDBContext;

        public WorkItemConverter(IDatabaseContext context)
        {
            Ensure.ParamNotNull(context, nameof(context));

            mDBContext = context;
        }

        /// <inheritdoc/>
        public WorkItem Convert(WorkItemPublic source)
        {
            Ensure.ParamNotNull(source, nameof(source));

            WorkItem destination = mDBContext.WorkItems.AsNoTracking().FirstOrDefault(wi => wi.ID == source.ID);

            if (destination == null)
            {
                destination = new WorkItem();
            }

            destination.Name = source.Name;
            destination.Description = source.Description;
            destination.WorkItemCollection = mDBContext.WorkItemCollections.Find(source.WorkItemCollectionID);
            destination.WorkItemCollectionId = source.WorkItemCollectionID;
            destination.WorkItemStart = source.WorkItemStart;
            destination.WorkItemEnd = source.WorkItemEnd;
            destination.Color = source.Color;

            return destination;
        }

        /// <inheritdoc/>
        public WorkItemPublic Convert(WorkItem source)
        {
            Ensure.ParamNotNull(source, nameof(source));

            WorkItemPublic destination = new WorkItemPublic()
            {
                ID = source.ID,
                Name = source.Name,
                Color = source.Color,
                Description = source.Description,
                WorkItemCollectionID = source.WorkItemCollectionId,
                WorkItemEnd = source.WorkItemEnd,
                WorkItemStart = source.WorkItemStart
            };

            return destination;
        }
    }
}
