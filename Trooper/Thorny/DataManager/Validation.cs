namespace Trooper.Thorny.DataManager
{
    using Microsoft.Practices.EnterpriseLibrary.Validation;
    using System.Collections.Generic;
    using Trooper.Interface.Thorny.Business.Response;
    using Trooper.Thorny.Business.Response;
    using Trooper.Thorny.Interface.DataManager;
    using Trooper.Thorny.Utility;
    using System;
    using Business.Operation.Core;
    using System.Linq;

    public class Validation<TEnt> : IValidation<TEnt> 
        where TEnt : class, new()
    {
        public IUnitOfWork Uow { get; set; }

        public bool IsValid(TEnt item)
        {
            return this.Validate(item, new Response());
        }

        public bool AreValid(IEnumerable<TEnt> items)
        {
            return this.Validate(items, new Response());
        }

        public bool IsValid(TEnt item, IResponse response)
        {
            return this.Validate(item, response);
        }

        public bool AreValid(IEnumerable<TEnt> items, IResponse response)
        {
            return this.Validate(items, response);
        }

        public IResponse Validate(TEnt item)
        {
            var response = new Response();
            this.Validate(item, response);

            return response;
        }

        public IResponse Validate(IEnumerable<TEnt> items)
        {
            var response = new Response();
            this.Validate(items, response);

            return response;
        }

        public virtual bool Validate(TEnt item, IResponse response)
        {
            if (item == null)
            {
                MessageUtility.Errors.Add("The item in null", BusinessCore.NullDataCode, response);
            }

            //Todo: implement default validation

            //var vr = Microsoft.Practices.EnterpriseLibrary.Validation.Validation.Validate(item);

            //foreach (var v in vr)
            //{
            //    MessageUtility.Errors.Add(v.Message, InvalidPropertyCode, v.Target, v.Key, response);
            //}

            return response.Ok;
        }

        public virtual bool Validate(IEnumerable<TEnt> items, IResponse response)
        {
            if (items == null)
            {
                MessageUtility.Errors.Add("The item in null", BusinessCore.NullDataCode, response);
                return response.Ok;
            }
            
            items.All((i) => { this.Validate(i, response); return response.Ok; });
                 
            //Todo: implement default validation

            //var vr = Microsoft.Practices.EnterpriseLibrary.Validation.Validation.Validate(item);

            //foreach (var v in vr)
            //{
            //    MessageUtility.Errors.Add(v.Message, InvalidPropertyCode, v.Target, v.Key, response);
            //}

            return response.Ok;
        }

    }
}
