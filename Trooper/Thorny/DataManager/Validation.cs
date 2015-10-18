namespace Trooper.Thorny.DataManager
{
    using Microsoft.Practices.EnterpriseLibrary.Validation;
    using Trooper.Interface.Thorny.Business.Response;
    using Trooper.Thorny.Business.Response;
    using Trooper.Thorny.Interface.DataManager;
    using Trooper.Thorny.Utility;

    public class Validation<TEnt> : IValidation<TEnt> 
        where TEnt : class, new()
    {
        public IUnitOfWork Uow { get; set; }

        public bool IsValid(TEnt item)
        {
            return this.Validate(item, new Response());
        }

        public bool IsValid(TEnt item, IResponse response)
        {
            return  this.Validate(item, response);
        }

        public IResponse Validate(TEnt item)
        {
            var response = new Response();
            this.Validate(item, response);

            return response;
        }

        public virtual bool Validate(TEnt item, IResponse response)
        {
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
