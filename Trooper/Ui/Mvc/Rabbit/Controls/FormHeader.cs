using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trooper.Ui.Mvc.Rabbit.Controls
{
    public enum Method
    {
        Post,
        Get
    }

    public class FormHeader : FormControl
    {
        public FormHeader()
        {
            this.Method = Controls.Method.Post;
        }

        public string Action { get; set; }

        public Method Method { get; set; }
    }
}
