﻿namespace Trooper.Thorny.Interface.DataManager
{
    using Trooper.Thorny.Interface.OperationResponse;

    public interface IValidation<TEnt> 
        where TEnt : class, new()
    {
        IUnitOfWork Uow { get; set; }

        bool IsValid(TEnt item);

        bool IsValid(TEnt item, IResponse response);

        IResponse Validate(TEnt item);

        IResponse Validate(TEnt item, IResponse response);        
    }
}
