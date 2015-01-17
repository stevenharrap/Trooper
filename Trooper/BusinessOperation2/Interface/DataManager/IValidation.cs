namespace Trooper.BusinessOperation2.Interface.DataManager
{
    using Trooper.BusinessOperation2.Interface.OperationResponse;

    public interface IValidation<Tc> 
        where Tc : class, new()
    {
        IUnitOfWork Uow { get; set; }

        bool IsValid(Tc item);

        bool IsValid(Tc item, IResponse response);

        IResponse Validate(Tc item);

        IResponse Validate(Tc item, IResponse response);        
    }
}
