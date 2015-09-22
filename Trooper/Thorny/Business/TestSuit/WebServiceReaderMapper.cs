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
    public class WebServiceReaderMapper<TPoco> : IBusinessRead<TPoco>
        where TPoco : class
    {
        private object webserviceReference;

        public WebServiceReaderMapper(object webserviceReference)
        {
            this.webserviceReference = webserviceReference;
        }

        public ISingleResponse<bool> IsAllowed(IRequestArg<TPoco> argument, IIdentity identity)
        {
            var t = webserviceReference.GetType();
            var n = nameof(IBusinessRead<TPoco>.IsAllowed);
            var m = t.GetMethod(n);

            if (m == null)
            {
                throw new MissingMethodException(string.Format("The '{0}' was expected", n));
            }

            var result = m.Invoke(webserviceReference, new object[] { argument, identity });
            return result as ISingleResponse<bool>;
        }

        ISingleResponse<bool> IBusinessRead<TPoco>.ExistsByKey(TPoco item, IIdentity identity)
        {
            var t = webserviceReference.GetType();
            var n = nameof(IBusinessRead<TPoco>.ExistsByKey);
            var m = t.GetMethod(n);

            if (m == null)
            {
                throw new MissingMethodException(string.Format("The '{0}' was expected", n));
            }

            var result = m.Invoke(webserviceReference, new object[] { item, identity });
            return result as ISingleResponse<bool>;
        }

        IManyResponse<TPoco> IBusinessRead<TPoco>.GetAll(IIdentity identity)
        {
            var t = webserviceReference.GetType();
            var n = nameof(IBusinessRead<TPoco>.GetAll);
            var m = t.GetMethod(n);

            if (m == null)
            {
                throw new MissingMethodException(string.Format("The '{0}' was expected", n));
            }

            var result = m.Invoke(webserviceReference, new object[] { identity });
            return result as IManyResponse<TPoco>;
        }

        ISingleResponse<TPoco> IBusinessRead<TPoco>.GetByKey(TPoco item, IIdentity identity)
        {
            var t = webserviceReference.GetType();
            var n = nameof(IBusinessRead<TPoco>.GetByKey);
            var m = t.GetMethod(n);

            if (m == null)
            {
                throw new MissingMethodException(string.Format("The '{0}' was expected", n));
            }

            var result = m.Invoke(webserviceReference, new object[] { item, identity });
            return result as ISingleResponse<TPoco>;
        }

        IManyResponse<TPoco> IBusinessRead<TPoco>.GetSome(ISearch search, IIdentity identity)
        {
            var t = webserviceReference.GetType();
            var n = nameof(IBusinessRead<TPoco>.GetSome);
            var m = t.GetMethod(n);

            throw new NotImplementedException(string.Format("GetSome not implememted yet..."));
        }        
    }
}
