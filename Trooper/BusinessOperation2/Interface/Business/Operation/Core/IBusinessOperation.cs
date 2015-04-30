namespace Trooper.BusinessOperation2.Interface.Business.Operation.Core
{
    using Trooper.BusinessOperation2.Interface.Business.Security;

    public interface IBusinessOperation<Tc, Ti>
        where Tc : class, Ti, new()
        where Ti : class
    {
        IIdentity DefaultIdentity { get; set; }

        IBusinessCore<Tc, Ti> BusinessCore { get; set; }
    }
}
