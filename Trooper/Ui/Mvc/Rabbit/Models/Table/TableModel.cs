namespace Trooper.Ui.Mvc.Rabbit.Models.Table
{
    using PagedList;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Linq.Dynamic;
    using System.Linq.Expressions;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web.Helpers;
    using Trooper.Ui.Interface.Mvc.Rabbit.Table;
    using Trooper.Ui.Mvc.Rabbit.Models.Table;
    using Trooper.Ui.Mvc.Rabbit.Props;
    using Trooper.Ui.Mvc.Rabbit.Props.Table;
    using Trooper.Ui.Mvc.Utility;
    using Trooper.Utility;

    public class TableModel<T> where T : class
    {
        public TableModel()
        {
            this.PageNumber = 1;
            this.Selected = new List<T>();
        }
        
        private int pageNumber;

        private IEnumerable<T> source;

        private IList<IColumn<T>> sorting;

        private PersistedTableModel persistedModel;

        public string PersistedData
        {
            get
            {
                return null;
            }

            set
            {
                this.persistedModel = Json.Decode<PersistedTableModel>(value);

                this.PageNumber = this.persistedModel.PageNumber;

                if (this.PageNumber <= 0)
                {
                    this.PageNumber = 1;
                }

                this.BindSorting();
            }
        }

        [ReadOnly(true)]
        public IEnumerable<T> Source
        {
            get
            {
                return this.source;
            }

            set
            {
                this.source = value;

                this.BindSelected();
            }
        }

        [ReadOnly(true)]
        public IList<T> Selected { get; set; }

        [ReadOnly(true)]
        public int PageNumber 
        { 
            get
            {
                return this.pageNumber;
            }

            set
            {
                if (value > 0)
                {
                    this.pageNumber = value;
                }
            }
        }

        [ReadOnly(true)]
        public IList<IColumn<T>> Sorting
        {
            get
            {
                return this.sorting;
            }

            set
            {
                this.sorting = value;

                this.BindSorting();
            }
        }

        private void BindSorting()
        {
            if (this.sorting == null || this.persistedModel == null || this.persistedModel.Sorting == null)
            {
                return;
            }

            foreach (var ps in this.persistedModel.Sorting.Where(s => s.Value != null))
            {
                var column = this.sorting.FirstOrDefault(c => ps.Key != null && c.SortIdentityName == ps.Key);

                if (column != null)
                {
                    column.SortImportance = ps.Value.Importance;
                    column.SortDirection = ps.Value.Direction;
                }
            }
        }

        private void BindSelected()
        {
            if (this.source == null || this.persistedModel == null || this.persistedModel.Selected == null)
            {
                return;
            }

            this.Selected = new List<T>();

            foreach (var s in this.persistedModel.Selected)
            {
                var p = 0;

                var where = s.Select(i => string.Format("{0}=@{1}", i.Key, p++));
                var whereQuery = string.Join(" AND ", where);
                var whereParams = s.Select(i => i.Value).ToArray<object>();

                var found = this.source.AsQueryable().Where(whereQuery, whereParams).FirstOrDefault();

                if (found != null)
                {
                    this.Selected.Add(found);
                }

                p = 0;
            }
        }
    }
}
