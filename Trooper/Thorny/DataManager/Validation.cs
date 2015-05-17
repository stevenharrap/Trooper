namespace Trooper.Thorny.DataManager
{
    using Microsoft.Practices.EnterpriseLibrary.Validation;
    using Trooper.Thorny.Interface.DataManager;
    using Trooper.Thorny.Interface.OperationResponse;
    using Trooper.Thorny.Utility;

    public class Validation<Tc> : Validation, IValidation<Tc> 
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
            //var vr = Microsoft.Practices.EnterpriseLibrary.Validation.Validation.Validate(item);

            //foreach (var v in vr)
            //{
            //    MessageUtility.Errors.Add(v.Message, InvalidPropertyCode, v.Target, v.Key, response);
            //}

            return response;
        }
    }

    public class Validation
    {
        public const string InvalidPropertyCode = Constants.ValidationErrorCodeRoot + ".InvalidProperty";
    }
}
