namespace Trooper.Thorny.Interface.DataManager
{
    using Trooper.Thorny.Interface.OperationResponse;

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
