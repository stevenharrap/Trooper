namespace Trooper.Ui.Mvc.Rabbit.Models.Table
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web.Helpers;
    using Trooper.Ui.Mvc.Rabbit.Models.Table;
    using Trooper.Ui.Mvc.Rabbit.Props;
    using Trooper.Ui.Mvc.Utility;
    using Trooper.Utility;

    public class TableModel
    {
        private PersistedTableModel persistedModel;

        public string PersistedData { get; set; }

        public PersistedTableModel GetPersistedModel<T>(TableProps<T> tProps)
            where T : class
        {
            if (this.persistedModel == null)
            {
                try
                {
                    this.persistedModel = Json.Decode<PersistedTableModel>(this.PersistedData);
                }
                catch
                {
                    this.persistedModel = new PersistedTableModel();
                }

                if (this.persistedModel.Sorting == null && tProps != null)
                {
                    this.persistedModel.Sorting = (from c in tProps.Columns
                                                   where c.SortIdentity != null
                                                   let k = ReflectionHelper.GetNameFromExpression<T>(c.SortIdentity)
                                                   let v = new PersistedSortInfo { Direction = c.SortDirection, Importance = c.SortImportance }
                                                   orderby c.SortImportance descending
                                                   select new { k, v }).ToDictionary(i => i.k, i => i.v);
                }
                else if (this.persistedModel.Sorting == null)
                {
                    this.persistedModel.Sorting = new Dictionary<string, PersistedSortInfo>();
                }


                this.persistedModel.Sorting = this.persistedModel.Sorting ?? new Dictionary<string, PersistedSortInfo>();
            }

            return this.persistedModel;
            
        }

        public PersistedSortInfo GetSortInfoFormColumn<T>(string column, TableProps<T> tProps)
            where T : class
        {
            var m = this.GetPersistedModel(tProps);

            if (m.Sorting.ContainsKey(column))
            {
                return m.Sorting[column];
            }

            return null;
        }
    }
}
