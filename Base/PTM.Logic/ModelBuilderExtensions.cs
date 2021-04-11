using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Text;
using Tesseract.Common;

namespace PTM.Logic
{
    /// <summary>
    /// Klasa odpowiadająca za ucinanie "s" przez sam Entity Framework. Szukaj Pluralism Entity Framework
    /// </summary>
    public static class ModelBuilderExtensions
    {
        public static void RemovePluralizingTableNameConvention(this ModelBuilder modelBuilder)
        {
            Ensure.ParamNotNull(modelBuilder, nameof(modelBuilder));
            foreach (IMutableEntityType entity in modelBuilder.Model.GetEntityTypes())
            {
                entity.SetTableName(entity.DisplayName());
            }
        }
    }
}
