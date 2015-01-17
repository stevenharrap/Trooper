namespace Trooper.BusinessOperation2.DataManager
{
    using Microsoft.Practices.EnterpriseLibrary.Validation;
    using Trooper.BusinessOperation2.Interface.DataManager;
    using Trooper.BusinessOperation2.Interface.OperationResponse;
    using Trooper.BusinessOperation2.Utility;

    public class Validation<Tc> : IValidation<Tc> 
        where Tc : class, new()
    {
        public IUnitOfWork Uow { get; set; }

        public bool IsValid(Tc item)
        {
            var result = this.Validate(item, new OperationResponse.Response());

            return result.Ok;
        }

        public bool IsValid(Tc item, IResponse response)
        {
            var result = this.Validate(item, response);

            return result.Ok;
        }

        public IResponse Validate(Tc item)
        {
            var response = new OperationResponse.Response();
            var result = this.Validate(item, response);

            return result;
        }

        public virtual IResponse Validate(Tc item, IResponse response)
        {
            var vr = Validation.Validate(item);

            foreach (var v in vr)
            {
                MessageUtility.Errors.Add(v.Message, v.Target, v.Key, response);
            }

            return response;
        }
    }
}
