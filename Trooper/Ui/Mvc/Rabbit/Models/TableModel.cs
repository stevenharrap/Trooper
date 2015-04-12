namespace Trooper.Ui.Mvc.Rabbit.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web.Helpers;
    using Trooper.Utility;

    public class TableModel
    {
        private int pageNumber;

        public string PersistedData { get; set; }

        [ReadOnly(true)]
        private bool Persisted { get; set; }

        public int PageNumber
        {
            get
            {
                if (this.Persisted)
                {
                    this.Persist();
                }

                return this.pageNumber;
            }

            set
            {
                if (!this.Persisted)
                {
                    this.Persist();
                }

                this.pageNumber = value;
            }
        }

        public TableModel()
        {
            this.pageNumber = 1;
        }

        private void Persist()
        {
            this.Persisted = true;

            if (this.PersistedData == null)
            {
                return;
            }

            var data = Json.Decode(this.PersistedData);

            this.pageNumber = Conversion.ConvertToInt(data.PageNumber, 1);
        }
    }
}
