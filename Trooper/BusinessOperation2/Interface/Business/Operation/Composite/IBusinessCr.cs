namespace Trooper.BusinessOperation2.Interface.Business.Operation.Composite
{
    using Trooper.BusinessOperation2.Interface.Business.Operation.Single;

    public interface IBusinessCr<Tc, Ti> : IBusinessCreate<Tc, Ti>, IBusinessRead<Tc, Ti>  
        where Tc : class, Ti, new()
        where Ti : class
    {
    }
}
