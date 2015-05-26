using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trooper.Interface.Thorny.Business.Security;

namespace Trooper.Thorny.Business.Security
{
	public class Behaviour : IBehaviour
	{
		public string Action { get; set; }

		public bool Allow { get; set; }
	}
}
