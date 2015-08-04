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
            var result = this.Validate(item, new Response());

            return result.Ok;
        }

        public bool IsValid(TEnt item, IResponse response)
        {
            var result = this.Validate(item, response);

            return result.Ok;
        }

        public IResponse Validate(TEnt item)
        {
            var response = new Response();
            var result = this.Validate(item, response);

            return result;
        }

        public virtual IResponse Validate(TEnt item, IResponse response)
        {
            //var vr = Microsoft.Practices.EnterpriseLibrary.Validation.Validation.Validate(item);

            //foreach (var v in vr)
            //{
            //    MessageUtility.Errors.Add(v.Message, InvalidPropertyCode, v.Target, v.Key, response);
            //}

            return response;
        }
    }
}
