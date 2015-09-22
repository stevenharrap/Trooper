using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trooper.Interface.Thorny.Business.Operation.Single;
using Trooper.Interface.Thorny.Business.Response;
using Trooper.Interface.Thorny.Business.Security;
using Trooper.Thorny.Interface.DataManager;

namespace Trooper.Thorny.Business.TestSuit
{
    public class WebServiceCreaterMapper<TPoco> : IBusinessCreate<TPoco>
        where TPoco : class
    {
        private object webserviceReference;

        public WebServiceCreaterMapper(object webserviceReference)
        {
            this.webserviceReference = webserviceReference;
        }

        public ISingleResponse<bool> IsAllowed(IRequestArg<TPoco> argument, IIdentity identity)
        {
            var t = webserviceReference.GetType();
            var n = nameof(IBusinessCreate<TPoco>.IsAllowed);
            var m = t.GetMethod(n);

            if (m == null)
            {
                throw new MissingMethodException(string.Format("The '{0}' was expected", n));
            }

            var result = m.Invoke(webserviceReference, new object[] { argument, identity });
            return result as ISingleResponse<bool>;
        }

        IAddResponse<TPoco> IBusinessCreate<TPoco>.Add(TPoco item, IIdentity identity)
        {
            var t = webserviceReference.GetType();
            var n = nameof(IBusinessCreate<TPoco>.Add);
            var m = t.GetMethod(n);

            if (m == null)
            {
                throw new MissingMethodException(string.Format("The '{0}' was expected", n));
            }

            var result = m.Invoke(webserviceReference, new object[] { item, identity });
            return result as IAddResponse<TPoco>;
        }

        IAddSomeResponse<TPoco> IBusinessCreate<TPoco>.AddSome(IEnumerable<TPoco> items, IIdentity identity)
        {
            var t = webserviceReference.GetType();
            var n = nameof(IBusinessCreate<TPoco>.AddSome);
            var m = t.GetMethod(n);

            if (m == null)
            {
                throw new MissingMethodException(string.Format("The '{0}' was expected", n));
            }

            var result = m.Invoke(webserviceReference, new object[] { items, identity });
            return result as IAddSomeResponse<TPoco>;
        } 
    }
}
